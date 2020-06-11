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
        [Route("thuong-hieu/{url}-{id:int}")]
        public async Task<ActionResult> ShopCategory(int id, string url, string sort)
        {
            var brand = await new BrandDAO().LoadByID(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            ViewBag.brand = brand;
            ViewBag.sort = sort;
            return View();
        }

        [HttpGet]
        [Route("san-pham/{url}-{id:int}")]
        public async Task<ActionResult> Detail(int id, string url)
        {
            try
            {
                var dao = new ProductDAO();
                var prod = await dao.LoadByID(id);
                if (prod == null)
                    return HttpNotFound();
                await dao.UpdateViewCount(prod.ProductID);
                return View(prod);
            }
            catch
            {
                return HttpNotFound();
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> ConfigDetail(int id)
        {
            try
            {
                var config = await new ConfigurationDAO().LoadConfig(id);
                if (config == null)
                {
                    return HttpNotFound();
                }
                return PartialView("ConfigDetail", config);
            }
            catch
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> ShopPartial(int? brandid, string search, string sort, int pageindex)
        {
            search = HttpUtility.UrlDecode(search);
            int pagesize = 16;
            return PartialView("ShopPartial",
                await new ProductDAO().LoadProduct(brandid, search, sort, pagesize, pageindex));
        }

        [HttpPost]
        public async Task<JsonResult> GetProductName(string prefix)
        {
            return Json(new { name = await new ProductDAO().LoadName(prefix) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SelectTop(string cond)
        {
            int top = 8;
            var list = await new ProductDAO().SelectCondition(cond, top);
            return PartialView("SelectTop", list);
        }
    }
}