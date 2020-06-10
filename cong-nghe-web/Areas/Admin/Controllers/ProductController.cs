﻿using Models.DAO;
using Models.EF;
using cong_nghe_web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace cong_nghe_web.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class ProductController : BaseController
    {
        /// <summary>
        /// Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult DeleteProduct(int id)
        //{
        //    if (new ProductDAO().LoadByID(id) == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    if (!(new ProductDAO().Delete(id)))
        //    {
        //        return HttpNotFound();
        //    }
        //    return RedirectToAction("ProductList");
        //}

        [HttpPost]
        public async Task<JsonResult> DeleteProduct(int id)
        {
            if (await new ProductDAO().LoadByID(id) == null)
            {
                return Json(new { Success = 0}, JsonRequestBehavior.AllowGet);
            }
            if (!(await new ProductDAO().DeleteProduct(id)))
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = 1 }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ProductDetail(int id)
        {
            ViewBag.Brand = await new BrandDAO().LoadData();

            var item = await new ProductDAO().LoadByID(id);
            return View(item);
        }

        public async Task<ActionResult> ProductList()
        {
            return View(await new ProductDAO().LoadProduct());
        }

        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductPartial(int brandid)
        {
            return PartialView("CreateProductPartial", await new BrandDAO().LoadByID(brandid));
        }

        [HttpPost]
        public async Task<JsonResult> CreateProduct(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var prod = new PRODUCT
                {
                    ProductName = model.ProductName,
                    ProductPrice = model.ProductPrice,
                    ProductDescription = model.ProductDescription,
                    PromotionPrice = model.PromotionPrice,
                    ProductStock = model.ProductStock,
                    ProductURL = SlugGenerator.SlugGenerator.GenerateSlug(model.ProductName),
                    ProductImage = model.ProductImage,
                    ProductStatus = model.ProductStatus,
                    BrandID = model.BrandID,
                    CreatedDate = DateTime.Now
                };
                int result = await new ProductDAO().CreateProduct(prod);
                return Json(new { Success = true, id = result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}