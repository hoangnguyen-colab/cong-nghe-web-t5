using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace cong_nghe_web.Controllers
{
    public class ShopController : Controller
    {
        [HttpGet]
        [Route("shop")]
        public ActionResult Shop(string search, string sort)
        {
            ViewBag.search = HttpUtility.UrlEncode(search);
            ViewBag.sort = sort;
            return View();
        }

        [HttpGet]
        [Route("thuong-hieu/{url}")]
        public async Task<ActionResult> ShopCategory(string url, string sort)
        {
            ViewBag.brand = await new BrandDAO().LoadByURL(url);
            ViewBag.sort = sort;
            return View();
        }

        [HttpGet]
        [Route("san-pham/{url}")]
        public async Task<ActionResult> Detail(string url)
        {
            try
            {
                var prod = await new ProductDAO().LoadByURL(url);

                // ViewData["ProductConfig"] = await new ProductDetailDAO().LoadSize(prod.ProductID);
                return View(prod);
            }
            catch
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult> ShopPartial(string url, string search, string sort, int pageindex)
        {
            search = HttpUtility.UrlDecode(search);
            int pagesize = 16;
            return PartialView("ShopPartial",
                await new ProductDAO().LoadProduct(url, search, sort, pagesize, pageindex));
        }

        [HttpPost]
        public async Task<JsonResult> GetProductName(string prefix)
        {
            var prod = await new ProductDAO().LoadName(prefix);

            return Json(prod, JsonRequestBehavior.AllowGet);
        }
    }
}