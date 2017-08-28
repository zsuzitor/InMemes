using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using System.Web.Mvc.Ajax;
using Im.Models;
using Microsoft.AspNet.Identity.EntityFramework;


/*
 * в представление загрузка картинок
 @using (Html.BeginForm("Ad_img_add_ad", "Home", FormMethod.Post, new { enctype = "multipart/form-data" }))
    {
        @Html.Hidden("obg", JsonConvert.SerializeObject(Model))
        <input type="file" name="uploadImage[0]" />
        <input type="file" name="uploadImage[1]" />
        <input type="file" name="uploadImage[2]" />
        <input type="file" name="uploadImage[3]" />
        <input type="file" name="uploadImage[4]" />
        <input type="file" name="uploadImage[5]" />
        <input type="file" name="uploadImage[6]" />
        <input type="file" name="uploadImage[7]" />
        <input type="file" name="uploadImage[8]" />
        <input type="file" name="uploadImage[9]" />



                        <input type="submit" value="Добавить" />
    } 
  
  в представление отображение картинки

    @Html.Raw(string.Concat("<img  class='Show_one_ad_featured_big_img'  id ='Show_one_ad_featured_small_img_id_main'", " src=\"data:image/jpeg;base64,"
                                , Convert.ToBase64String(Model.Images_byte[0]), "\" />"))


    метод в контроллер для картинки 
    [HttpPost]
        public ActionResult Ad_img_add_ad(HttpPostedFileBase[] uploadImage,string obg=null)
        {
        }
  
 * */
namespace Im.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
    
public ActionResult Index()
        {
            //если залогинен то на страницу
            //если не залогинен то на главную
            return View();
        }

        public ActionResult Personal_record(int id=1)
        {
            
            return View();
        }

        public ActionResult Group_record(int id = 1)
        {

            return View();
        }




        //-PARTIAL BLOCK------------------------------------------------------------------------------------------------------------------------------//

        [ChildActionOnly]
        public ActionResult Left_menu_personal(int id = 1)
        {
            //показывать меню именно того пользователя мб id убрать хз пока как
            return PartialView();
        }

        //END-PARTIAL BLOCK------------------------------------------------------------------------------------------------------------------------------//

    }
}