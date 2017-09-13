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
//string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();

namespace Im.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        

        


        public ActionResult Index()
        {
            /*
            var a=db_users.Users.First();
            a.Images_count = 0;
            a.Images_id = "";
            a.Main_images_id = "";
            db_users.SaveChanges();
            var t = db_all.Images.RemoveRange(db_all.Images.ToList());
            db_all.SaveChanges();
            */
            // a.Menu_left= "Моя Cтраница,Personal_record,Новости,News,Сообщения,Mesages,Друзья,Friends,Фотографии,Albums,Музыка,Music,Видео,Video";
            //db_users.SaveChanges();
            //если залогинен то на страницу
            //если не залогинен то на главную
            //var res = Record(System.Web.HttpContext.Current.User.Identity.GetUserId(), "Personal_record");

            //db.Users.First().Menu_left = "Моя Cтраница,Personal_record,Новости,News,Сообщения,Mesages,Группы,Groups_personal,Друзья,Friends,Фотографии,Albums,Музыка,Music,Видео,Video";
            /*
            db.Groups.RemoveRange(db.Groups.ToList());
            db.Users.First().Groups_count = 0;
            db.Users.First().Groups_id = "" ;
            db.SaveChanges();
            */
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
        
            public ActionResult Groups_personal(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
            var res = Record(id,"Groups_all");


            return View(res.Groups);
        }
        public ActionResult Group_record(string id)
        {
            var res = db.Groups.First();


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
            var res = db.Users.First(x1 => x1.Id == id);

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
            var res = db.Users.First(x1 => x1.Id == id);

            return View();
        }
        //TODO 
        public ActionResult Music(string id)
        {
            //TODO 
            var res = db.Users.First(x1 => x1.Id == id);

            return View();
        }
        //TODO 
        public ActionResult Video(string id)
        {
            //TODO 
            var res = db.Users.First(x1 => x1.Id == id);

            return View();
        }
        
        //TODO 
        public ActionResult Edit_group_record()
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            
            return View();
        }
        //TODO 
        public ActionResult Edit_personal_record()
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.list_menu = new string[] {"Основное","Контакты","Интересы","Образование","Карьера","Военная служба","Жизненная позиция" };

            return View();
        }


        //-PARTIAL BLOCK------------------------------------------------------------------------------------------------------------------------------//

        

        [ChildActionOnly]
        public ActionResult Left_menu_personal()
        {
            
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pers = Record(check_id, "info_person");
            if(pers!=null)
            ViewBag.list_str = pers.db.Menu_left;
            ViewBag.id = check_id;
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult Wall_memes(string from,string id)
        {
            
            
            IPage_view pers= null;
            switch (from)
            {
                case "Personal_record":
                     pers = Record(id, "Wall");
                    return PartialView(((Personal_record)pers).Wall);
                    break;

                default ://"News"
                    string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    pers = Record(check_id, "News");
                    return PartialView(((Personal_record)pers).News);
                    break;
            }

          



            
        }

        //TODO 

        public ActionResult Add_new_group_ajax()
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();

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
                //res = new Person_info_short() { Age=pers.db.Age, Country = pers.db.Country, Town = pers.db.Town, Street = pers.db.Street, Description = pers.db.Description };
                res = new Person_info_short(pers);
            }
            
            ViewBag.Open = open;
            ViewBag.Id = id;
 
            return PartialView(res);
        }
        public ActionResult Edit_personal_record_info_load_ajax(string obg)
        {
            ViewBag.click = obg;
            if (string.IsNullOrEmpty(obg))
                ViewBag.click = "Основное";
            
            ViewBag.list_menu = new string[] { "Основное", "Контакты", "Интересы", "Образование", "Карьера", "Военная служба", "Жизненная позиция" };
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pers = Record(check_id, "Personal_record");

            return PartialView(pers.db);
        }

        //END-PARTIAL BLOCK------------------------------------------------------------------------------------------------------------------------------//
        
        public ActionResult Change_pers_rec(ApplicationUser a,string click)
        {
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //var pers = db_users.Users.First(x1=>x1.Id==check_id);
            //TODO проверять валидацией все
            var pers = Record(check_id, "Personal_record");



            switch (click)
    {
        case "Основное":
            {
                        pers.db.Name = a.Name;
                        pers.db.Surname = a.Surname;
                        pers.db.Birthday = a.Birthday;
                        pers.db.Country = a.Country;
                        pers.db.Town = a.Town;
                        pers.db.Street = a.Street;
                        pers.db.Status = a.Status;
                        pers.db.Description = a.Description;
                        db.SaveChanges();

                break;
            }
        case "Контакты":
            {
                /*@Html.EditorFor(x1 => x1.Family_id)
                     @Html.EditorFor(x1 => x1.Black_list_id)
                    @Html.EditorFor(x1 => x1.Groups_id)
                    @Html.EditorFor(x1 => x1.Menu_left)
                    */
                    
                break;
            }
        case "Интересы":
            {


                break;
            }
        case "Образование":
            {


                break;
            }
        case "Карьера":
            {


                break;
            }
        case "Военная служба":
            {


                break;
            }
        case "Жизненная позиция":
            {


                break;
            }


    }

            ViewBag.click = click;
            ViewBag.save_chenges = true;
            if (string.IsNullOrEmpty(click))
                ViewBag.click = "Основное";

            ViewBag.list_menu = new string[] { "Основное", "Контакты", "Интересы", "Образование", "Карьера", "Военная служба", "Жизненная позиция" };



            return PartialView("Edit_personal_record_info_load_ajax", pers.db);
        }

        //TODO 
        [HttpPost]
        public ActionResult Add_new_group(Group a)
        {
            //TODO 
            //TO-DO проверять есть ли такие названия и тд тд тд
            


            if (a != null && !string.IsNullOrEmpty(a.Name))
            {
                try
                {
                    db.Groups.First(x1 => x1.Name == a.Name);
                    ViewBag.er_name = "Имя группы занято";
                }
                catch
                {
                    //
                    string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    var pers = Record(check_id, "DB");
                    
                    a.Admins_id = check_id+",";
                    a.Followers_id = check_id + ",";
                    
                    db.Groups.Add(a);
                    db.SaveChanges();
                    pers.db.Groups_count += 1;
                    pers.db.Groups_id += a.Id + ",";
                    db.SaveChanges();

                    ViewBag.add_success = "Группа создана";
                    ViewBag.id=a.Id;
                }

            }
            
            
            

            return PartialView("Add_new_group_ajax");
        }
        [HttpPost]
        public ActionResult Add_new_image(HttpPostedFileBase[] uploadImage, string for_what,string from)
        {
            IPage_view res_page = null;
            var lst1 = Get_photo_post(uploadImage);
            var lst2 = Get_photo_post(lst1);
            if (from == "person")
            {
                string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                res_page  = Record(check_id, "Personal_record");
                db.Images.Add(lst2[0]);
                db.SaveChanges();
                //СЕЙЧАС УБРАТЬ
                foreach (var i in db.Images)
                {
                    var t = i;
                }

                ((Personal_record)res_page).db.Images_count += 1;
                ((Personal_record)res_page).db.Images_id += lst2[0].Id + ",";
                ((Personal_record)res_page).Images.Insert(0, lst1[0]);
                if (for_what == "main_img")
                {
                    ((Personal_record)res_page).db.Main_images_id += lst2[0].Id + ",";
                    ((Personal_record)res_page).Main_images.Insert(0, lst1[0]);
                }


                db.SaveChanges();
                //СЕЙЧАС УБРАТЬ
                //res_page = Record(check_id, "Personal_record");

            }
            if (from == "group")
            {

            }
            //СЕЙЧАС УБРАТЬ
            foreach (var i in db.Images)
            {
                var t = i;
            }
            return View("Personal_record", res_page);
        }
            [HttpPost]
        public ActionResult Add_new_memes(HttpPostedFileBase[] uploadImage,string Description_mem,string id,string bool_access)
        {
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            Memes res_db = null;
            var res = new Memes_record();
            IPage_view res_page = null;
            switch (bool_access)
            {
                case "person"://добавление записи со страницы пользователя

                    if(id== check_id)
                    {
                        var pers=Record(id, "Personal_record");
                        res_db = new Memes(string.Concat(pers.db.Name, " ", pers.db.Surname), id)
                        {
                            Description = Description_mem
                         };
                        List<byte[]>photo_byte= Get_photo_post(uploadImage);
                        if (photo_byte.Count == 1)
                        {
                            res_db.Image = photo_byte[0];
                        }
                        if (photo_byte.Count > 1)
                        {
                            foreach(var i in photo_byte)
                            {
                                res.Images.Add(i);
                                Img tmp = new Img(i);
                                db.Images.Add(tmp);
                                db.SaveChanges();
                                res_db.Images_id += tmp.Id + ",";
                            }
                        }
                        db.Memes.Add(res_db);
                        db.SaveChanges();
                        //pers.Wall.Add(new Memes_record(res_db));
                        pers.db.Wall_id+= res_db.Id+",";
                        pers.db.Wall_count += 1;
                        db.SaveChanges();
                        res_page=(IPage_view)pers;
                        res.db = res_db;
                    }
                    

                    break;


                case "group"://добавление записи со страницы группы +проверки

                    break;
            }
           
                return View("Personal_record", res_page);
        }

        public IPage_view Group(string id, string bool_fullness = "Group_record")
        {
            IPage_view res = null;
            var not_res= db.Groups.First(x1 => x1.Id.ToString() == id);
            if (bool_fullness == "Group_record")
            {
                //TODO надо доделать что бы поля тоже заполнялись как в рекорде
                res = new Group_record(not_res);
            }
                if (bool_fullness == "Group_short"){
                res = new Group_short(not_res);
            }

            return res;
        }

            public Personal_record Record(string id,string bool_fullness = "Personal_record")
        {

            //Personal_record
            //Info_person
            //bool_fullness=Wall,News-------только список мемов :Personal_record
            //News
            //Groups_all




            //TODO сравнивать id и если одинаковые то и меню слева отправлять иначе нет
            //TODO заполнять все списки и с мемами и тд
            Personal_record res = null;
            if (id != null)//убрать условие?
            {
                var res_ap = db.Users.First(x1 => x1.Id == id);
                res = new Personal_record(res_ap);
                //TODO заполнять все поля правильно в зависимости от bool_fullness
                
                try
                {
                    if (bool_fullness == "DB")
                    {
                        //просто с бд инфу отправлять выше сделано
                    }
                    //АВА
                    if (bool_fullness == "Personal_record")
                    {
                        try
                        {
                            var main_img = res_ap.Main_images_id.Split(',');
                            int id_tmp = Convert.ToInt32(main_img[main_img.Count() - 2]);

                            var a_b123_tmp = db.Images.First(x1 => x1.Id == id_tmp);
                            //СЕЙЧАС
                            foreach (var i in db.Images)
                            {
                                var t = i;
                            }

                            var a_b_tmp = db.Images.First(x1 => x1.Id == id_tmp).bytes;


                            res.Main_images.Add(a_b_tmp);
                        }
                        catch
                        {

                        }
                        try
                        {
                            //ФОТО, потом под 1 засунуть мб
                            var not_main_img = res_ap.Images_id.Split(',');

                            for (int b = 0, i = not_main_img.Count() - 2; i >= 0 && b < 5; --i, b++)
                            {
                                int id_tmp = Convert.ToInt32(not_main_img[i]);
                                res.Images.Add(db.Images.First(x1 => x1.Id == id_tmp).bytes);
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            //заполнение групп
                            List<string> gr = res.db.Groups_id.Split(',').ToList();
                            for (int b = 0, i = gr.Count() - 2; i >= 0 && b < 5; --i, b++)
                            {

                                res.Groups.Add((Group_short)Group(gr[i], "Group_short"));
                            }
                                

                        }
                        catch
                        {

                        }

                    }
                    
                    
                    //мемы стена
                    if (bool_fullness == "Wall")
                    {
                        try
                        {
                            var str_mem_id = res.db.Wall_id.Split(',');
                            for (int b = 0, i = str_mem_id.Count() - 2; i > 0 || b < 10; ++b, --i)
                            {
                                string id_mem = str_mem_id[i];
                                var mem_tmp = db.Memes.First(x1 => x1.Id.ToString() == id_mem);
                                res.Wall.Add(new Memes_record(mem_tmp));
                            }
                        }
                        catch
                        {

                        }
                        
                        
                    }
                    //мемы новости
                    if (bool_fullness == "News")
                    {

                        try
                        {
                            var str_mem_id = res.db.News_id.Split(',');
                            for (int b = 0, i = str_mem_id.Count() - 2; i > 0 || b < 10; ++b, ++i)
                            {
                                string id_mem = str_mem_id[i];
                                var mem_tmp = db.Memes.First(x1 => x1.Id.ToString() == id_mem);
                                res.News.Add(new Memes_record(mem_tmp));
                            }
                        }
                        catch
                        {

                        }
                        
                    }
                    if (bool_fullness == "Groups_all")
                    {
                        try
                        {
                            List<string> lst = res.db.Groups_id.Split(',').ToList();
                            for (var i = 0; i < lst.Count - 1; ++i)
                            {
                                res.Groups.Add(((Group_short)Group(lst[i], "Group_short")));
                            }
                        }
                        catch
                        {

                        }
                        

                    }
                    
                    }
                catch
                {

                }
                //res.db = res_ap;

                
            }




            return res;
        }
        public List<Img> Get_photo_post(IEnumerable<byte[]> Images)
        {
            List<Img> res = new List<Img>();
            foreach(var i in Images)
            {
                res.Add(new Img(i));
            }
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
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}