﻿using Models.DAO;
using cong_nghe_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace cong_nghe_web.Controllers
{
    public class CartController : Controller
    {
        [AllowAnonymous]
        [Route("cart")]
        public async Task<ActionResult> Cart()
        {
            return View(await GetCartItem());
        }

        [Authorize]
        [Route("checkout")]
        public async Task<ActionResult> Checkout()
        {
            if (Session["cart"] == null)
            {
                return RedirectToAction("Cart", "Cart");
            }
            var customer = HttpContext.User.Identity.Name;
            CustomerViewModel model = null;
            if (!string.IsNullOrEmpty(customer))
            {
                var item = await new CustomerDAO().LoadByUsername(customer);
                model = new CustomerViewModel()
                {
                    CustomerID = item.CustomerID,
                    CustomerUsername = item.CustomerUsername,
                    CustomerName = item.CustomerName,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone
                };
            }
            ViewData["Customer"] = model;
            return View(await GetCartItem());
        }

        [HttpPost]
        public JsonResult OrderNow(int prodId, int quantity)
        {
            if (Session["cart"] == null)
            {
                var cart = new List<CartSession>();
                cart.Add(new CartSession(prodId, quantity));
                Session["cart"] = cart;

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cart = (List<CartSession>)Session["cart"];
                int index = IsExist(prodId);
                if (index == -1)
                {
                    cart.Add(new CartSession(prodId, quantity));
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Delete(int id)
        {
            int index = IsExist(id);
            List<CartSession> cart = (List<CartSession>)Session["cart"];
            cart.RemoveAt(index);
            if (cart.Count == 0)
            {
                Session["cart"] = null;
            }
            return RedirectToAction("Cart", "Cart");
        }

        private int IsExist(int id)
        {
            var cart = (List<CartSession>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].ProductID == id)
                    return i;
            return -1;
        }

        public async Task<ActionResult> CartPartial()
        {
            return PartialView("_Cart", await GetCartItem());
        }

        public async Task<List<CartItemModel>> GetCartItem()
        {
            var list = new List<CartItemModel>();
            var session = (List<CartSession>)Session["cart"];
            if (session != null)
            {
                foreach (var item in session)
                {
                    list.Add(new CartItemModel(
                        await new ProductDAO().LoadByID(item.ProductID),
                        item.Quantity));
                }
            }
            return list;
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SubmitCheckout()
        {
            try
            {
                var customer = await new CustomerDAO().LoadByUsername(HttpContext.User.Identity.Name);
                var order = await new OrderDAO().AddOrder(customer.CustomerID, await GetTotal());
                if (order != 0)
                {
                    var orderdetail = new OrderDetailDAO();
                    foreach (var item in (List<CartSession>)Session["cart"])
                    {
                        await orderdetail.AddOrderDetail(order, item.ProductID, item.Quantity);
                    }
                    Session["cart"] = null;
                    return Json(new { Success = true, ID = order }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<decimal> GetTotal()
        {
            decimal total = 0;
            
            var cart = (List<CartSession>)Session["cart"];
            if (cart != null)
            {
                var dao = new ProductDAO();
                foreach (var item in cart)
                {
                    var product = await dao.LoadByID(item.ProductID);
                    if (product.PromotionPrice.HasValue)
                    {
                        total += product.PromotionPrice.Value * item.Quantity;
                    } else
                    {
                        total += product.ProductPrice * item.Quantity;
                    }
                }
            }
            return total;
        }
    }
}