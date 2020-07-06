using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;


namespace WebMvcMenuOrnek
{
   
    public class MenuXml
    {
        //------------------------------------------------------------- 
         static  int  menuItemId = 0;
        int maxMainMenuItemCount = 999;
        //Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxMainMenuItemCount"].ToString());

        public static List<MenuItemObject> ReadMenuItems()
        {
            try
            {
                string menuListPath = System.Web.Hosting.HostingEnvironment.MapPath("~/MenuItems/xmlMenuOrnek1.xml");
                var document = XDocument.Load(menuListPath);
                List<MenuItemObject> list = LoadMenuItemObject(document.Descendants("MainNode").Elements("Node"));

                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        //------------------------------------------------------------- 
        public static List<MenuItemObject> LoadMenuItemObject(IEnumerable<XElement> units)
        { 
            try
            {
            return units.Select(x => new MenuItemObject()
                    {
                        Id = (menuItemId += 1),
                        Name = x.Attribute("title").Value,
                        Link = string.IsNullOrEmpty(x.Attribute("link").Value) ? "javascript:;" : x.Attribute("link").Value,
                        Icon = x.Attribute("icon").Value,
                        Roles = x.Attribute("roles").Value.Split(',').ToList(),
                        ChildMenuItems = LoadMenuItemObject(x.Elements("Node"))
                    }).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        //------------------------------------------------------------- 
        public List<MenuItemObject> GetMenuList(List<string> roleNames)
        {
            List<MenuItemObject> allMenuList = ReadMenuItems();
            List<MenuItemObject> menuList = new List<MenuItemObject>();
            MenuItemObject mainMenu = null;

            foreach (MenuItemObject item in allMenuList)
            {
                menuList.Add(GetMenuItemByRoles(item, roleNames));
            }

            //null elemanları kaldırıyor
            menuList.RemoveAll(m => m == null);

            if (menuList.Count >= maxMainMenuItemCount)
            {
                mainMenu = new MenuItemObject()
                {
                    ChildMenuItems = menuList.GetRange(0, menuList.Count),
                    Icon = "fa fa-bars",
                    Link = "javascript:;",
                    Name = "Uygulamalar"
                };

                menuList.Clear();
                menuList.Add(mainMenu);
            }

            return menuList;
        }
        //------------------------------------------------------------- 
        public MenuItemObject GetMenuItemByRoles(MenuItemObject menuObj, List<string> roleNames)
        {
            if (menuObj == null)
            {
                return null;
            }

            //Menü Elemanında Rol tanımlıysa  alt elemanlara iniyor
            //insersect: İki Listenin kesişimini alır
            if (menuObj.Roles.Intersect(roleNames).Count() > 0 || menuObj.Roles[0] == "*")
            {
                for (int i = 0; i < menuObj.ChildMenuItems.Count; i++)
                {
                    //Herbir çocuk için alt menüye iniyor
                    menuObj.ChildMenuItems[i] = GetMenuItemByRoles(menuObj.ChildMenuItems[i], roleNames);
                }

                //null elemanları kaldırıyor
                menuObj.ChildMenuItems.RemoveAll(m => m == null);

                return menuObj;
            }
            else
            {
                return null;
            }
        }
        //-------------------------------------------------------------
        public class MenuItemObject  
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Link { get; set; }
            public List<string> Roles { get; set; }
            public string Icon { get; set; }
            public List<MenuItemObject> ChildMenuItems { get; set; }
        }
        //------------------------------------------------------------- 

        public static IEnumerable<MenuItemObject> GetNodes(MenuItemObject node )
        {
            if (node == null)
            {
                yield break;
            }
           
                yield return node;
         
            foreach (var n in node.ChildMenuItems)
            {
                foreach (var innerN in GetNodes(n))
                {
                    yield return innerN;
                }
            }


        }
        //-------------------------------------------------------------
        public static IEnumerable<MenuItemObject> GetNodesSadeceEnUstNodes(MenuItemObject node)
        {
            if (node == null)
            {
                yield break;
            }

            yield return node;

            foreach (var n in node.ChildMenuItems)
            {
                
                    yield return n;
                 
            }


        }






    }
}