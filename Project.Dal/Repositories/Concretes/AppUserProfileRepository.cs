using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class AppUserProfileRepository(MyContext context):BaseRepository<AppUserProfile>(context),IAppUserProfileRepository
    {
    }




    //Biz artık class'lar ile acıkca calısmayacagız..Bizim acıkca calısacagımız yapılar Interfaceler olacak


    //Benim controller'im interface'imi tanıyacak...Neden class'ımı tanımayacak : Cünkü benim Controller'imin class'ımı tanıması onun o class'ın ait oldugu teknolojiye kitlenmesi anlamına gelir...Bir Interface , implementation icermedigi icin dogal olarak bir teknolojiye kitli degildir. Dolayısıyla Benim controller'imin interface'imi tanıması onun herhangi bir yere kitli olmadıgını belirtir...Bu da bana istedigim an cok ufak bir degişiklikle farklı farklı teknolojilere geciş yapabilmemi saglar...
}
