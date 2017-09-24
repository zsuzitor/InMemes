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
            bool loginned = false;
            string id = null;
            Personal_record res = null;
            //ViewBag.My_page = false;
            try
            {
                id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    throw new Exception();
                
                    ViewBag.My_page = true;
                loginned = true;
                res = (Personal_record)Record(id, "Personal_record");
            }
            catch
            {
                
                loginned = false;
            }
            if (loginned)
            {
                return View("Personal_record",res);
            }
            else
            {
                //TODO
                //на страницу регистрации.логина
                return View();
            }


            
        }

        public ActionResult Personal_record(string id)
        {
           // ViewBag.My_page = false;
            if (string.IsNullOrEmpty(id))
            {
                 id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
            var res = Record(id, "Personal_record");
            
            if (id== System.Web.HttpContext.Current.User.Identity.GetUserId())
            ViewBag.My_page = true;
            return View(res);
        }
        
            public ActionResult Groups_personal(string id, string from= "Personal_record")
        {
            if (string.IsNullOrEmpty(id))
            {
                id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
            IPage_view res = null;
            switch (from)
            {
                case "Personal_record":
                     res = Record(id, "Groups_all");
                    break;
                case "Group_record":
                    break;
            }
            //var res = Record(id,"Groups_all");


            return View(((Personal_record)res).Groups);
        }
        public ActionResult Group_record(string id)
        {
            var res = (Group_record)Group(id, "Group_record");
            var check_id= System.Web.HttpContext.Current.User.Identity.GetUserId();
            


            return View(res);
        }

        //TODO 
        public ActionResult Record_photo_page(string from, string id)
        {
            //TODO закидывать фото юзеру и проверять что бы его фото отдавать продумать мб вообще не нужно
            //смотреть че как и фото отображать
            if (string.IsNullOrEmpty(id))
            {
                id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
            IPage_view res = null;

            switch (from)
            {
                case "Personal_record":
                    res = Record(id, "Groups_all");
                    break;
                case "Group_record":
                    break;
            }
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
            var res = (Personal_record)Record(check_id, "News");

            return View(res);
        }
        //TODO 
        public ActionResult Messages(string id)
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pers = Record(check_id, "Messages");

            return View(((Personal_record)pers).Message);
        }
        //TODO 
        public ActionResult Messages_one_dialog(string id)
        {
            //TODO 
            //Message_person_block
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //var pers = Record(check_id, "Messages");

            //TODO проверять есть ли доступ к этой переписке
            var res = Message_person_block(id,bool_fullness: "Messages_one_dialog");


            return View(res.Messages);
        }
        //TODO 
        public ActionResult Friends(string from, string id,string what)
        {
            //TODO 
            //what= Friends, Admins ,Followers
            if (string.IsNullOrEmpty(id))
            {
                id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
            Personal_record res = null;

            
                    res = (Personal_record)Record(id, "Friends");
                    

            //var res = db.Users.First(x1 => x1.Id == id);

            return View(res.Friends);
        }
        //TODO 
        public ActionResult Followers_group(string from, string id, string what)
        {
            //TODO 
            //what= Friends, Admins ,Followers
            if (string.IsNullOrEmpty(id))
            {
                id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
            IPage_view res = null;

            switch (what)
            {
                case "Followers":

                    res = Group(id, "Followers");
                    return View(((Group_record)res).Followers);
                    //ViewBag.Who_display = "Followers";
                    break;
                case "Admins":
                    res = Group(id, "Admins");
                    return View(((Group_record)res).Admins);
                    //ViewBag.Who_display = "Admins";
                    break;
            }

            //var res = db.Users.First(x1 => x1.Id == id);

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
        public ActionResult Edit_group_record(string id)
        {
            //TODO 
            //
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var res = (Group_record)Group(id,"DB");
            foreach(var i in res.db.Admins_id.Split(','))
            {

                if (i == check_id)
                {
                    return View(res);
                }
            }
            res = (Group_record)Group(id, "Group_record");
            return View("Group_record", res);
        }
        //TODO 
        public ActionResult Edit_personal_record()
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.list_menu = new string[] {"Основное","Контакты","Интересы","Образование","Карьера","Военная служба","Жизненная позиция" };

            return View();
        }
        //TODO 
        public ActionResult Action_memes(string id,string action_m)
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var mem = Memes(id);
            switch (action_m)
            {
                case "like":
                    {
                        var mass_liked = mem.db.Liked_id.Split(',');
                        List<string> mass_liked_1 = new List<string>();
                        bool liked = false;
                        
                            foreach (var i in mass_liked)
                            {
                                if (!string.IsNullOrEmpty(i))
                                {
                                if (i == check_id)
                                    liked = true;
                                else
                                    mass_liked_1.Add(i);
                                }
                            }
                        if (!liked)
                        {
                            mem.db.Liked_id += check_id + ",";
                            ViewBag.like = true;

                            

                        }
                        else
                        {
                            //уже лайкнул
                            mem.db.Liked_id ="";
                            foreach(var i in mass_liked_1)
                            {
                                mem.db.Liked_id +=i +",";
                            }
                            ViewBag.like = false;
                        }
                        db.SaveChanges();

                    }
                    
                    break;
                case "repost":
                    {
                        var pers =(Personal_record) Record(check_id, "DB");
                        pers.db.Wall_id += mem.db.Id+",";
                        bool fl = false;
                        foreach(var i in mem.db.Repost_id.Split(','))
                        {
                            if (i == check_id)
                                fl = true;
                        }
                        if (!fl)
                            mem.db.Repost_id += check_id + ",";

                        db.SaveChanges();

                    }
                    break;
                case "":
                    break;
            }


            return PartialView("Memes_partial", mem);
        }


        //-PARTIAL BLOCK------------------------------------------------------------------------------------------------------------------------------//
        [ChildActionOnly]
        public ActionResult Memes_partial(string from,string id_mem)
        {
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var res = Memes(id_mem);
            return PartialView(res);
        }



        [ChildActionOnly]
        public ActionResult Left_menu_personal()
        {
            
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pers = (Personal_record)Record(check_id, "info_person");
            if(pers!=null)
            ViewBag.list_str = pers.db.Menu_left;
            ViewBag.id = check_id;
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult Wall_memes(string from,string id,int start=0)
        {
           var res = Wall_memes_function(from, id, start);
            return PartialView(res.Reverse());





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
                res = new Person_info_short((Personal_record)pers);
            }
            
            ViewBag.Open = open;
            ViewBag.Id = id;
 
            return PartialView(res);
        }

        //TODO
        public ActionResult Follow_ajax(string from,string id ,bool click=false)
        {
            //TODO добавить список для не подтвержденных друей со стороны отправляющего
            //Group
            //Personal_record
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.id = id;
            ViewBag.from = from;

            if (from== "Group")
            {
                var pers = Group(id, "DB");
                var pers_peop = Record(check_id, "DB");
                try
                {
                    var tmp = ((Group_record)pers).db.Followers_id.Split(',').First(x1 => x1 == check_id);
                    if (tmp == null)
                        throw new Exception();
                    if (click)
                    {
                        //нужно отписать от группы
                        
                        var mass_peop = ((Personal_record)pers_peop).db.Groups_id.Split(',');
                        var mass = ((Group_record)pers).db.Followers_id.Split(',');
                        
                        ((Group_record)pers).db.Followers_id = "";
                        for (int i=0;i< mass.Count() - 1; ++i)
                        {
                            if (mass[i] != check_id)
                                ((Group_record)pers).db.Followers_id+= mass[i]+",";
                        }
                        ((Personal_record)pers_peop).db.Groups_id = "";
                        for (int i = 0; i < mass_peop.Count() - 1; ++i)
                        {
                            if (mass_peop[i] != id)
                                ((Personal_record)pers_peop).db.Groups_id += mass_peop[i] + ",";
                        }
                        db.SaveChanges();
                        ViewBag.follow = false;
                        ViewBag.message = "Подписаться";
                    }
                    else
                    {
                        ViewBag.follow = true;
                        ViewBag.message = "Отписаться";
                    }
                    
                }
                catch
                {
                    
                    if (click)
                    {
                        //нужно подписать на группу
                        ((Group_record)pers).db.Followers_id += check_id + ",";
                        ((Personal_record)pers_peop).db.Groups_count += 1;
                        ((Personal_record)pers_peop).db.Groups_id +=id +",";
                        db.SaveChanges();
                        ViewBag.follow = true;
                        ViewBag.message = "Отписаться";
                    }
                    else
                    {
                        ViewBag.follow = false;
                        ViewBag.message = "Подписаться";
                    }
                        
                }
            }
            if (from == "Personal_record")
            {
                var pers = Record(id, "DB");
                var pers_peop = Record(check_id, "DB");


                try
                {
                    var tmp = ((Personal_record)pers).db.Followers_id.Split(',').First(x1 => x1 == check_id);
                    if (tmp == null)
                        throw new Exception();
                    if (click)
                    {
                        //нужно отписать от человека

                        var mass = ((Personal_record)pers).db.Followers_id.Split(',');
                        var mass_peop = ((Personal_record)pers_peop).db.Friends_id.Split(',');
                        ((Personal_record)pers).db.Followers_id = "";
                        for (int i = 0; i < mass.Count() - 1; ++i)
                        {
                            if (mass[i] != check_id)
                                ((Personal_record)pers).db.Followers_id += mass[i] + ",";
                        }

                        ((Personal_record)pers_peop).db.Friends_id = "";
                        for (int i = 0; i < mass_peop.Count() - 1; ++i)
                        {
                            if (mass_peop[i] != id)
                                ((Personal_record)pers_peop).db.Friends_id += mass_peop[i] + ",";
                        }
                        db.SaveChanges();

                        ViewBag.follow = false;
                        ViewBag.message = "Подписаться";
                    }
                    else
                    {
                        ViewBag.follow = true;
                        ViewBag.message = "Удалить из друзей";
                    }

                }
                catch
                {

                    if (click)
                    {
                        //нужно подписать на человека
                        ((Personal_record)pers).db.Followers_id += check_id + ",";
                        



                        ((Personal_record)pers_peop).db.Friends_count += 1;
                        ((Personal_record)pers_peop).db.Friends_id += id + ",";
                        db.SaveChanges();

                        ViewBag.follow = true;
                        ViewBag.message = "Удалить из друзей";
                    }
                    else
                    {
                        ViewBag.follow = false;
                        ViewBag.message = "Добавить в друзья";
                    }

                }


            }

                return PartialView();
        }
        public ActionResult Edit_personal_record_info_load_ajax(string obg)
        {
            ViewBag.click = obg;
            if (string.IsNullOrEmpty(obg))
                ViewBag.click = "Основное";
            
            ViewBag.list_menu = new string[] { "Основное", "Контакты", "Интересы", "Образование", "Карьера", "Военная служба", "Жизненная позиция" };
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pers = Record(check_id, "Personal_record");

            return PartialView(((Personal_record)pers).db);
        }

        //END-PARTIAL BLOCK------------------------------------------------------------------------------------------------------------------------------//
        
        public ActionResult Change_pers_rec(ApplicationUser a,string click)
        {
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //var pers = db_users.Users.First(x1=>x1.Id==check_id);
            //TODO проверять валидацией все
            var pers = (Personal_record)Record(check_id, "Personal_record");



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




        [HttpPost]
        public ActionResult Additionally_Download(string what_download, string from, string id, int start_for_form = 0)
        {
            if (start_for_form < 0)
                start_for_form = 0;
            switch (what_download)
            {
                case "Wall":
                    {
                        var res = Wall_memes_function(from, id, start_for_form);
                        return PartialView("Wall_memes", res);
                        //return PartialView("Wall_memes");
                    }
                    
                    break;

                    
            }
            return PartialView("Wall_memes");
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
                    var pers = (Personal_record)Record(check_id, "DB");
                    
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
            //ViewBag.My_page = false;
            if (from == "person")
            {
                string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                res_page  = Record(check_id, "Personal_record");
                ViewBag.My_page = true;
                db.Images.Add(lst2[0]);
                db.SaveChanges();
                

                ((Personal_record)res_page).db.Images_count += 1;
                ((Personal_record)res_page).db.Images_id += lst2[0].Id + ",";
                ((Personal_record)res_page).Images.Insert(0, lst1[0]);
                if (for_what == "main_img")
                {
                    ((Personal_record)res_page).db.Main_images_id += lst2[0].Id + ",";
                    ((Personal_record)res_page).Main_images.Insert(0, lst1[0]);
                }


                db.SaveChanges();
                

            }
            if (from == "group")
            {

            }
            
            return View("Personal_record", res_page);
        }
            [HttpPost]
        public ActionResult Add_new_memes(HttpPostedFileBase[] uploadImage,string Description_mem,string id,string bool_access)
        {
            //TODO проверять можно ли добавить пост туда куда требуют
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            Memes res_db = null;
            //var res = new Memes_record();
            IPage_view res_page = null;
           // ViewBag.My_page = false;
            switch (bool_access)
            {
                case "person"://добавление записи со страницы пользователя
                    //TODO сейчас добавляется только на свою страницу
                    
                    {
                        if (id == check_id)
                        {
                            ViewBag.My_page = true;
                            var pers = (Personal_record)Record(id, "Personal_record");
                            res_db = new Memes(string.Concat(pers.db.Name, " ", pers.db.Surname), id)
                            {
                                Description = Description_mem
                            };
                            List<byte[]> photo_byte = Get_photo_post(uploadImage);
                            if (photo_byte.Count == 1)
                            {
                                res_db.Image = photo_byte[0];
                            }
                            if (photo_byte.Count > 1)
                            {
                                foreach (var i in photo_byte)
                                {
                                    //res.Images.Add(i);
                                    Img tmp = new Img(i);
                                    db.Images.Add(tmp);
                                    db.SaveChanges();
                                    res_db.Images_id += tmp.Id + ",";
                                }
                            }
                            db.Memes.Add(res_db);
                            db.SaveChanges();
                            //???
                            //res.db = res_db;
                            //pers.Wall.Add(res);

                            pers.db.Wall_id += res_db.Id + ",";
                            pers.db.Wall_count += 1;

                            db.SaveChanges();
                            //раскидать друзьям подписчикам
                            {
                                var mass_fr = pers.db.Friends_id.Split(',');
                                foreach(var i in mass_fr)
                                {
                                    if (!string.IsNullOrEmpty(i))
                                    {
                                        var rec = Record(i, "DB");
                                        ((Personal_record)rec).db.News_id += res_db.Id + ",";
                                    }
                                    

                                }
                                var mass_follow = pers.db.Followers_ignore_id.Split(',');
                                foreach (var i in mass_fr)
                                {
                                    if (!string.IsNullOrEmpty(i))
                                    {
                                        var rec = Record(i, "DB");
                                        ((Personal_record)rec).db.News_id += res_db.Id + ",";
                                    }
                                }
                                db.SaveChanges();
                            }

                            res_page = (IPage_view)pers;
                            //res.db = res_db;
                        }
                    }

                    break;


                case "group"://добавление записи со страницы группы +проверки
                    //TODO проверять можно ли добавить пост туда куда требуют и в списке админов то  ViewBag.My_page = true;
                    {
                        var pers = (Group_record)Group(id, "DB");
                        res_db = new Memes(pers.db.Name, id)
                        {
                            Description = Description_mem
                        };
                        List<byte[]> photo_byte = Get_photo_post(uploadImage);
                        if (photo_byte.Count == 1)
                        {
                            res_db.Image = photo_byte[0];
                        }
                        if (photo_byte.Count > 1)
                        {
                            foreach (var i in photo_byte)
                            {
                                //res.Images.Add(i);
                                Img tmp = new Img(i);
                                db.Images.Add(tmp);
                                db.SaveChanges();
                                res_db.Images_id += tmp.Id + ",";
                            }
                        }
                        db.Memes.Add(res_db);
                        db.SaveChanges();
                        //???
                        //res.db = res_db;
                        //pers.Wall.Add(res);


                        pers.db.Wall_id += res_db.Id + ",";
                        db.SaveChanges();

                        //раскидать друзьям подписчикам
                        {
                            var mass_follow = pers.db.Followers_id.Split(',');
                            foreach (var i in mass_follow)
                            {
                                if (!string.IsNullOrEmpty(i))
                                {
                                    var rec = Record(i, "DB");
                                    ((Personal_record)rec).db.News_id += res_db.Id + ",";
                                }
                            }
                            
                            db.SaveChanges();
                        }

                        res_page = (IPage_view)pers;
                        //res.db = res_db;


                        return View("Group_record", res_page);
                    }
                    break;
            }
           
                return View("Personal_record", res_page);
        }
        
public Message_obg_record Message_person_block(string id,int start_obg=0, int start_mes = 0, string bool_fullness = "Messages")
        {
            //Messages
            //Messages_one_dialog
            var res =new Message_obg_record( db.Messages_obg.First(x1 => x1.Id.ToString() == id));



            try
            {
                string[] str = res.db.Person_id.Split(',');



                for (int b = 0, i = str.Count() - 2 - start_obg; i >= 0 && b < 10; --i, ++b)
                {
                    res.Person.Add((Person_short)Record(str[i], "Person_short"));

                }


            }
            catch
            {

            }
            if (bool_fullness== "Messages")
            {

                
                try
                {
                    //db.Messages.First(x1 => x1.Id.ToString() == id)
                    string[] str = res.db.Messages_id.Split(',');

                        var a = db.Messages.First(x1 => x1.Id.ToString() == str[str.Count() - 2]);
                        res.Messages.Add(a);
                    

                }
                catch
                {

                }
            }
            if (bool_fullness == "Messages_one_dialog")
            {

                try
                {
                    //db.Messages.First(x1 => x1.Id.ToString() == id)
                    string[] str = res.db.Messages_id.Split(',');


                    for (int b = 0, i = str.Count() - 2 - start_mes; i >= 0 && b < 30; --i, ++b)
                    {
                        var a = db.Messages.First(x1 => x1.Id.ToString() == str[i]);
                        res.Messages.Add(a);
                    }

                }
                catch
                {

                }
            }
                

            return res;

        }
        /*public Message Message_person_one(string id)
        {

            var res = db.Messages.First(x1 => x1.Id.ToString() == id);
        }
        */
            public Memes_record Memes(string id)
        {
            var not_res= db.Memes.First(x1 => x1.Id.ToString() == id);
            ViewBag.like = false;
            ViewBag.repost = false;
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            Memes_record res=new Memes_record(not_res);
            try
            {
                for (int i = 0; i < res.db.Images_id.Count() - 2; ++i)
                {
                    res.Images.Add((db.Images.First(x1 => x1.Id == res.db.Images_id[0])).bytes);
                }
            }
            catch
            {

            }

            
            //проверка на лайк
            foreach (var i in not_res.Liked_id.Split(','))
            {
                if (!string.IsNullOrEmpty(i))
                {
                    if (i == check_id)
                        ViewBag.like = true;
                    
                }
            }
            //проверка на репост
            foreach (var i in not_res.Repost_id.Split(','))
            {
                if (!string.IsNullOrEmpty(i))
                {
                    if (i == check_id)
                        ViewBag.repost = true;

                }
            }


            

            return res;
        }
            public IPage_view Group(string id, string bool_fullness = "Group_record",int start=0)
        {
            //TODO брать инфу только ту которая нужна остальное убирать например Person_short

            //DB
            //Group_record
            //Wall
            //Group_short
            //Followers  Admins

            IPage_view res = null;
            var not_res= db.Groups.First(x1 => x1.Id.ToString() == id);


            if (bool_fullness == "Group_record")
            {
                //TODO надо доделать что бы поля тоже заполнялись как в рекорде
                res = new Group_record(not_res);
                string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                try
                {
                    var str = ((Group_record)res).db.Followers_id.Split(',');
                    for (int b = 0, i = str.Count() - 2; i >= 0 && b < 5; --i, b++)
                    {
                        //-СЕЙЧАС
                        var a = ((Person_short)Record(str[i], "Person_short"));

                        //((Group_record)res).Followers.Add(((Person_short)Record(str[i], "Person_short")));
                        ((Group_record)res).Followers.Add(a);
                    }
                        
                    
                }
                catch
                {

                }
                try
                {
                    var str = ((Group_record)res).db.Admins_id.Split(',');
                    ViewBag.My_page = false ;
                    foreach (var i in str)
                    {

                        if (i == check_id)
                        {
                            ViewBag.My_page = true;
                        }
                    }
                    for (int b = 0, i = str.Count() - 2; i >= 0 && b < 5; --i, b++)
                    {
                        //-СЕЙЧАС
                        var a = ((Person_short)Record(str[i], "Person_short"));

                        //((Group_record)res).Followers.Add(((Person_short)Record(str[i], "Person_short")));
                        ((Group_record)res).Admins.Add(a);
                    }



                    


                }
                catch
                {

                }

                try
                {
                    var str = ((Group_record)res).db.Images_id.Split(',');
                    for (int b = 0, i = str.Count() - 2; i >= 0 && b < 5; --i, b++)
                    {
                        ((Group_record)res).Images.Add(db.Images.First(x1=>x1.Id.ToString()== str[i]).bytes);
                    }


                }
                catch
                {

                }
                try
                {
                    var str = ((Group_record)res).db.Main_images_id.Split(',');
                    
                        ((Group_record)res).Main_images.Add(db.Images.First(x1 => x1.Id.ToString() == str[str.Count() - 2]).bytes);
                    

                }
                catch
                {

                }
                try
                {
                    
                        var str = ((Group_record)res).db.Groups_id.Split(',');
                    for (int b = 0, i = str.Count() - 2; i >= 0 && b < 5; --i, b++)
                    {
                        ((Group_record)res).Groups.Add((Group_short)Group(str[i], "Group_short"));
                    }
                }
                catch
                {

                }

            }
            if (bool_fullness == "Followers")
            {
                res = new Group_record(not_res);
                var lst = not_res.Followers_id.Split(',');
                foreach(var i in lst)
                {
                    if (string.IsNullOrEmpty(i))
                    {
                        ((Group_record)res).Followers.Add((Person_short)Record(i, "Person_short"));
                    }
                    
                }
            }
            if (bool_fullness == "Admins")
            {
                res = new Group_record(not_res);
                var lst = not_res.Admins_id.Split(',');
                foreach (var i in lst)
                {
                    if (string.IsNullOrEmpty(i))
                    {
                        ((Group_record)res).Admins.Add((Person_short)Record(i, "Person_short"));
                    }

                }

            }
            if (bool_fullness == "Wall")
            {
                res = new Group_record(not_res);
                /*
                try
                {
                    res = new Group_record(not_res);
                    var str = ((Group_record)res).db.Wall_id.Split(',');
                    for (int b = 0, i = str.Count() - 2 - start; i >= 0 && b < 10; --i, b++)
                    {
                        ((Group_record)res).Wall.Add(Memes(str[i]));
                    }


                }
                catch
                {

                }
                */

            }
                if (bool_fullness == "Group_short"){
                res = new Group_short(not_res);
            }
            if (bool_fullness == "DB")
            {
                res = new Group_record(not_res);
            }

            return res;
        }

            public IPage_view Record(string id,string bool_fullness = "Personal_record",int start=0)
        {
            //TODO брать инфу только ту которая нужна остальное убирать например Person_short
            //Personal_record
            //Info_person
            //bool_fullness=Wall,News-------только список мемов :Personal_record
            //News
            //Groups_all
            //Person_short ????
            //Friends Followers
            //Messages
            //Messages_one_dialog




            //TODO сравнивать id и если одинаковые то и меню слева отправлять иначе нет
            //TODO заполнять все списки и с мемами и тд
            Personal_record res = null;
            if (id != null)//убрать условие? проверять есть ли такой человек в базе
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
                    if (bool_fullness == "Person_short")
                    {
                        byte[] a_b_tmp = null;
                        try
                        {
                            var img = res.db.Main_images_id.Split(',');
                            int id_tmp = Convert.ToInt32(img[img.Count() - 2]);

                            var a_b123_tmp = db.Images.First(x1 => x1.Id == id_tmp);


                             a_b_tmp = db.Images.First(x1 => x1.Id == id_tmp).bytes;


                            
                        }
                        catch
                        {

                        }
                            return new Person_short(res.db.Id, a_b_tmp, res.db.Name);
                    }
                        //АВА
                        if (bool_fullness == "Personal_record")
                    {
                        try
                        {
                            var main_img = res_ap.Main_images_id.Split(',');
                            int id_tmp = Convert.ToInt32(main_img[main_img.Count() - 2]);

                            var a_b123_tmp = db.Images.First(x1 => x1.Id == id_tmp);
                           

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
                        try
                        {
                            //заполнение подписчиков думаю не нужно
                            /*
                            var lst = res.db.Followers_id.Split(',');

                            for (int b = 0, i = lst.Count() - 2; i >= 0 && b < 5; --i, b++)
                            {

                                res.Followers.Add((Person_short)Record(lst[i], "Person_short"));
                            }
                            var lst2 = res.db.Followers_id.Split(',');

                            for (int b = 0, i = lst2.Count() - 2; i >= 0 && b < 5; --i, b++)
                            {

                                res.Followers.Add((Person_short)Record(lst2[i], "Person_short"));
                            }
                            */

                        }
                        catch
                        {

                        }
                        try
                        {
                            //заполнение друзей
                            var lst = res.db.Friends_id.Split(',');
                            
                            for (int b = 0, i = lst.Count() - 2; i >= 0 && b < 5; --i, b++)
                            {

                                res.Friends.Add((Person_short)Record(lst[i], "Person_short"));
                            }


                        }
                        catch
                        {

                        }


                    }
                    
                    
                    //мемы стена
                    if (bool_fullness == "Wall")
                    {
                        /*
                        try
                        {
                            var str_mem_id = res.db.Wall_id.Split(',');
                            for (int b = 0, i = str_mem_id.Count() - 2-start; i > 0 && b < 10; ++b, --i)
                            {
                                string id_mem = str_mem_id[i];
                                var mem_tmp = db.Memes.First(x1 => x1.Id.ToString() == id_mem);
                                res.Wall.Add(new Memes_record(mem_tmp));
                            }
                        }
                        catch
                        {

                        }
                        */
                        
                    }
                    //мемы новости
                    if (bool_fullness == "News")
                    {
                        /*
                        try
                        {
                            var str_mem_id = res.db.News_id.Split(',');
                            for (int b = 0, i = str_mem_id.Count() - 2-start; i > 0 && b < 10; ++b, ++i)
                            {
                                string id_mem = str_mem_id[i];
                                var mem_tmp = db.Memes.First(x1 => x1.Id.ToString() == id_mem);
                                res.News.Add(new Memes_record(mem_tmp));
                            }
                        }
                        catch
                        {

                        }
                        */
                    }
                    if (bool_fullness == "Friends")
                    {
                        var lst = res.db.Friends_id.Split(',');
                        foreach(var i in lst)
                        {
                            if(string.IsNullOrEmpty(i))
                            res.Friends.Add((Person_short)Record(i, "Person_short"));
                        }

                    }
                    if (bool_fullness == "Followers")//мб разделить
                    {
                        var lst = res.db.Followers_ignore_id.Split(',');
                        foreach (var i in lst)
                        {
                            if (string.IsNullOrEmpty(i))
                                res.Followers_ignore.Add((Person_short)Record(i, "Person_short"));
                        }
                        var lst2 = res.db.Followers_id.Split(',');
                        foreach (var i in lst)
                        {
                            if (string.IsNullOrEmpty(i))
                                res.Followers.Add((Person_short)Record(i, "Person_short"));
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
                    if (bool_fullness == "Messages")
                    {
                        try
                        {
                            List<string> lst = res.db.Message_id.Split(',').ToList();
                            for (var i = 0; i < lst.Count - 1; ++i)
                            {

                                res.Message.Add(Message_person_block(lst[i]));
                            }
                        }
                        catch
                        {

                        }


                    }
                    
                       /* if (bool_fullness == "Messages_one_dialog")
                    {
                        try
                        {
                            List<string> lst = res.db.Message_id.Split(',').ToList();
                            for (var i = 0; i < lst.Count - 1; ++i)
                            {

                                res.Message.Add(Message_person_block(lst[i]));
                            }
                        }
                        catch
                        {

                        }


                    }*/



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


        IEnumerable<string> Wall_memes_function(string from, string id, int start = 0)
        {
            if (string.IsNullOrEmpty(id))
                id = System.Web.HttpContext.Current.User.Identity.GetUserId();

            IPage_view pers = null;
            IEnumerable<string> res = null;
            ViewBag.start = start;
            switch (from)
            {
                case "Personal_record":
                    pers = Record(id, "Wall", start);
                    res = ((Personal_record)pers).db.Wall_id.Split(',');
                    if (start == 0)
                        if (res.Count() == 0)
                            ViewBag.Count = 0;


                    return res;
                    break;

                case "Group_record":
                    pers = Group(id, "Wall", start);
                    res = ((Group_record)pers).db.Wall_id.Split(','); 
                    return res;
                    break;

                default://"News"
                    string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    pers = Record(check_id, "News", start);
                    res = ((Personal_record)pers).db.Wall_id.Split(',');
                    if (start == 0)
                        if (res.Count() == 0)
                            ViewBag.Count = 0;

                    return res;
                    break;
            }
        }


        }
}


