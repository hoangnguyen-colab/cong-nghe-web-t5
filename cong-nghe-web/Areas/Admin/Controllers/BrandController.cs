using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace cong_nghe_web.Areas.Admin.Controllers
{
    public class BrandController : BaseController
    {
        public ActionResult CreateBrand()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateBrand(BRAND model)
        {
            if (ModelState.IsValid)
            {
                model.BrandURL = SlugGenerator.SlugGenerator.GenerateSlug(model.BrandName);
                model.CreatedDate = DateTime.Now;
                int result = await new BrandDAO().CreateBrand(model);
                return RedirectToAction("CreateBrand");
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> EditBrand(BRAND model, int id)
        {
            if (ModelState.IsValid)
            {
                model.BrandURL = SlugGenerator.SlugGenerator.GenerateSlug(model.BrandName);
                int result = await new BrandDAO().EditBrand(model, id);
                if (result == 0)
                {
                    return Json(new { Success = false, id }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = true, id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> BrandList()
        {
            return PartialView("BrandList", await new BrandDAO().LoadData());
        }

        [HttpPost]
        public async Task<JsonResult> DeleteBrand(int id)
        {
            if (await new BrandDAO().LoadByID(id) == null)
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (!(await new BrandDAO().DeleteBrand(id)))
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = 1 }, JsonRequestBehavior.AllowGet);
        }
    }
}