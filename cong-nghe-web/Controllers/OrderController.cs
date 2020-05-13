using cong_nghe_web.Models;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace cong_nghe_webWeb.Controllers
{
    public class OrderController : Controller
    {
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> CancelOrder(int OrderID)
        {
            //5 - cancel
            var result = await new OrderDAO().ChangeOrder(OrderID, 5);
            if (result == 0)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<ActionResult> OrderList(int? StatusID)
        {
            var membername = HttpContext.User.Identity.Name;
            var customer = await new CustomerDAO().LoadByUsername(membername);

            //var list = await new OrderDAO().LoadOrder<OrderModel>(customer.CustomerID);

            var list = await new OrderDAO().LoadOrder(customer.CustomerID);

            if (!StatusID.Equals(0))
            {
                list = list.Where(x => x.OrderStatusID == StatusID).ToList();
            }

            return PartialView("OrderList", list);
        }

        [Authorize]
        public async Task<ActionResult> OrderProductList(int OrderID)
        {
            return PartialView("OrderProductList", await new OrderDAO().LoadProductOrder(OrderID));
        }

        public async Task<JsonResult> GetStatus()
        {
            var status = await new OrderStatusDAO().LoadStatus();
            return Json(status, JsonRequestBehavior.AllowGet);
        }
    }
}