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
using Microsoft.AspNet.Identity;

//JsonConvert.SerializeObject(Model)
// res = JsonConvert.DeserializeObject<Person_info_short>(obg);


namespace Im.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db_users = new ApplicationDbContext();
        All_db_Context db_all = new All_db_Context();

        


        public ActionResult Index()
        {
            //var a=db_users.Users.First();
            
           // a.Menu_left= "Моя Cтраница,Personal_record,Новости,News,Сообщения,Mesages,Друзья,Friends,Фотографии,Albums,Музыка,Music,Видео,Video";
            //db_users.SaveChanges();
            //если залогинен то на страницу
            //если не залогинен то на главную
            return View();
        }

        public ActionResult Personal_record(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                 id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
            var res = Record(id, "Personal_record");
            return View(res);
        }

        public ActionResult Group_record(string id)
        {
            var res = db_all.Groups.First();


            return View(res);
        }

        //TODO 
        public ActionResult Record_photo_page(string id)
        {
            //TODO закидывать фото юзеру и проверять что бы его фото отдавать продумать мб вообще не нужно
            //смотреть че как и фото отображать
            //
            return View();
        }
        //TODO 
        public ActionResult Albums(string id)
        {
            //TODO 
            var res = db_users.Users.First(x1 => x1.Id == id);

            return View();
        }
        //TODO 
        public ActionResult News(string id)
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();

            return View();
        }
        //TODO 
        public ActionResult Mesages(string id)
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();

            return View();
        }
        //TODO 
        public ActionResult Friends(string id)
        {
            //TODO 
            var res = db_users.Users.First(x1 => x1.Id == id);

            return View();
        }
        //TODO 
        public ActionResult Music(string id)
        {
            //TODO 
            var res = db_users.Users.First(x1 => x1.Id == id);

            return View();
        }
        //TODO 
        public ActionResult Video(string id)
        {
            //TODO 
            var res = db_users.Users.First(x1 => x1.Id == id);

            return View();
        }
        //TODO 
        public ActionResult Edit_personal_record()
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();

            return View();
        }


        //-PARTIAL BLOCK------------------------------------------------------------------------------------------------------------------------------//

        [ChildActionOnly]
        public ActionResult Left_menu_personal()
        {
            
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pers = Record(check_id, "info_person");
            ViewBag.list_str = pers.Menu_left;
            ViewBag.id = check_id;
            return PartialView();
        }

       
        public ActionResult Get_info_person_ajax_1(bool open=false, string id = "",string obg=null)
        {
            
            Person_info_short res = null;
            if (obg != null)
                res = JsonConvert.DeserializeObject<Person_info_short>(obg);
            else
            {
                //по id доставать параметры
                var pers = Record(id, "info_person");
                res = new Person_info_short() { Age=pers.Age, Country = pers.Country, Town = pers.Town, Street = pers.Street, Description = pers.Description };
            }
            
            ViewBag.Open = open;
            ViewBag.Id = id;
 
            return PartialView(res);
        }
        //END-PARTIAL BLOCK------------------------------------------------------------------------------------------------------------------------------//

            [HttpPost]
        public ActionResult Add_new_memes(HttpPostedFileBase[] uploadImage,string Description,string id,string bool_access)
        {
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            Memes res = null;
            IPage_view res_page = null;
            switch (bool_access)
            {
                case "person"://добавление записи со страницы пользователя

                    if(id== check_id)
                    {
                        var pers=Record(id, "Personal_record");
                         res = new Memes(string.Concat(pers.Name, " ", pers.Surname), id)
                        {
                            Description = Description
                        };
                        List<byte[]>photo_byte= Get_photo_post(uploadImage);
                        if (photo_byte.Count == 1)
                        { 
                            res.Image = photo_byte[0];
                        }
                        if (photo_byte.Count > 1)
                        {
                            foreach(var i in photo_byte)
                            {
                                res.Images.Add(i);
                                Img tmp = new Img(i);
                                db_all.Images.Add(tmp);
                                db_all.SaveChanges();
                                res.Images_id += tmp.Id + ",";
                            }
                        }
                        db_all.Memes.Add(res);
                        db_all.SaveChanges();
                        pers.Wall.Add(res);
                        pers.Wall_id+= res.Id+",";
                        pers.Wall_count += 1;
                        db_users.SaveChanges();
                        res_page=(IPage_view)pers;
                    }
                    

                    break;


                case "group"://добавление записи со страницы группы +проверки

                    break;
            }
           
                return View("Personal_record", res_page);
        }



        public ApplicationUser Record(string id,string bool_fullness)
        {
            var res = db_users.Users.First(x1 => x1.Id == id);
            //TODO заполнять все поля правильно в зависимости от bool_fullness

            //Personal_record
            //info_person
            //



            return res;
        }
        

        public List<byte[]> Get_photo_post(HttpPostedFileBase[] uploadImage)
        {
            List<byte[]> res = new List<byte[]>();
            if (uploadImage != null)
            {
                
                    foreach (var i in uploadImage)
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
                            res.Add(imageData); 

                        }
                        catch
                        {

                        }


                    
                }

            }


            return res;
        }


        protected override void Dispose(bool disposing)
        {
            db_all.Dispose();
            base.Dispose(disposing);
        }
    }
}