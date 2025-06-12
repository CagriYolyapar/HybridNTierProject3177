using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Project.Bll.Managers.Abstracts;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PageVms;
using System.Threading.Tasks;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        readonly IProductManager _productManager;
        readonly ICategoryManager _categoryManager;

        public ProductController(IProductManager productManager, ICategoryManager categoryManager)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _productManager.GetAllAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            CreateProductPageVm cpVm = new()
            {
                Categories =  _categoryManager.GetActives()
            };
            return View(cpVm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductPageVm model,IFormFile formFile)
        {
            #region ResimKodlari
            Guid uniqueName = Guid.NewGuid();
            string extension = Path.GetExtension(formFile.FileName); //Dosyanın uzantısını ele gecirdik

            //if(extension != "png" || extension != "gif" || extension != "jpeg")
            //{

            //}

            //Db'inizin bilecegi yol icin hazırlıkları
            model.Product.ImagePath = $"/images/{uniqueName}{extension}";

            //Server'daki yol icin hazırlık(Resmin asıl kaydolacagı directory)..Otomatik olarak projenin bulundugu server'i tespit ettirmemiz lazım ki oraya kaydedilsin
            string path =$"{Directory.GetCurrentDirectory()}/wwwroot/{model.Product.ImagePath}" ;

            FileStream stream = new(path, FileMode.Create);
            formFile.CopyTo(stream);




            #endregion


            await _productManager.CreateAsync(model.Product);
            return RedirectToAction("Index");
        }

        //Update - Delete ödev olarak tamamlansın
    }
}
