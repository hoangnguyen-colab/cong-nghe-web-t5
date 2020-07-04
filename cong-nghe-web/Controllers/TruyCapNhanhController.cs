using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cong_nghe_web.Controllers
{
    public class TruyCapNhanhController : Controller
    {
        // GET: TruyCapNhanh
        [Route("huong-dan")]
        public ActionResult HuongDanMuaHangOnLine()
        {
            return View();
        }
    }
}