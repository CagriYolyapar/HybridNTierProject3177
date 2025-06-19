using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Bll.Managers.Abstracts;
using Project.Entities.Models;
using Project.MvcUI.Models.PageVms;
using Project.MvcUI.Models.SessionService;
using Project.MvcUI.Models.ShoppingTools;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Extensions;

namespace Project.MvcUI.Controllers
{
    public class ShoppingController : Controller
    {
        readonly IProductManager _productManager;
        readonly ICategoryManager _categoryManager;
        readonly IOrderManager _orderManager;
        readonly IOrderDetailManager _orderDetailManager;
        readonly UserManager<AppUser> _userManager;
        readonly IHttpClientFactory _httpClientFactory; //API icin simdiden gelecege yatırım amaclı entegre ettigimiz bir field

        public ShoppingController(IProductManager productManager, ICategoryManager categoryManager, IOrderManager orderManager, IOrderDetailManager orderDetailManager, UserManager<AppUser> userManager, IHttpClientFactory httpClientFactory)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _orderManager = orderManager;
            _orderDetailManager = orderDetailManager;
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
        }

        //ToDo : API icin sistem entegrasyonu yapılacak

        public async Task<IActionResult> Index(int? page,int? categoryId)
        {
            //string a = "Cagrı";

            //string b = a ?? "Deneme"; eger a null ise "Deneme" string verisini , null degilse a'nın kendi degerini b'ye ata demektir
            List<Product> productList = categoryId == null ? await _productManager.GetAllAsync() : _productManager.Where(x => x.CategoryId == categoryId).ToList();

            //IPagedList yine List gibi bir koleksiyon tipidir, List'ten farkı bu koleksiyonun icerisindeki elemanların Front End'de bir Pagination(Yani sayfalama sistemine) entegre olabilmesidir...
            IPagedList<Product> pagedProducts = productList.ToPagedList(page ?? 1 ,5);
            List<Category> categories = await _categoryManager.GetAllAsync();
            ShoppingPageVm spVm = new()
            {
                Products = pagedProducts,
                Categories = categories
            };

            if (categoryId != null) TempData["catId"] = categoryId;


            

            return View(spVm);
        }

        //Dikkat edin bu metotlar private'tir yani bir Request'e cevap vermek icin degil Controller'in görevini rahatlatmak icin özel olusturulmus yapılardır

        void SetCartForSession(Cart c)
        {
            HttpContext.Session.SetObject("scart", c);
        }

        Cart GetCartFromSession(string key)
        {
            return HttpContext.Session.GetObject<Cart>(key);
        }

        void ControlCart(Cart c)
        {
            if (c.GetCartItems.Count == 0) HttpContext.Session.Remove("scart");
          
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            Cart c = GetCartFromSession("scart") == null ? new Cart() : GetCartFromSession("scart");
            Product productToBeAdded = await _productManager.GetByIdAsync(id);

            CartItem ci = new()
            {
                Id = productToBeAdded.Id,
                ProductName = productToBeAdded.ProductName,
                UnitPrice = productToBeAdded.UnitPrice,
                ImagePath = productToBeAdded.ImagePath,
                CategoryId = productToBeAdded.CategoryId,
                CategoryName = productToBeAdded.Category == null ? "Kategorisi yok" : productToBeAdded.Category.CategoryName
            };

            c.AddToCart(ci);
            SetCartForSession(c);
            TempData["Message"] = $"{ci.ProductName} isimli ürün sepete eklenmiştir";
            return RedirectToAction("Index");
        }

        public IActionResult CartPage()
        {
            if(GetCartFromSession("scart") == null)
            {
                TempData["Message"] = "Sepetiniz su anda bos";
                return RedirectToAction("Index");
            }
            Cart c = GetCartFromSession("scart");
            return View(c);
        }

        //Todo : Refactor Session ve Sepet işlemlerini icin yapılmalıdır

        public IActionResult RemoveFromCart(int id)
        {
            if(GetCartFromSession("scart") != null)
            {
                Cart c = GetCartFromSession("scart");
                c.RemoveFromCart(id);
                SetCartForSession(c);
                ControlCart(c);
            }

            return RedirectToAction("CartPage");
        }

        public IActionResult DecreaseCartItem(int id)
        {
            if(GetCartFromSession("scart") != null)
            {
                Cart c = GetCartFromSession("scart");
                c.Decrease(id);
                SetCartForSession(c);
                ControlCart(c);
            }

            return RedirectToAction("CartPage");
        }

        public IActionResult IncreaseCartItem(int id)
        {
            if (GetCartFromSession("scart") != null)
            {
                Cart c = GetCartFromSession("scart");
                c.IncreaseCartItem(id);
                SetCartForSession(c);
                ControlCart(c);
            }

            return RedirectToAction("CartPage");
        }

        
        public  IActionResult ConfirmOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(OrderRequestPageVm ovm)
        {
            Cart c = GetCartFromSession("scart");

            ovm.Order.Price = c.TotalPrice;

            #region APIBankaEntegrasyonu

            #endregion

            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
                ovm.Order.AppUserId = appUser.Id;
            }

            await _orderManager.CreateAsync(ovm.Order); //Kesinlikle Order nesnesini veritabanına ekleyen bu kodu yazmalısınız...Cünkü Order instance'inin id'si Identity'dir. Yani ancak ve ancak veritabanına eklendiginde otomatik olarak verilir...Yani benim Order'imin Id'sinin olusması icin önce Order'in veritabanında yaratılması gerekir...

            foreach(CartItem item in c.GetCartItems)
            {
                OrderDetail od = new();
                od.OrderId = ovm.Order.Id;
                od.ProductId = item.Id;
                od.Amount = item.Amount;
                od.UnitPrice = item.UnitPrice;

                await _orderDetailManager.CreateAsync(od);

                //ürün stoktan düsmeli ve productManager ürünü Update etmeli
            }

            TempData["Message"] = "Siparişiniz bize basarıyla ulasmıstır...Teşekkür ederiz";
            HttpContext.Session.Remove("scart"); //Session'i silme kodu
            return RedirectToAction("Index");

        }
    }
}
