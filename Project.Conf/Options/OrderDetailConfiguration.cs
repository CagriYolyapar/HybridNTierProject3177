﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Conf.Options
{
    public class OrderDetailConfiguration : BaseConfiguration<OrderDetail>
    {
        public override void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            base.Configure(builder);

            //HasIndex, Sql'de Index olusturma görevini yapmanızı saglar...
            builder.HasIndex(x => new
            {
                x.OrderId,
                x.ProductId
            }).IsUnique();

            builder.Property(x => x.UnitPrice).HasColumnType("money");
            
        }
    }
}
