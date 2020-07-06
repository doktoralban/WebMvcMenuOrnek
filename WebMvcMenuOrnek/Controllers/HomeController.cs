using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMvcMenuOrnek;

namespace WebMvcMenuOrnek.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        { 
            //--------------------------------------------------
            MenuXml menuRoleAppCode = new MenuXml();
            List<string> roleList;


            //if (TempData["MenuItems"] != null)
            //{
            //    //var oldMenuItems = TempData["MenuItems"] as List<BusinessObject.MenuItem.MenuItemObject>;
            //    //TempData["MenuItems"] = null;
            //    //TempData["MenuItems"] = oldMenuItems;
            //    ////return PartialView(oldMenuItems);
            //}

            //try
            //{
            //    //roleList = (new LoginModels(HttpContext.User)).Roller.ToList();
            //}
            //catch (Exception ex)
            //{
            //    //myLog.Info("Rol Alınamadı, Login olunmamış.", ex);               
            //}
             roleList = new List<string>();
            roleList.Add("Rol_3");

            var menuItems = menuRoleAppCode.GetMenuList(roleList);
            TempData["MenuItems"] = menuItems;
            Session["YetkiliMenuler"] = menuItems;
            //return PartialView(menuItems); 

            //--------------------------------------------------
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}