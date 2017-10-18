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
            try
            {
                db.Wall_memes_connected.RemoveRange(db.Wall_memes_connected.Where(x1 => x1.Something_one_id == "3"));
                db.SaveChanges();
            }
            catch { }*/
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
            //db.Wall_memes_connected.RemoveRange(db.Wall_memes_connected.ToList());
            //db.Memes.RemoveRange(db.Memes.ToList());
            //db.SaveChanges();
            bool loginned = false;
            string id = null;
            Personal_record res = null;
            //ViewBag.My_page = false;
            try
            {
                id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    throw new Exception();
                
                   // ViewBag.My_page = true;
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
        public ActionResult Search_main(string text)
        {
            //TODO сейчас поиск только по людям
            List<Person_short> res = new List<Person_short>();
            try
            {
                var pers = db.Users.First(x1 => (x1.Name.IndexOf(text) != -1) || (x1.Surname.IndexOf(text) != -1));
                res.Add((Person_short)Record(pers.Id, "Person_short"));
            }
            catch { }
            ViewBag.person_list = res;



            return View();
        }
        public ActionResult Personal_record(string id)
        {
           // ViewBag.My_page = false;
            if (string.IsNullOrEmpty(id))
            {
                 id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
            var res = Record(id, "Personal_record");
            
            //if (id== System.Web.HttpContext.Current.User.Identity.GetUserId())
            //ViewBag.My_page = true;
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
        public ActionResult Record_photo_page( string id_image, string album_name)//string from,,string album_name
        {
            //что то типо параметров поиска хз пока что
            //TODO закидывать фото юзеру и проверять что бы его фото отдавать продумать мб вообще не нужно
            //смотреть че как и фото отображать
            //TODO надо соседние фото определить что бы листать можно было
            //TODO список альбомов польователя??? пока что так


            //TODO Record_photo_page  проверить сейчас альбомы null или устые возвращает + добавить ViewBag.Preview_img_id и  ViewBag.Next_img_id
            //ViewBag.Preview_img_id и  ViewBag.Next_img_id
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var list_album = new List<Img>();


            Img res = null;
            if (!string.IsNullOrEmpty(id_image))
            {
                int int_id_image = Convert.ToInt32(id_image);
                res = db.Images.First(x1 => x1.Id == int_id_image);
                var list_like = db.Liked_connected.Where(x1 => x1.Something_one_id == id_image);
                ViewBag.Count_like = list_like.Count();
                ViewBag.Like_list = list_like.Take(5);
                //+репост




                {


                    //TODO альбомы исправить на что то лучше иб при отдельном запросе мб хранить просто в списке
                    var list_photo_all_id = db.Images_connected.Where(x1 => x1.Something_two_id == check_id).ToList();
                var albums = new List<string>();
                // var res = new List<Img>();

                foreach (var i in list_photo_all_id)
                {
                    int int_id = Convert.ToInt32(i.Something_one_id);
                    var image = db.Images.First(x1 => x1.Id == int_id);
                        if ( (string.IsNullOrEmpty(album_name) ?true: image.Albums == res.Albums))//мб сравнивать с album_name
                            list_album.Add(image);
                    //res.Add(image);
                    try
                    {
                        albums.First(x1 => x1 == image.Albums);
                    }
                    catch
                    {
                        albums.Add(image.Albums);
                    }

                }
                ViewBag.Albums = albums;
                }
 for(var i=0;i< list_album.Count; ++i)
                {

                    if (list_album[i].Id == res.Id)
                    {
                        if (i > 0)
                            ViewBag.Preview_img_id = list_album[i - 1].Id;
                        else
                            ViewBag.Preview_img_id = list_album[list_album.Count-1].Id;
                        if(i< list_album.Count-1)
                            ViewBag.Next_img_id= list_album[i + 1].Id;
                        else
                            ViewBag.Next_img_id = list_album[0].Id;
                    }
                }
               
            }
            else
            {

            }


            //
            return PartialView(res);
        }
        //TODO
        public ActionResult Album_photo(string id_user, string album_name = "")
        {
            id_user = string.IsNullOrEmpty(id_user) ?  System.Web.HttpContext.Current.User.Identity.GetUserId() : id_user;
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var list_photo_all_id = db.Images_connected.Where(x1 => x1.Something_two_id == check_id).ToList();
            var res = new List<Img>();
            ViewBag.Album_name = string.IsNullOrEmpty( album_name)? "Все Альбомы": album_name;
            foreach (var i in list_photo_all_id)
            {
                
                int int_id = Convert.ToInt32(i.Something_one_id);
                try
                {
                    var image = db.Images.First(x1 => x1.Id == int_id&&( string.IsNullOrEmpty(album_name)?true:x1.Albums==album_name));
                    //if (image.Albums == album_name)
                        res.Add(image);
                }
                catch { }
                
                

            }


            return PartialView(res);
        }



        //TODO 
        public ActionResult Albums(string id)
        {
            //TODO 
            id=string.IsNullOrEmpty(id) ?  System.Web.HttpContext.Current.User.Identity.GetUserId() : id;
            //string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.id_user = id;
            var list_photo_all_id = db.Images_connected.Where(x1 => x1.Something_two_id == id).ToList();
            var albums= new List<Img>();
           // var res = new List<Img>();

            foreach(var i  in list_photo_all_id)
            {
                int int_id = Convert.ToInt32(i.Something_one_id);
                var image = db.Images.First(x1 => x1.Id == int_id);
                //res.Add(image);
                try
                {
                    albums.First(x1 => x1.Albums == image.Albums);
                }
                catch
                {
                    albums.Add(image);
                }
                
            }
            //res.Reverse();
            //ViewBag.Albums = albums;

            return View(albums);
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
        public ActionResult Messages_one_dialog(string id,string person_id)
        {
            //TODO 
            //Message_person_block
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.My_id = check_id;
            //var pers = Record(check_id, "Messages");

            //TODO проверять есть ли доступ к этой переписке
            var res = Message_person_block(id:id, person_id: person_id, bool_fullness: "Messages_one_dialog");

            ViewBag.Id_dialog = res.db.Id.ToString();
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
            switch (what)
            {
                case "Followers_ignore":
                    res = (Personal_record)Record(id, "Followers_ignore");
                    return View(res.Followers_ignore);
                    break;
                case "Followers":
                    res = (Personal_record)Record(id, "Followers");
                    return View(res.Followers);
                    break;
                case "Send_follow":
                    res = (Personal_record)Record(id, "Send_follow");
                    return View(res.Followers);
                    break;
                default :
                    res = (Personal_record)Record(id, "Friends");
                    return View(res.Friends);
                    break;
            }
                
                    

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
        public ActionResult Delete(string what,string from_id,string from,string id)
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();

            switch (what)
            {
                case "photo":
                    //удаление фото
                    //TODO удалять мемы которые== этому фото(мб все мемы с этим фото\оставлять текст умемов этих) хз
                    if (from == "Personal_record")
                    {
                        try
                        {
                            int int_id = Convert.ToInt32(id);
                            var list = db.Images_connected.Where(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id);
                            db.Images_connected.RemoveRange(list);
                            db.Images.RemoveRange(db.Images.Where(x1 => x1.Id == int_id));
                            try
                            {
                                db.Liked_connected.RemoveRange(db.Liked_connected.Where(x1 => x1.Something_one_id == id));
                            }
                            catch { }
                            try
                            {
                                db.Repost_connected.RemoveRange(db.Repost_connected.Where(x1 => x1.Something_one_id == id));
                            }
                            catch { }


                            db.SaveChanges();
                        }
                        catch
                        {
                            return View("Personal_record", Record(check_id, "Personal_record"));
                        }
                    }
                    else if (from == "Group_record")
                    {
                        try
                        {
                            var pers = db.Friends_connected.First(x1 => x1.Something_one_id == from_id && x1.Something_two_id == check_id && x1.Admin_group);


                            int int_id = Convert.ToInt32(id);
                            var list = db.Images_connected.Where(x1 => x1.Something_one_id == id);
                            db.Images_connected.RemoveRange(list);
                            db.Images.RemoveRange(db.Images.Where(x1 => x1.Id == int_id));
                            try
                            {
                                db.Liked_connected.RemoveRange(db.Liked_connected.Where(x1 => x1.Something_one_id == id));
                            }
                            catch { }
                            try
                            {
                                db.Repost_connected.RemoveRange(db.Repost_connected.Where(x1 => x1.Something_one_id == id));
                            }
                            catch { }


                            db.SaveChanges();
                        }
                        catch
                        {
                            return View("Personal_record", Record(check_id, "Personal_record"));
                        }


                    }
                    else if (from == "News")
                    { }
                        
                    
                        



                    

                    break;

                case "memes":
                    //удаление мема
                    
                    if (from == "Personal_record")
                    {
                        try
                        {
                            var list = db.Wall_memes_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id);
                            db.Wall_memes_connected.Remove(list);
                            
                            
                            try
                            {
                                int int_id = Convert.ToInt32(id);
                                var mem = db.Memes.First(x1 =>x1.Id== int_id && x1.Source_id == check_id);
                                //мем человека который удаляет и мем нужно удалить
                                try
                                {
                                    db.Repost_connected.Remove(db.Repost_connected.First(x1 => x1.Something_one_id == id));// && x1.Something_two_id == check_id

                                }
                                catch { }
                                try
                                {
                                  db.Liked_connected.RemoveRange(db.Liked_connected.Where(x1 => x1.Something_one_id == id) );//&& x1.Something_two_id == check_id)
                                }
                                catch { }
                                try
                                {
                                    var img_list = db.Images_connected.Where(x1 => x1.Something_one_id == id);
                                    db.Images_connected.RemoveRange(img_list);
                                    foreach (var i in img_list)
                                    {
                                        int int_id_i = Convert.ToInt32(i.Something_two_id);
                                        db.Images.RemoveRange(db.Images.Where(x1 => x1.Id == int_id_i));
                                    }

                                }
                                catch { }
                                try
                                {
                                    //сам мем
                                    db.Memes.Remove(db.Memes.First(x1=>x1.Id== int_id && x1.Source_id==check_id));
                                }
                                
                                catch { }
                                try
                                {
                                    db.Wall_memes_connected.RemoveRange(db.Wall_memes_connected.Where(x1=>x1.Something_one_id==id));
                                }
                                catch { }
                            }
                            catch {
                                //мем НЕ человека который удаляет и мем НЕ нужно удалять

                                try
                                {
                                    db.Repost_connected.Remove(db.Repost_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id));

                                }
                                catch { }
                                try
                                {
                                    db.Wall_memes_connected.RemoveRange(db.Wall_memes_connected.Where(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id));
                                }
                                catch { }
                            }


                            db.SaveChanges();



                        }
                        catch
                        {
                            return View("Personal_record", Record(check_id, "Personal_record"));
                        }
                    }
                    else if (from == "Group_record")
                    {

                        try
                        {
                            var pers = db.Friends_connected.First(x1 => x1.Something_one_id == from_id && x1.Something_two_id == check_id && x1.Admin_group);


                            
                            try
                            {
                                int int_id = Convert.ToInt32(id);
                                var mem = db.Memes.First(x1 => x1.Id == int_id && x1.Source_id == from_id);
                                //мем человека(группы) который удаляет и мем нужно удалить

                                try
                                {
                                    db.Repost_connected.RemoveRange(db.Repost_connected.Where(x1 => x1.Something_one_id == id));
                                }
                                catch { }
                                try
                                {
                                    db.Liked_connected.RemoveRange(db.Liked_connected.Where(x1 => x1.Something_one_id == id));
                                }
                                catch { }

                                try
                                {
                                    var img_list = db.Images_connected.Where(x1 => x1.Something_one_id == id);
                                    db.Images_connected.RemoveRange(img_list);
                                    foreach (var i in img_list)
                                    {
                                        int int_id_i = Convert.ToInt32(i.Something_two_id);
                                        db.Images.RemoveRange(db.Images.Where(x1 => x1.Id == int_id_i));
                                    }

                                }
                                catch { }
                                try
                                {
                                    db.Wall_memes_connected.RemoveRange(db.Wall_memes_connected.Where(x1 => x1.Something_one_id == id));
                                }
                                catch { }
                                try
                                {
                                    //сам мем
                                    //db.Memes.Remove(db.Memes.First(x1 => x1.Id == int_id && x1.Source_id == check_id));
                                    db.Memes.Remove(mem);
                                }
                                catch { }
                            }
                            catch {
                                try
                                {
                                    db.Wall_memes_connected.RemoveRange(db.Wall_memes_connected.Where(x1 => x1.Something_one_id == id && x1.Something_two_id == from_id));
                                }
                                catch { }

                            }


                            db.SaveChanges();


                        }
                        catch{
                            return View("Personal_record", Record(check_id, "Personal_record"));
                        }
                    }
                    else if (from == "News")
                    {
                        try
                        {
                            db.Wall_memes_connected.RemoveRange(db.Wall_memes_connected.Where(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id));
                        }
                        catch { }
                        db.SaveChanges();
                    }

                    break;
                case "person_from_list":
                    //удаление человека из списка друзей человека\группы
                    //from группа друзья и тд
                    if (from== "Personal_record")
                    {
                        var list = db.Friends_connected.First(x1=>(x1.Something_one_id==id&&x1.Something_two_id==check_id)||(x1.Something_two_id==id&&x1.Something_one_id == check_id));
                        //foreach(var i in list)
                        //{
                            if(list.Something_one_id==check_id)
                            db.Followers_ignore_connected.Add(new Relationship_string_string_Followers_ignore_connected(list.Something_one_id, list.Something_two_id));
                            else
                                db.Followers_ignore_connected.Add(new Relationship_string_string_Followers_ignore_connected(list.Something_two_id, list.Something_one_id));
                            db.Friends_connected.Remove(list);

                        //}
                        db.SaveChanges();

                    }
                    else if (from == "Group_record")
                    {
                        
                            //проверка можно ли удалять
                            try
                            {
                                var gr_adm = db.Friends_connected.First(x1 => x1.Something_one_id == from_id && x1.Something_two_id == check_id && x1.Admin_group);
                            var list = db.Friends_connected.First(x1 => x1.Something_one_id ==from_id&&x1.Something_two_id==id);
                            //foreach (var i in list)
                            //{
                               
                                    db.Followers_ignore_connected.Add(new Relationship_string_string_Followers_ignore_connected(list.Something_one_id, list.Something_two_id));
                               db.Friends_connected.Remove(list);

                            //}

                        }
                            catch { }
                        
                        
                        db.SaveChanges();




                    }
                    break;
                case "message":
                    //удаление сообщения
                    try
                    {
                        //db.Messages_obg
                        var mess=db.Messages_one_dialog_connected.First(x1=>x1.Something_one_id==from_id&&x1.Something_two_id==id);
                        db.Messages_one_dialog_connected.Remove(mess);
                        int int_id = Convert.ToInt32(mess.Something_two_id);
                        db.Messages.Remove(db.Messages.First(x1=>x1.Id== int_id));
                    }
                    catch { }
                    db.SaveChanges();
                    break;
                case "person":
                    //удаление человека из бд
                    db.Users.Remove(db.Users.First(x1 => x1.Id == check_id));

                    db.SaveChanges();
                    break;
                case "group":
                    //удаление группы
                    db.Users.Remove(db.Users.First(x1 => x1.Id == check_id));

                    db.SaveChanges();
                    break;

            }

            //TODO временно
            return View("Personal_record", Record(check_id, "Personal_record"));
            //return View();
        }
        //TODO 
        public ActionResult Edit_group_record(string id)
        {
            //TODO 
            //
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var res = (Group_record)Group(id,"DB");
            var admins = db.Friends_connected.Where(x1 => x1.Something_one_id == id && x1.Admin_group).ToList();
            foreach(var i in admins)
            {

                if (i.Something_two_id == check_id)
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
        public ActionResult Action_image(string id, string action_m,string obg="")
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();

            switch (action_m)
            {
                case "change_album":
                    {
                        //TODO сейчас кто угодно может сменить альбом
                        int int_id = Convert.ToInt32(id);
                        var img=db.Images.First(x1 => x1.Id == int_id);
                        img.Albums = obg;
                        db.SaveChanges();
                    }
                    break;
                case "like":
                    {
                        Like_something(id);

                    }
                    break;
                case "repost":
                    {
                    }
                    break;
                case "comment":
                    {
                    }
                    break;
            }


            //var mem = Memes(id);
            return RedirectToAction("Record_photo_page", "Home", new { id_image = id, album_name = ""  });
            //return Record_photo_page(id,null);
            //return PartialView("Memes_partial", mem);
        }
                        //TODO 
        public ActionResult Action_memes(string id,string action_m)
        {
            //TODO 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            
            switch (action_m)
            {
                case "like":
                    {
                        Like_something( id);
                        //int id_int = Convert.ToInt32(id);
                        

                    }
                    
                    break;
                case "repost":
                    {
                        var pers =(Personal_record) Record(check_id, "DB");
                        
                        db.Wall_memes_connected.Add(new Relationship_with_memes(id,check_id,false));
                        bool fl = false;
                        var repost = db.Repost_connected.Where(x1=>x1.Something_one_id==id).ToList();
                        foreach(var i in repost)
                        {
                            if (i.Something_two_id == check_id)
                                fl = true;
                        }
                        if (!fl)
                            db.Repost_connected.Add(new Relationship_string_string_Repost_connected(id,check_id));

                        db.SaveChanges();

                    }
                    break;
                case "comment":
                    break;
            }
            var mem = Memes(id);

            return PartialView("Memes_partial", mem);
        }


        //-PARTIAL BLOCK------------------------------------------------------------------------------------------------------------------------------//
        [ChildActionOnly]
        public ActionResult Memes_partial(string from,string from_id,string id_mem)
        {
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var res = Memes(id_mem);
            ViewBag.from = from;
            ViewBag.from_id = from_id;
            return PartialView(res);
        }
        [HttpPost]
        public ActionResult Change_left_menu(string edit_text)
        {
            string Base_Menu_left = "Моя Cтраница,Personal_record,Новости,News,Сообщения,Messages,Группы,Groups_personal,Друзья,Friends,Фотографии,Albums,Музыка,Music,Видео,Video";
            
//

            //изменение
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pers = (Personal_record)Record(check_id, "info_person");
            
            if(pers.db.Menu_left.IndexOf(edit_text)!=-1)
            {
                //уже есть надо удалить
                string[] pers_menu_left = pers.db.Menu_left.Split(',');
                //List<string> res = new List<string>();
                pers.db.Menu_left = "";
                for (int i=0;i< pers_menu_left.Count();++i)
                {
                    if (pers_menu_left[i]!= edit_text)
                    {
                        if (!string.IsNullOrEmpty(pers_menu_left[i]))
                        {
                            //res.Add(pers_menu_left[i]);
                            //res.Add(pers_menu_left[++i]);
                            pers.db.Menu_left += pers_menu_left[i] + "," + pers_menu_left[++i] + ",";
                        }
                        
                    }
                    else
                    {
                        ++i;
                    }
                        
                }
                db.SaveChanges();
            }
            else
            {
                //нет надо добавить
                string[] Base_Menu_leftmass = Base_Menu_left.Split(',');
                string res_str="";
                for(int i=0;i< Base_Menu_leftmass.Count(); ++i)
                {
                    if (!string.IsNullOrEmpty(Base_Menu_leftmass[i]))
                    {
                        if(pers.db.Menu_left.IndexOf(Base_Menu_leftmass[i])!=-1&& pers.db.Menu_left.IndexOf(Base_Menu_leftmass[i+1]) != -1){
                            res_str += Base_Menu_leftmass[i]+","+ Base_Menu_leftmass[i+1]+",";
                        }
                        if(Base_Menu_leftmass[i]== edit_text)
                        {
                            res_str += Base_Menu_leftmass[i] + "," + Base_Menu_leftmass[i + 1] + ",";

                        }
                    }
                    ++i;
                }
                pers.db.Menu_left = res_str;
                db.SaveChanges();
            }
            
            
            if (pers != null)
                ViewBag.list_str = pers.db.Menu_left;
            ViewBag.id = check_id;
            return PartialView("Left_menu_personal");
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
            ViewBag.from = from;
            ViewBag.from_id = id;
            var res = Wall_memes_function(from, id, start);
            res.Reverse();
            return PartialView(res);





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
                    string tmp = null;
                    try
                    {
                        tmp = db.Friends_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id).Something_two_id;
                    }
                    catch
                    {
                        try
                        {
                            tmp = db.Followers_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id).Something_two_id;
                        }
                        catch
                        {
                            try
                            {
                                tmp = db.Followers_ignore_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id).Something_two_id;
                            }
                            catch
                            {

                            }
                        }
                    }
                     
                    //var tmp = ((Group_record)pers).db.Followers_id.Split(',').First(x1 => x1 == check_id);
                    if (tmp == null)
                        throw new Exception();
                    if (click)
                    {
                        //нужно отписать от группы
                        
                        //var mass_peop = ((Personal_record)pers_peop).db.Groups_id.Split(',');
                        
                        
                        //var mass = ((Group_record)pers).db.Followers_id.Split(',');

                        //((Group_record)pers).db.Followers_id = "";
                        
                        //не нужно вроде
                        //var mass_peop = db.Groups_connected.Where(x1 => x1.Something_two_id == check_id).ToList();

                        db.Groups_connected.Remove(db.Groups_connected.First(x1 => x1.Something_two_id == check_id && x1.Something_one_id == id));
                        //не нужно вроде
                        //var mass = db.Friends_connected.Where(x1 => x1.Something_one_id == id).ToList();

                        db.Friends_connected.Remove(db.Friends_connected.First(x1 => x1.Something_two_id == check_id && x1.Something_one_id == id));
                        db.Followers_connected.Remove(db.Followers_connected.First(x1 => x1.Something_two_id == check_id && x1.Something_one_id == id));
                        db.Followers_ignore_connected.Remove(db.Followers_ignore_connected.First(x1 => x1.Something_two_id == check_id && x1.Something_one_id == id));
                        ((Personal_record)pers_peop).db.Groups_count -= 1;
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
                        //((Group_record)pers).db.Followers_id += check_id + ",";
                        db.Friends_connected.Add(new Relationship_with_admin_group(id, check_id, false));
                        db.Groups_connected.Add(new Relationship_string_string_Groups_connected(id,check_id));
                        ((Personal_record)pers_peop).db.Groups_count += 1;
                        //((Personal_record)pers_peop).db.Groups_id +=id +",";
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
                    //var tmp = ((Personal_record)pers).db.Followers_id.Split(',').First(x1 => x1 == check_id);
                    int flag_srch = 0;
                    try
                    {
                        var tmp = db.Followers_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id).Something_two_id;
                        flag_srch = 1;
                    }
                    catch {
                        try
                        {
                            var tmp = db.Followers_ignore_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id).Something_two_id;
                            flag_srch = 2;
                        }
                        catch {
                            try
                            {
                                var tmp = db.Friends_connected.First(x1 => (x1.Something_one_id == id && x1.Something_two_id == check_id)||(x1.Something_one_id == check_id && x1.Something_two_id ==id )).Something_two_id;
                                flag_srch = 3;
                            }
                            catch { }
                        }
                    }
                    
                    
                if (flag_srch== 0)
                        throw new Exception();
                    if (click)
                    {
                        //нужно отписать от человека

                        //db.Followers_connected.Remove(db.Followers_connected.First(x1 => x1.Something_two_id == check_id && x1.Something_one_id == id));
                        //не нужно вроде
                        //var mass = db.Friends_connected.Where(x1 => x1.Something_one_id == id).ToList();
                        switch (flag_srch) {
                            case 1:
                                try
                                {
                                    db.Followers_connected.Remove(db.Followers_connected.First(x1 => x1.Something_two_id == check_id && x1.Something_one_id == id));
                                    ((Personal_record)pers_peop).db.Followers_count -= 1;
                                }
                                catch { }
                                break;
                            case 2:
                                try
                                {
                                    db.Followers_ignore_connected.Remove(db.Followers_ignore_connected.First(x1 => x1.Something_two_id == check_id && x1.Something_one_id == id));
                                    ((Personal_record)pers_peop).db.Followers_ignore_count -= 1;
                                }
                                catch { }
                                break;

                            case 3:
                                try
                                {
                                    db.Friends_connected.Remove(db.Friends_connected.First(x1 => (x1.Something_two_id == check_id && x1.Something_one_id == id)||(x1.Something_two_id ==id  && x1.Something_one_id == check_id)));
                                    db.Followers_ignore_connected.Add(new Relationship_string_string_Followers_ignore_connected(check_id, id));

                                    ((Personal_record)pers_peop).db.Friends_count -= 1;
                                }
                                catch { }
                                break;
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
                        int flag_srch = 0;
                        try
                        {
                            var tmp = db.Followers_connected.First(x1 => x1.Something_one_id == check_id && x1.Something_two_id ==id ).Something_two_id;
                            flag_srch = 1;
                        }
                        catch
                        {
                            try
                            {
                                var tmp = db.Followers_ignore_connected.First(x1 => x1.Something_one_id == check_id && x1.Something_two_id == id).Something_two_id;
                                flag_srch = 2;
                            }
                            catch
                            {
                                
                            }
                        }
                        if (flag_srch != 0)
                        {
                            db.Friends_connected.Add(new Relationship_with_admin_group(id, check_id));
                            if (flag_srch == 1)
                                db.Followers_connected.Remove(db.Followers_connected.First(x1 => x1.Something_one_id == check_id && x1.Something_two_id == id));

                            if (flag_srch == 2)
                                db.Followers_ignore_connected.Remove(db.Followers_ignore_connected.First(x1 => x1.Something_one_id == check_id && x1.Something_two_id == id));

                        }

                        else
                        {
                            db.Followers_connected.Add(new Relationship_string_string_Followers_connected(id, check_id));
                            ((Personal_record)pers).db.Followers_count += 1;
                        }
                        
                        
                        

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
            ViewBag.Left_menu =  "Моя Cтраница,Personal_record,Новости,News,Сообщения,Messages,Группы,Groups_personal,Друзья,Friends,Фотографии,Albums,Музыка,Music,Видео,Video";

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
        public ActionResult Add_new_message(HttpPostedFileBase[] uploadImage, string message_text, string Id_dialog)
        {
           var dialog= db.Messages_obg.First(x1=>x1.Id.ToString()== Id_dialog);
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pers = (Personal_record)Record(check_id, "DB");
            if (dialog.Person_id.IndexOf(check_id) != -1)
            {
                List<byte[]> photo_byte = Get_photo_post(uploadImage);
                
                var mes = new Message(message_text, check_id, pers.db.Name);
                if (photo_byte != null && photo_byte.Count > 0)
                {
                    mes.Image = photo_byte[0];
                    Img tmp = new Img(photo_byte[0]);
                    db.Images.Add(tmp);
                }
                db.Messages.Add(mes);
                db.SaveChanges();
                db.Messages_one_dialog_connected.Add(new Relationship_string_string_Messages_one_dialog_connected(dialog.Id.ToString(), mes.Id.ToString()));
                //dialog.Messages_id += mes.Id + ",";
                db.SaveChanges();
                //не уверен что нужно обращаться
                var res = Message_person_block(id: Id_dialog, person_id:null, bool_fullness: "Messages_one_dialog");

                ViewBag.Id_dialog = res.db.Id.ToString();
                return View("Messages_one_dialog",res.Messages);
            }
            else
            {
                //взлом?
            }

            return View();

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
                    
                    
                    db.Groups.Add(a);
                    db.SaveChanges();
                    db.Friends_connected.Add(new Relationship_with_admin_group(a.Id.ToString(),check_id,true));
                    db.Groups_connected.Add(new Relationship_string_string_Groups_connected(a.Id.ToString(), check_id));
                    pers.db.Groups_count += 1;
                    
                    db.SaveChanges();

                    ViewBag.add_success = "Группа создана";
                    ViewBag.id=a.Id;
                }

            }
            
            
            

            return PartialView("Add_new_group_ajax");
        }
        [HttpPost]
        public ActionResult Add_new_image(HttpPostedFileBase[] uploadImage, string for_what,string from,string Album_name="")
        {
            IPage_view res_page = null;
            var lst1 = Get_photo_post(uploadImage);
            var lst2 = Get_photo_post(lst1, Album_name);
            //ViewBag.My_page = false;
            if (from == "person")
            {
                string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                res_page  = Record(check_id, "DB");
                //ViewBag.My_page = true;
                db.Images.Add(lst2[0]);
                db.SaveChanges();
                

                ((Personal_record)res_page).db.Images_count += 1;
                db.Images_connected.Add(new Relationship_with_images(lst2[0].Id.ToString(),check_id, for_what == "main_img"?true:false));

                db.SaveChanges();

                res_page = Record(check_id, "Personal_record");
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
                            //ViewBag.My_page = true;
                            var pers = (Personal_record)Record(id, "DB");
                            res_db = new Memes(string.Concat(pers.db.Name, " ", pers.db.Surname), id)
                            {
                                Description = Description_mem
                            };
                            List<byte[]> photo_byte = Get_photo_post(uploadImage);
                            if (photo_byte.Count == 1)
                            {
                                res_db.Image = photo_byte[0];
                            }
                            res_db.Source_type = "Personal_record";
                            db.Memes.Add(res_db);

                            if (photo_byte.Count > 1)
                            {
                                foreach (var i in photo_byte)
                                {
                                    //res.Images.Add(i);
                                    Img tmp = new Img(i);
                                    db.Images.Add(tmp);
                                    db.SaveChanges();
                                    db.Images_connected.Add(new Relationship_with_images(res_db.Id.ToString(), tmp.Id.ToString(), false));
                                    
                                }
                            }
                            db.SaveChanges();
                            //???
                            //res.db = res_db;
                            //pers.Wall.Add(res);
                            //var memes = new Relationship_with_memes(res_db.Id.ToString(), pers.db.Id.ToString(), false);
                            db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), pers.db.Id.ToString(), false));
                            
                            pers.db.Wall_count += 1;

                            db.SaveChanges();
                            //раскидать друзьям подписчикам
                            {
                                // var mass_fr = pers.db.Friends_id.Split(',');
                                //TODO еще и всем фоловерам отправлять
                                {
                                    var mass_fr = db.Friends_connected.Where(x1 => x1.Something_one_id == check_id || x1.Something_two_id == check_id).ToList();
                                    foreach (var i in mass_fr)
                                    {
                                        if (i.Something_one_id == check_id)
                                        {
                                            db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), i.Something_two_id, true));
                                        }
                                        else
                                        {
                                            db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), i.Something_one_id, true));
                                        }


                                    }
                                }
                                {
                                    var mass_fr = db.Followers_connected.Where(x1 => x1.Something_one_id == check_id).ToList();
                                    foreach (var i in mass_fr)
                                    {

                                        db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), i.Something_two_id, true));

                                    }
                                }
                                {
                                    var mass_fr = db.Followers_ignore_connected.Where(x1 => x1.Something_one_id == check_id ).ToList();
                                    foreach (var i in mass_fr)
                                    {
                                        db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), i.Something_two_id, true));
                                    }
                                }
                                


                                db.SaveChanges();
                            }

                            res_page = Record(id, "Personal_record"); ;
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
                                db.Images_connected.Add(new Relationship_with_images(res_db.Id.ToString(), tmp.Id.ToString(), false));

                                
                            }
                        }
                        res_db.Source_type = "Group_record";
                        db.Memes.Add(res_db);
                        db.SaveChanges();
                        //???
                        //res.db = res_db;
                        //pers.Wall.Add(res);

                        db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), pers.db.Id.ToString(), false));

                        pers.db.Wall_count += 1;

                        db.SaveChanges();

                        //раскидать друзьям подписчикам
                        {
                            var mass_fr = db.Friends_connected.Where(x1 => x1.Something_one_id == id ).ToList();
                            foreach (var i in mass_fr)
                            {
                                
                                    db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), i.Something_two_id, true));
                                


                            }
                        }
                        db.SaveChanges();
                       

                        res_page = (IPage_view)pers;
                        //res.db = res_db;

                        return View("Group_record",(Group_record)Group(id));

                        //сейчас
                        return View("Group_record", res_page);
                    }
                    break;
            }
           
                return View("Personal_record", Record(((Personal_record)res_page).db.Id));
        }
        
public Message_obg_record Message_person_block(string id,string person_id,int start_obg=0, int start_mes = 0, string bool_fullness = "Messages")
        {
            //Messages
            //Messages_one_dialog
            Message_obg_record res = null;
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(id))
            {
                 res = new Message_obg_record(db.Messages_obg.First(x1 => x1.Id.ToString() == id));
                //не уверен TODO заполнение перед отправкой
            }
            else
            {
                

                
                try
                {
                    if (person_id == check_id)
                    {
                        
                        res = new Message_obg_record(db.Messages_obg.First(x1 => x1.Person_id.IndexOf(check_id + "," + check_id)!=-1));

                    }
                    else
                    {
                        res = new Message_obg_record(db.Messages_obg.First(x1 => (x1.Person_id.IndexOf(person_id) != -1) && (x1.Person_id.IndexOf(check_id) != -1)));
                        //не уверен  TODO заполнение перед отправкой
                    }
                }
                catch
                {
                    var t = new Message_obg() ;
                    t.Person_id += check_id + "," + person_id + ",";
                    db.Messages_obg.Add(t);
                    db.SaveChanges();
                    id = t.Id.ToString();
                    db.Messages_dialog_person_connected.Add(new Relationship_string_string_Messages_dialog_person_connected( t.Id.ToString(), check_id));
                    if(person_id != check_id)
                    db.Messages_dialog_person_connected.Add(new Relationship_string_string_Messages_dialog_person_connected(t.Id.ToString(), person_id));
                    db.SaveChanges();
                    res = new Message_obg_record(t);
                }
                
            }
            

            

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
                    //var str = db.Messages_one_dialog_connected.Where(x1 => x1.Something_one_id == id).ToList();

                    var str = db.Messages_one_dialog_connected.Where(x1 => x1.Something_one_id == id).ToList().Last();

                   // var str = db.Messages_one_dialog_connected.Where(x1 => x1.Something_one_id == id).Reverse().First();
                    int tmp =Convert.ToInt32( str.Something_two_id);
                    var a = db.Messages.First(x1 => x1.Id == tmp);
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
                    //var str = db.Messages_one_dialog_connected.Where(x1 => x1.Something_one_id == id).Reverse().Take(30).Reverse().ToList();

                    var str = db.Messages_one_dialog_connected.Where(x1 => x1.Something_one_id == id).ToList();
                    str.Reverse();
                    str = str.Take(30).ToList();
                    str.Reverse();
                    List<Message> temp_mass_message = new List<Message>();
                    foreach(var i in str)
                    {
                        int tmp = Convert.ToInt32(i.Something_two_id);
                        var a = db.Messages.First(x1 => x1.Id == tmp);
                        temp_mass_message.Add(a);
                    }
                    
                    //var temp_mass_message_array = temp_mass_message.ToArray().Reverse();
                   // Array.Reverse(temp_mass_message_array);
                    res.Messages.AddRange(temp_mass_message);

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
            string id_mem = not_res.Id.ToString();
            Memes_record res=new Memes_record(not_res);
            try
            {
                
                var mass=db.Images_connected.Where(x1 => x1.Something_one_id == id_mem).ToList();
                foreach(var i in mass)
                {
                    int id_img =Convert.ToInt32( i.Something_two_id.ToString());
                    res.Images.Add((db.Images.First(x1 => x1.Id == id_img).bytes));
                }
                
            }
            catch
            {

            }

            //var mass_liked = not_res.Liked_id.Split(',');
            var mass_liked = db.Liked_connected.Where(x1=>x1.Something_one_id== id_mem).ToList();
            //ViewBag.count_like =  mass_liked.Count() > 1 ? mass_liked.Count() - 1 : 0;
            ViewBag.count_like = mass_liked.Count;
            //проверка на лайк
            foreach (var i in mass_liked)
            {
                if (!string.IsNullOrEmpty(i.Something_two_id))
                {
                    if (i.Something_two_id == check_id)
                        ViewBag.like = true;
                    
                }
            }

            //var mass_repost = not_res.Repost_id.Split(',');
            var mass_repost = db.Repost_connected.Where(x1 => x1.Something_one_id== id_mem).ToList();
            ViewBag.count_repost = mass_repost.Count;
            //проверка на репост
            foreach (var i in mass_repost)
            {
                if (!string.IsNullOrEmpty(i.Something_two_id))
                {
                    if (i.Something_two_id == check_id)
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
            //ViewBag.My_page = false;
            IPage_view res = null;
            int int_id = Convert.ToInt32(id);
           
            
            var not_res= db.Groups.First(x1 => x1.Id == int_id);
            

            if (bool_fullness == "Group_record")
            {
                //TODO надо доделать что бы поля тоже заполнялись как в рекорде
                res = new Group_record(not_res);
                string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                try
                {
                    //var str = ((Group_record)res).db.Followers_id.Split(',');
                    var str = db.Friends_connected.Where(x1 => x1.Something_one_id== id).ToList();
                    ViewBag.Count_followers = str.Count;
                     str.Reverse();
                    
                    foreach(var i in str.Take(6))
                    {
                        var a = ((Person_short)Record(i.Something_two_id, "Person_short"));
                        ((Group_record)res).Followers.Add(a);
                    }
                    
                        
                    
                }
                catch
                {

                }
                try
                {
                    //var str = ((Group_record)res).db.Admins_id.Split(',');
                    var str = db.Friends_connected.Where(x1 => x1.Something_one_id == id&&x1.Admin_group).ToList();
                    //ViewBag.My_page = false ;
                    foreach (var i in str)
                    {

                        if (i.Something_two_id == check_id)
                        {
                            ViewBag.My_page = true;
                        }
                    }
                    str = str.Take(5).ToList();
                    foreach(var i in str)
                    {
                        var a = ((Person_short)Record(i.Something_two_id, "Person_short"));
                        ((Group_record)res).Admins.Add(a);
                    }
  

                }
                catch
                {

                }

                try
                {
                    //var str = ((Group_record)res).db.Images_id.Split(',');
                    var str = db.Images_connected.Where(x1 => x1.Something_one_id == id).ToList();
                    str.Reverse();
                    foreach (var i in str.Take(5))
                    {
                        int int_id_img = Convert.ToInt32(i.Something_two_id);
                        ((Group_record)res).Images.Add(db.Images.First(x1 => x1.Id == int_id_img));
                    }
                    


                }
                catch
                {

                }
                try
                {

                    var str = db.Images_connected.Where(x1 => x1.Something_one_id == id&&x1.Main).ToList().Last();
                    //var str = db.Images_connected.Where(x1 => x1.Something_one_id == id && x1.Main).Reverse().First();
                    int int_id_img = Convert.ToInt32(str.Something_two_id);
                    ((Group_record)res).Main_images.Add(db.Images.First(x1 => x1.Id == int_id_img));
                    

                }
                catch
                {

                }
                try
                {

                    //var str = ((Group_record)res).db.Groups_id.Split(',');
                    var str = db.Groups_connected.Where(x1=>x1.Something_one_id==id).Take(5).ToList();
                    foreach(var i in str)
                    {
                        ((Group_record)res).Groups.Add((Group_short)Group(i.Something_two_id, "Group_short"));
                    }
                    
                }
                catch
                {

                }

            }
            if (bool_fullness == "Followers")
            {
                res = new Group_record(not_res);

                //var lst = not_res.Followers_id.Split(',');
                var lst = db.Friends_connected.Where(x1=>x1.Something_one_id==id).ToList();
                ViewBag.Count_followers = lst.Count;
                foreach (var i in lst)
                {
                    if (string.IsNullOrEmpty(i.Something_two_id))
                    {
                        ((Group_record)res).Followers.Add((Person_short)Record(i.Something_two_id, "Person_short"));
                    }
                    
                }
            }
            if (bool_fullness == "Admins")
            {
                res = new Group_record(not_res);
                //var lst = not_res.Admins_id.Split(',');
                var lst = db.Friends_connected.Where(x1 => x1.Something_one_id == id && x1.Admin_group).ToList();
                foreach (var i in lst)
                {
                    if (string.IsNullOrEmpty(i.Something_two_id))
                    {
                        ((Group_record)res).Admins.Add((Person_short)Record(i.Something_two_id, "Person_short"));
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
                var count_fl = db.Friends_connected.Where(x1 => x1.Something_one_id == id).Count();
                //TODO мб тут не null я хз че туда надо
                res = new Group_short(not_res, count_fl,null);
                
                ViewBag.Count_followers = count_fl;
                //((Group_short)res).Count_followers = ViewBag.Count_followers;
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
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();


            /*ViewBag.My_page = false;
            if (id == check_id)
                ViewBag.My_page = true;*/





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
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                    }
                    if (bool_fullness == "Person_short")
                    {
                        byte[] a_b_tmp = null;
                        try
                        {
                            //var img = res.db.Main_images_id.Split(',');
                            var img = db.Images_connected.Where(x1 => x1.Something_two_id == id&&x1.Main).ToList().Last();
                            //var img = db.Images_connected.Where(x1 => x1.Something_two_id == id && x1.Main).Reverse().First();
                            int id_tmp = Convert.ToInt32(img.Something_one_id);

                            //var a_b123_tmp = db.Images.First(x1 => x1.Id == id_tmp);


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
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        try
                        {

                            var main_img = db.Images_connected.Where(x1 => x1.Something_two_id == id && x1.Main).ToList().Last();

                            //сейчас
                            //var f = db.Images_connected.First();
                            //var g = db.Images_connected.Where(x1 => x1.Something_two_id == id && x1.Main).ToList();


                            //var main_img = db.Images_connected.Where(x1 => x1.Something_two_id == id && x1.Main).Reverse().First();
                            int id_tmp = Convert.ToInt32(main_img.Something_one_id);

                            //var a_b123_tmp = db.Images.First(x1 => x1.Id == id_tmp);
                           

                            var a_b_tmp = db.Images.First(x1 => x1.Id == id_tmp);//.bytes


                            res.Main_images.Add(a_b_tmp);
                        }
                        catch
                        {

                        }
                        try
                        {
                            //ФОТО, потом под 1 засунуть мб
                            //var not_main_img = res_ap.Images_id.Split(',');
                            var not_main_img = db.Images_connected.Where(x1 => x1.Something_two_id == id).ToList();
                            not_main_img.Reverse();
                            foreach (var i in not_main_img.Take(5))
                            {
                                int id_tmp = Convert.ToInt32(i.Something_one_id);
                                res.Images.Add(db.Images.First(x1 => x1.Id == id_tmp));//.bytes
                            }
                            
                        }
                        catch
                        {

                        }
                        try
                        {
                            //заполнение групп
                            //List<string> gr = res.db.Groups_id.Split(',').ToList();
                            var gr = db.Groups_connected.Where(x1 => x1.Something_two_id == id).Take(5);
                            foreach(var i in gr.ToList())
                            {
                                res.Groups.Add((Group_short)Group(i.Something_one_id, "Group_short"));
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
                            //var lst = res.db.Friends_id.Split(',');
                            var lst = db.Friends_connected.Where(x1 => x1.Something_one_id == id|| x1.Something_two_id == id).Take(6).ToList();

                            foreach(var i in lst)
                            {
                                if(i.Something_one_id!=id)
                                res.Friends.Add((Person_short)Record(i.Something_one_id, "Person_short"));
                                else
                                    res.Friends.Add((Person_short)Record(i.Something_two_id, "Person_short"));
                            }
                            

                        }
                        catch
                        {

                        }


                    }
                    
                    
                    //мемы стена
                    if (bool_fullness == "Wall")
                    {
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        /*
                        try
                        {
                            //var tmp_mem = new List<Memes_record>();
                           var tmp = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == id && !x1.News).Reverse().Skip(start).Take(10).ToList();
                            foreach(var i in tmp)
                            {
                                //res.Wall.Add(Memes(i.Something_one_id));
                                var mem_tmp = db.Memes.First(x1 => x1.Id.ToString() == i.Something_one_id);
                                res.Wall.Add(new Memes_record(mem_tmp));
                            }
                           
                        }
                        catch { }
                        */

                    }
                    //мемы новости
                    if (bool_fullness == "News")
                    {
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        /*
                        try
                        {
                            //var tmp_mem = new List<Memes_record>();
                            var tmp = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == id && x1.News).Reverse().Skip(start).Take(10).ToList();
                            foreach (var i in tmp)
                            {
                                //res.News.Add(Memes(i.Something_one_id));
                                var mem_tmp = db.Memes.First(x1 => x1.Id.ToString() == i.Something_one_id);
                                res.News.Add(new Memes_record(mem_tmp));
                            }

                        }
                        catch { }
                        */

                    }
                    if (bool_fullness == "Friends")
                    {
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        var lst = db.Friends_connected.Where(x1 => x1.Something_one_id == id || x1.Something_two_id == id).ToList();

                        foreach (var i in lst)
                        {
                            if (i.Something_one_id != id)
                                res.Friends.Add((Person_short)Record(i.Something_one_id, "Person_short"));
                            else
                                res.Friends.Add((Person_short)Record(i.Something_two_id, "Person_short"));
                        }

                    }
                    if (bool_fullness == "Followers_ignore")//мб разделить
                    {
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        var lst = db.Followers_ignore_connected.Where(x1 => x1.Something_one_id == id).ToList();

                            foreach (var i in lst)
                            {

                                res.Followers_ignore.Add((Person_short)Record(i.Something_two_id, "Person_short"));

                            }
                        
                        
 

                    }
                    
                    if (bool_fullness == "Followers")//мб разделить
                    {
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        var lst = db.Followers_connected.Where(x1 => x1.Something_one_id == id).ToList();
                        //сейчас
                        var tmp = db.Followers_connected.ToList();

                        foreach (var i in lst)
                        {

                            res.Followers.Add((Person_short)Record(i.Something_two_id, "Person_short"));

                        }
                    }
                    if (bool_fullness == "Send_follow")//мб разделить
                    {
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        var lst1 = db.Followers_connected.Where(x1 => x1.Something_two_id == id).ToList();
                        var lst2 = db.Followers_ignore_connected.Where(x1 => x1.Something_two_id == id).ToList();
                        List<Relationship_string_string_Followers_connected> lst = new List<Relationship_string_string_Followers_connected>();
                        lst.AddRange(lst1);
                        foreach(var i in lst2)
                        {
                            lst.Add(new Relationship_string_string_Followers_connected(i.Something_one_id,i.Something_two_id));
                        }
                        foreach (var i in lst)
                        {

                            res.Followers.Add((Person_short)Record(i.Something_one_id, "Person_short"));

                        }
                    }
                    if (bool_fullness == "Groups_all")
                    {
                        try
                        {
                            var lst = db.Groups_connected.Where(x1 => x1.Something_two_id == id).ToList();
                            foreach(var i in lst)
                            {
                                res.Groups.Add(((Group_short)Group(i.Something_one_id, "Group_short")));
                            }
                            
                        }
                        catch
                        {

                        }
                        

                    }
                    if (bool_fullness == "Messages")
                    {
                        //можео просто тру оставить
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        try
                        {
                            //var tmp=db.Messages_dialog_person_connected.ToList();
                            var lst = db.Messages_dialog_person_connected.Where(x1 => x1.Something_two_id == id).ToList();

                            foreach(var i in lst)
                            {
                                res.Message.Add(Message_person_block(i.Something_one_id, person_id: null));
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
        public List<Img> Get_photo_post(IEnumerable<byte[]> Images,string Album_name)
        {
            List<Img> res = new List<Img>();
            foreach(var i in Images)
            {
                res.Add(new Img(i) { Albums= Album_name });
            }
            return res;
        }

            public List<byte[]> Get_photo_post(HttpPostedFileBase[] uploadImage)
        {

            /* сохранение картинок как файл ...
              HttpPostedFileBase image = Request.Files["fileInput"];
            
            if (image != null && image.ContentLength > 0 && !string.IsNullOrEmpty(image.FileName))
            {
                string fileName = image.FileName;
                image.SaveAs(Path.Combine(Server.MapPath("Images"), fileName));
            }
             
             * */
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

        void Like_something(string id)
        {
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var mass_liked = db.Liked_connected.Where(x1 => x1.Something_one_id == id).ToList();

            bool liked = false;

            foreach (var i in mass_liked)
            {
                if (!string.IsNullOrEmpty(i.Something_two_id))
                {
                    if (i.Something_two_id == check_id)
                        liked = true;

                }
            }
            if (!liked)
            {
                db.Liked_connected.Add(new Relationship_string_string_Liked_connected(id, check_id));
                ViewBag.like = true;
                ViewBag.count_like += 1;



            }
            else
            {
                //уже лайкнул
                db.Liked_connected.Remove(db.Liked_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id));

                ViewBag.like = false;
                ViewBag.count_like -= 1;
            }
            db.SaveChanges();



            return;
        }
            IEnumerable<string> Wall_memes_function(string from, string id, int start = 0)
        {
            if (string.IsNullOrEmpty(id))
                id = System.Web.HttpContext.Current.User.Identity.GetUserId();

            //IPage_view pers = null;
            IEnumerable<string> res = null;
            ViewBag.start = start;
            List<string> not_res = new List<string>();
            switch (from)
            {
                case "Personal_record":
                    // pers = Record(id, "Wall", start);
                    
                    try
                    {
                        //var tmp_mem = new List<Memes_record>();
                        {

                            //СЕЙЧАС
                            var tmp1 = db.Wall_memes_connected.Where(x1=>true).ToList();
                            int a = 0;
                        }
                        var tmp = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == id && !x1.News).ToList();
                        tmp.Reverse();
                       // tmp = tmp.Skip(start).Take(10).ToList();
                        foreach (var i in tmp.Skip(start).Take(10))
                        {
                            not_res.Add(i.Something_one_id);
                        }

                    }
                    catch { }
                    res = not_res;



                    //TODO хз нужно ли
                    //if (start == 0)
                    // if (res.Count() == 0)
                    // ViewBag.Count = 0;
                    ViewBag.Count = start + res.Count();

                    return res;
                    break;

                case "Group_record":
                    //pers = Group(id, "Wall", start);

                    try
                    {
                        //var tmp_mem = new List<Memes_record>();
                        var tmp = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == id && !x1.News).ToList();
                        tmp.Reverse();
                        
                        foreach (var i in tmp.Skip(start).Take(10))
                        {
                            not_res.Add(i.Something_one_id);
                        }

                    }
                    catch { }
                    res = not_res;



                    
                    return res;
                    break;

                default://"News"
                    string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    //pers = Record(check_id, "News", start);
                    try
                    {
                        //var tmp_mem = new List<Memes_record>();
                        var tmp = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == check_id && x1.News).ToList();
                        tmp.Reverse();
                        foreach (var i in tmp.Skip(start).Take(10))
                        {
                            not_res.Add(i.Something_one_id);
                        }

                    }
                    catch { }
                    res = not_res;



                    //TODO хз нужно ли
                    //if (start == 0)
                    // if (res.Count() == 0)
                    // ViewBag.Count = 0;
                    ViewBag.Count = start + res.Count();
                    return res;
                    break;
            }
        }


        }
}


