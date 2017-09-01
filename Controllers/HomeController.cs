﻿using System;
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

    if ( uploadImage != null)//ModelState.IsValid &&
            {
                foreach(var i in uploadImage)
                {
                    try
                    {
                        byte[] imageData = null;
                        // считываем переданный файл в массив байтов
                        using (var binaryReader = new BinaryReader(i.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(i.ContentLength);
                        }
                        // установка массива байтов
                        res.Images_byte.Add(imageData);



                        //return RedirectToAction("Add_new_ad", res);

                    }
                    catch
                    {

                    }


                }



        }
  
 * */
namespace Im.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db_users = new ApplicationDbContext();
        All_db_Context db_all = new All_db_Context();

        /*db.Users.Add(user2);
                db.SaveChanges();*/


        public ActionResult Index()
        {
            //db_all.Groups.Add(new Group("тестова група 1"));
            //db_all.SaveChanges();
            
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


        public ActionResult Record_photo_page(string id=null)
        {
            //TODO закидывать фото юзеру и проверять что бы его фото отдавать продумать мб вообще не нужно
            //смотреть че как и фото отображать
            //
            return View();
        }



        //-PARTIAL BLOCK------------------------------------------------------------------------------------------------------------------------------//

        [ChildActionOnly]
        public ActionResult Left_menu_personal(int id = 1)
        {
            //показывать меню именно того пользователя мб id убрать хз пока как
            return PartialView();
        }

       
        public ActionResult Get_info_person_ajax_1(bool open=false, string id = "")
        {
            //TODO раскоментить,доделать и заменить то что ниже до //
            /*
             ApplicationUser res = Record(id);
            ViewBag.Open = open;
            //параметры учетки в viewbag
            ViewBag.Age = res.Age;
            ViewBag.Country = res.Country;
            ViewBag.Town = res.Town;
            ViewBag.Street = res.Street;
            ViewBag.Description = res.Description;
            */
            
            ViewBag.Open = open;
            //параметры учетки в viewbag
            ViewBag.Age = 20;
            ViewBag.Country = "Россия";
            ViewBag.Town = "Москва";
            ViewBag.Street = "Ленина";
            ViewBag.Description = "описание 0000000                        12          аааааааааааааааааа ыыыыыыыыыыыы       ссссссссссссссс";


            

            return PartialView();
        }
        //END-PARTIAL BLOCK------------------------------------------------------------------------------------------------------------------------------//





        public ApplicationUser Record(string id)
        {



            return new ApplicationUser();
        }


        protected override void Dispose(bool disposing)
        {
            db_all.Dispose();
            base.Dispose(disposing);
        }
    }
}