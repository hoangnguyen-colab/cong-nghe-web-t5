using Models.DAO;
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
    public class ConfigurationController : Controller
    {
        // GET: Admin/Configuration
        public async Task<ActionResult> CreateConfigurationPartial(int id)
        {
            return PartialView("CreateConfigurationPartial",id);
        }

        public async Task<JsonResult> CreateConfiguration(CONFIGURATION model)
        {
            if (ModelState.IsValid)
            {
                var config = new CONFIGURATION
                {
                    ProductID = model.ConfigID,
                    OSName = model.OSName,
                    OSVersion = model.OSVersion,
                    SizeDisplay = model.SizeDisplay,
                    FrontCamera = model.FrontCamera,
                    RearCamera = model.RearCamera,
                    Cpu = model.Cpu,
                    Ram = model.Ram,
                    Rom = model.Rom,
                    SimStatus = model.SimStatus,
                    Battery = model.Battery
                };
                int result = await new ConfigurationDAO().CreateConfiguration(config);
                return Json(new { Success = true, id = result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

    }
}