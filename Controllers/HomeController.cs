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

            
            bool loginned = false;
            string id = null;
            Personal_record res = null;
            //ViewBag.My_page = false;
            try
            {
                id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    throw new Exception();
                var test=db.Users.First(x1=>x1.Id==id);
                if (test==null)
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
                    return View(((Personal_record)res).Groups);
                    break;
                case "Group_record":
                    //groups_friends
                    res = Group(id, "groups_friends");
                    return View(((Group_record)res).Groups);
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
        public ActionResult Record_photo_page( string id_image, string album_name,string from_id,string from_type,bool main_img=false)//string from,,string album_name
        {
            //что то типо параметров поиска хз пока что
            //TODO закидывать фото юзеру и проверять что бы его фото отдавать продумать мб вообще не нужно
            //смотреть че как и фото отображать
            //TODO надо соседние фото определить что бы листать можно было
            //TODO список альбомов польователя??? пока что так
            ViewBag.from_id = from_id;
            ViewBag.from_type = from_type;
            ViewBag.main_img = main_img;
            var check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //var list_album = new List<Img>();
            Memes_record res = null;



            if (!string.IsNullOrEmpty(id_image))
            {
                res = Memes(id_image);
                int int_id_image = Convert.ToInt32(id_image);
                
                               
                bool? prev_bool = null;
                {
                    var album_tmp = db.Albums.Where(x1 => x1.Source_id == check_id).ToList();
                    var str_tmp_album = new List<string>();
                    foreach (var i in album_tmp)
                    {
                        str_tmp_album.Add(i.Name);
                    }
                    
                    if (from_type == "Personal_record")
                    {
                        if(res.db.Source_id == check_id)
                        {
                            ViewBag.Albums = str_tmp_album;
                        }
                    }
                    else
                    {
                        try
                        {
                            var tmp = db.Friends_connected.First(x1 => x1.Something_one_id == from_id && x1.Something_two_id == check_id && x1.Admin_group);
                            ViewBag.Albums = str_tmp_album;
                        }
                        catch { }
                        }
                }
                //album_name = string.IsNullOrEmpty(album_name) ?res.Image.Albums: album_name;
                var mem_rel=db.Wall_memes_connected.Where(x1 => x1.Something_two_id == from_id && x1.Who == from_type&&x1.Image).ToList();


                
                try
                {
                    // если картинка первая или последняя мб можно проще\быстрее хз
                    ViewBag.Next_img_id = mem_rel[0].Something_one_id;
                    ViewBag.Preview_img_id = mem_rel[mem_rel.Count - 1].Something_one_id;
                }
                catch { }
                    
                
                foreach (var i in mem_rel)
                //for(int i=0;i> mem_rel.Count;++i)
                {
                    if (prev_bool == null|| prev_bool==true)
                    {

                   
                    int int_id = Convert.ToInt32(i.Something_one_id);
                    var mem=db.Memes.First(x1=>x1.Id== int_id);
                    if(mem.Image_id!=null)
                    {
                        int int_id_img= Convert.ToInt32(mem.Image_id);
                        var img=db.Images.First(x1 => x1.Id == int_id_img);
                        if ( string.IsNullOrEmpty( album_name)?true: img.Albums == album_name)//img.Albums ==
                                if((main_img==true&&img.Main==true)||main_img!=true)
                            {
                                if (mem.Id == Convert.ToInt32(id_image))
                                {
                                    prev_bool = true;
                                }
                                else
                                {
                                    if (prev_bool == null)
                                    {
                                        ViewBag.Preview_img_id = mem.Id.ToString();
                                    }
                                    if (prev_bool==true)
                                    {
                                        ViewBag.Next_img_id = mem.Id.ToString();
                                            prev_bool = false;
                                    }
                                }
                                
                        }
                           

                        }
                    }
                }
                
            }
            else
            {

            }


            //
            return PartialView(res);
        }
        
        public ActionResult Album_photo(string id_user, string from= "Personal_record", string album_name = "")
        {
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            id_user = string.IsNullOrEmpty(id_user) ? check_id : id_user;
            ViewBag.id_user = id_user;
            
            var list_photo_all_id = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == id_user&&x1.Image&&x1.Who==from).ToList();
            var res = new List<Memes_record>();
            ViewBag.Album_name = string.IsNullOrEmpty( album_name)? "Все Альбомы": album_name;
            foreach (var i in list_photo_all_id)
            {
                
                int int_id = Convert.ToInt32(i.Something_one_id);
                try
                {
                    var image_memes = db.Memes.First(x1 => x1.Id == int_id);
                    int int_id_img = Convert.ToInt32(image_memes.Image_id);
                    var image = db.Images.First(x1=>x1.Id== int_id_img);
                    if (image.Albums == album_name)
                    {
                        res.Add(Memes(i.Something_one_id));
                    }
     
                }
                catch { }
                
                

            }

            res.Reverse();
            return PartialView(res);
        }



       
        public ActionResult Albums(string id,string album_name="",string from_type= "Personal_record")
        {
            
            
            //string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.id_user = id;
            ViewBag.from = from_type;
            ViewBag.album_name = album_name;
            var res = Albums_function(id, album_name, from_type);


            return View(res);
        }
        
        public ActionResult News(string id)
        {
            
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var res = (Personal_record)Record(check_id, "News");

            return View(res);
        }
        
        public ActionResult Messages(string id)
        {
            
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pers = Record(check_id, "Messages");

            return View(((Personal_record)pers).Message);
        }
        
        public ActionResult Messages_one_dialog(string id,string person_id)
        {
             
 
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.My_id = check_id;

            //TODO проверять есть ли доступ к этой переписке
            var res = Message_person_block(id:id, person_id: person_id, bool_fullness: "Messages_one_dialog");

            ViewBag.Id_dialog = res.db.Id.ToString();
            return View(res);
        }
        
        public ActionResult Friends(string from, string id,string what)
        {
            
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
        
        public ActionResult Followers_group(string from, string id, string what)
        {

            //what= Friends, Admins ,Followers
            /*
            if (string.IsNullOrEmpty(id))
            {
                id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }*/
            ViewBag.id = id;
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
                            var list = db.Wall_memes_connected.Where(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id);
                            Remove_memes(id, check_id);
                        }
                        catch
                        {
                            
                           // return View("Personal_record", Record(check_id, "Personal_record"));
                        }
                        return RedirectToAction("Personal_record", "Home", new { id = check_id });
                    }
                    else if (from == "Group_record")
                    {
                        try
                        {
                            var pers = db.Friends_connected.First(x1 => x1.Something_one_id == from_id && x1.Something_two_id == check_id && x1.Admin_group&&!x1.Person);

                            Remove_memes(id, from_id);


                            //db.SaveChanges();
                        }
                        catch
                        {
                            
                            //return View("Personal_record", Record(check_id, "Personal_record"));
                        }
                        return RedirectToAction("Group_record", "Home", new { id = from_id });


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
                            var list = db.Wall_memes_connected.First(x1 => x1.Who == "Personal_record" && x1.Something_one_id == id && x1.Something_two_id == check_id);
                            if (list.Image)
                            {
                                list.News = null;
                                db.SaveChanges();
                            }
                            else
                            {
                                db.Wall_memes_connected.Remove(list);


                                try
                                {
                                    int int_id = Convert.ToInt32(id);
                                    var mem = db.Memes.First(x1 => x1.Id == int_id && x1.Source_id == check_id);
                                    //db.Memes.Remove(mem);
                                    //мем человека который удаляет и мем нужно удалить
                                    Remove_memes(id, check_id);
                                }
                                catch
                                {
                                    //мем НЕ человека который удаляет и мем НЕ нужно удалять

                                    try
                                    {
                                        db.Repost_connected.Remove(db.Repost_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id));

                                    }
                                    catch { }
                                    try
                                    {
                                        db.Wall_memes_connected.RemoveRange(db.Wall_memes_connected.Where(x1 =>x1.Who== "Personal_record" && x1.Something_one_id == id && x1.Something_two_id == check_id));
                                    }
                                    catch { }
                                }

                                db.SaveChanges();
                            }
                            

                        }
                        catch
                        {
                            
                            //return View("Personal_record", Record(check_id, "Personal_record"));
                        }
                        return RedirectToAction("Personal_record", "Home", new { id = check_id });
                    }
                    else if (from == "Group_record")
                    {
                        try
                        {
                            var pers = db.Friends_connected.First(x1 => x1.Something_one_id == from_id && x1.Something_two_id == check_id && x1.Admin_group&&!x1.Person);
                            var list = db.Wall_memes_connected.First(x1 => x1.Who == "Group_record" && x1.Something_one_id == id && x1.Something_two_id == from_id);
                            if (list.Image)
                            {
                                list.News = null;
                                db.SaveChanges();
                            }
                            else
                            {
                                Remove_memes(id, from_id);
                            }
                        }
                        catch{
                            
                            //return View("Personal_record", Record(check_id, "Personal_record"));
                        }
                        return RedirectToAction("Group_record", "Home", new { id = from_id });
                    }
                    else if (from == "News")
                    {
                        try
                        {
                            db.Wall_memes_connected.RemoveRange(db.Wall_memes_connected.Where(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id&&x1.News==true));
                        }
                        catch { }
                        db.SaveChanges();
                    }

                    break;
                case "person_from_list":
                    //удаление человека из списка друзей человека\группы
                    //from группа друзья и тд
                    /*if (from== "Personal_record")
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




                    }*/
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
                        var memess=db.Message_mem_connected.RemoveRange(db.Message_mem_connected.Where(x1 => x1.Something_one_id == mess.Something_two_id));
                        foreach(var i in memess)
                        {
                            Remove_memes(i.Something_two_id,check_id);
                        }
                    }
                    catch { }
                    db.SaveChanges();
                    break;
                case "person":
                    //удаление человека из бд
                    //db.Users.Remove(db.Users.First(x1 => x1.Id == check_id));

                    db.SaveChanges();
                    break;
                case "group":
                    //удаление группы
                    //db.Users.Remove(db.Users.First(x1 => x1.Id == check_id));

                    db.SaveChanges();
                    break;
                case "comment":
                    //удаление коммента
                    //string what,string from_id,string from,string id)
                    {
                        int int_id_com = Convert.ToInt32(id);
                        var comm = db.Comments.First(x1 => x1.Id == int_id_com);
                        db.Liked_connected.RemoveRange(db.Liked_connected.Where(x1=>x1.Something_one_id==id));
                        if (comm.Person_id == check_id)
                        {
                            db.Comments.Remove(comm);
                            //Mem_comment_connected images memes
                            var rel=db.Mem_comment_connected.RemoveRange(db.Mem_comment_connected.Where(x1=>(x1.Something_one_id==id&&x1.What_one== "Comment") || (x1.Something_two_id == id&& x1.What_one == "Memes")));
                            foreach(var i in rel)
                            {

                                
                                if(i.What_one== "Comment")
                                {
                                    //int int_id_mem = Convert.ToInt32(i.Something_two_id);
                                    var mem = Remove_memes(i.Something_two_id,check_id);
                                }
                                
                            }
                        }
                    }
                    

                    db.SaveChanges();
                    break;

            }
            return RedirectToAction("Personal_record", "Home", new { id = check_id });
            //return View("Personal_record", Record(check_id, "Personal_record"));
            //return View();
        }
        
        public ActionResult Edit_group_record(string id)
        {

            //
            ViewBag.id = id;
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var res = (Group_record)Group(id,"DB");
            var admins = db.Friends_connected.Where(x1 => x1.Something_one_id == id && x1.Admin_group&&!x1.Person).ToList();
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
         
        public ActionResult Edit_personal_record()
        {
             
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.list_menu = new string[] {"Основное","Контакты","Интересы","Образование","Карьера","Военная служба","Жизненная позиция" };

            return View();
        }
         
        public ActionResult Action_comment(string id, string action_m, string size, string obg = "")
        {
            
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();

            switch (action_m)
            {
                case "edit":
                    {
                        int int_id_com = Convert.ToInt32(id);
                        var comment=db.Comments.First(x1=>x1.Id== int_id_com);
                        if (comment.Person_id == check_id)
                        {
                            comment.Comment_text = obg;
                        }
                        db.SaveChanges();
                    }
                    break;
                case "like":
                    {
                        
                        Like_something(id, "Comment");

                    }
                    break;
                
                   
                
                    
            }
            
            return RedirectToAction("One_comments_partial", "Home", new { from="", from_id="", id_mem = id, size = size });

        }
        

            public ActionResult Action_memes(string id,string action_m,string from, string obg = "")
        {
            
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            
            switch (action_m)
            {
                case "like":
                    {
                        Like_something( id, "Memes");

                    }
                    
                    break;
                case "repost":
                    {
                        var pers =(Personal_record) Record(check_id, "DB");
                        
                        db.Wall_memes_connected.Add(new Relationship_with_memes(id,check_id,false, "Personal_record",false));
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
                case "change_album":
                    {
                        //TODO сейчас кто угодно может сменить альбом
                        int int_id = Convert.ToInt32(id);
                        var img = db.Images.First(x1 => x1.Id == int_id);
                        img.Albums = obg;
                        db.SaveChanges();
                        
                            //TODO если группа или человек исправить + очень грязно мб переписать albums в бд(засунуть в связь с мемами)
                            var al = db.Albums.Where(x1=>x1.Source_id==check_id);
                        var con = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == check_id && x1.Image).Select(x1=>x1.Something_one_id);
                        var ph = new List<Img>();
                        foreach (var i in con)
                        {
                            int int_id1 = Convert.ToInt32(i);
                            var mem1 = db.Memes.First(x1 => x1.Id == int_id1);

                            //var mem1= db.Memes.First(x1 => x1.Image_id == id&&x1.Source_id==check_id);
                            int int_id2 = Convert.ToInt32(mem1.Image_id);
                            var g = db.Images.First(x1 => x1.Id == int_id2);
                            ph.Add(g);

                        }
                        foreach (var i in al)
                            {
                            
                            try
                            {
                                var test = ph.First(x1 => x1.Albums == i.Name);
                            }
                            catch {
                                //var al = db.Albums.Where(x1 => x1.Source_id == check_id);
                                db.Albums.Remove(db.Albums.First(x1=>x1.Source_id==check_id&&x1.Name==i.Name));
                            }

                        }
                            
                    }
                    break;

            }
            var mem = Memes(id);
            //string id_image, string album_name,string from_id,string from_type,bool main_img=false
            if (from== "Record_photo_page")
                return RedirectToAction("Record_photo_page", "Home", new {  });
            //return RedirectToAction("Memes_partial","Home",new { id_mem=id });
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
        //[ChildActionOnly]
        public ActionResult One_comments_partial(string from, string from_id, string id_mem,string size)
        {
            //Comment_record res = null;
            ViewBag.size = size;
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var list_ph = db.Mem_comment_connected.Where(x1 => x1.Something_one_id == id_mem && x1.What_one == "Comment").ToList();
                int int_id = Convert.ToInt32(id_mem);
                var com = new Comment_record(db.Comments.First(x1 => x1.Id == int_id)) { Images = new List<Memes_record>()
        };
                com.Source_person = (Person_short)Record(com.db.Person_id, "Person_short");
                try
                {
                    int int_id_st = Convert.ToInt32(com.db.Stikers_id);
                    com.Stiker = db.Stikers.First(x1 => x1.Id == int_id_st);
                }
                catch { }
                foreach (var i2 in list_ph)
                {
                    com.Images.Add(Memes(i2.Something_two_id, "full"));
                }
            try
            {
                db.Liked_connected.First(x1=>x1.Something_one_id==id_mem&&x1.Something_two_id== check_id && x1.What_one == "Comment");
                ViewBag.like = true;
            }
            catch {
                ViewBag.like = false;
            }
            
            
            return PartialView(com);

        }

        [ChildActionOnly]
        public ActionResult Comments_partial(string from, string from_id, string id_mem, string size)
        {
            //TODO проверять на лайк и тд
            ViewBag.from = from;
            ViewBag.from_id = from_id;
            ViewBag.size = size;
            //ViewBag.id_image = id_mem;
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var list_com=db.Mem_comment_connected.Where(x1=>x1.Something_one_id==id_mem&&x1.What_one=="Memes").Select(x1=>x1.Something_two_id).ToList();
            
            return PartialView(list_com);
        }


        [HttpPost]
        public ActionResult Change_left_menu(string edit_text)
        {
            string Base_Menu_left = "Моя Cтраница,Personal_record,Новости,News,Сообщения,Messages,Группы,Groups_personal,Друзья,Friends,Фотографии,Albums,Музыка,Music,Видео,Video";

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

         

        public ActionResult Add_new_group_ajax()
        {
            
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
                res = new Person_info_short((Personal_record)pers);
            }
            
            ViewBag.Open = open;
            ViewBag.Id = id;
 
            return PartialView(res);
        }

        
        public ActionResult Follow_ajax(string from,string id ,bool click=false)
        {
            
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
                        var p= db.Friends_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id&& !x1.Person);
                        if (p.Admin_group)
                        {
                            click = false;
                        }
                        tmp = p.Something_two_id;
                    }
                    catch
                    {
                        try
                        {
                            tmp = db.Followers_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id).Something_two_id;
                        }
                        catch
                        {
                           
                        }
                    }

                    if (tmp == null)
                        throw new Exception();
                    if (click)
                    {
                        //нужно отписать от группы

                        try
                        {
                            db.Groups_connected.Remove(db.Groups_connected.First(x1 => x1.Something_two_id == check_id && x1.Something_one_id == id));

                        }
                        catch { }
                        //не нужно вроде

                        try
                        {
                            db.Friends_connected.Remove(db.Friends_connected.First(x1 => x1.Something_two_id == check_id && x1.Something_one_id == id&& !x1.Person));

                        }
                        catch { }
                        try
                        {
                            db.Followers_connected.Remove(db.Followers_connected.First(x1 => x1.Something_two_id == check_id && x1.Something_one_id == id));

                        }
                        catch { }
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

                        db.Friends_connected.Add(new Relationship_with_admin_group(id, check_id,false, false));
                        //((Group_record)pers).db.;
                        db.Groups_connected.Add(new Relationship_string_string_Groups_connected(id,check_id,true));
                        ((Personal_record)pers_peop).db.Groups_count += 1;

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
               
                    int flag_srch = 0;
                    try
                    {
                        var tmp = db.Followers_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id).Something_two_id;
                        flag_srch = 1;
                    }
                    catch {
                        
                            try
                            {
                                var tmp = db.Friends_connected.First(x1 => (((x1.Something_one_id == id && x1.Something_two_id == check_id) || (x1.Something_one_id == check_id && x1.Something_two_id == id))&& x1.Person)).Something_two_id;
                                flag_srch = 2;
                            }
                            catch { }
   
                    }
                    
                    
                if (flag_srch== 0)
                        throw new Exception();
                    if (click)
                    {
                        //нужно отписать от человека

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
                                    db.Friends_connected.Remove(db.Friends_connected.First(x1 => ((x1.Something_two_id == check_id && x1.Something_one_id == id)||(x1.Something_two_id ==id  && x1.Something_one_id == check_id)&& x1.Person)));
                                    db.Followers_connected.Add(new Relationship_string_string_Followers_connected(check_id, id, true) { Ignored = true });

                                    ((Personal_record)pers_peop).db.Friends_count -= 1;
                                }
                                catch { }
                                break;
                           
                        }


                        db.SaveChanges();

                        ViewBag.follow = false;
                        ViewBag.message = "Добавить в друзья";
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
                           
                        }
                        if (flag_srch != 0)
                        {
                            db.Friends_connected.Add(new Relationship_with_admin_group(id, check_id,true));
                            {
                                ((Personal_record)pers).db.Friends_count += 1;
                                ((Personal_record)pers_peop).db.Friends_count += 1;
                            }
                            //if (flag_srch == 1)
                                db.Followers_connected.Remove(db.Followers_connected.First(x1 => x1.Something_one_id == check_id && x1.Something_two_id == id));
                            
                        }

                        else
                        {
                            db.Followers_connected.Add(new Relationship_string_string_Followers_connected(id, check_id,true));
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
        public ActionResult Edit_group_record_info_load_ajax(string id,string obg)
        {
            ViewBag.click = obg;
            ViewBag.group_id = id;
            if (string.IsNullOrEmpty(obg))
                ViewBag.click = "Основное";

            ViewBag.list_menu = new string[] { "Основное", "Списки", "Приватность" };
            
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pers = Group(id, "DB");//TODO загрузка в зависимости от требований

            return PartialView(((Group_record)pers).db);
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
            ViewBag.save_changes = true;
            if (string.IsNullOrEmpty(click))
                ViewBag.click = "Основное";

            ViewBag.list_menu = new string[] { "Основное", "Контакты", "Интересы", "Образование", "Карьера", "Военная служба", "Жизненная позиция" };



            return PartialView("Edit_personal_record_info_load_ajax", pers.db);
        }

        
        public ActionResult Change_group_rec(Group a, string click,string id)
        {
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //var pers = db_users.Users.First(x1=>x1.Id==check_id);
            //TODO проверять валидацией все
            var pers = (Group_record)Group(id, "Group_record");

            switch (click)
            {
                case "Основное":
                    {
                        if(!string.IsNullOrEmpty(a.Name))
                        {
                            try
                            {
                                db.Groups.First(x1=>x1.Name==a.Name);
                             }
                            catch
                            {
                                pers.db.Name = a.Name;
                            }
                        }
                        pers.db.Add_memes_private = a.Add_memes_private;
                        pers.db.Open_group = a.Open_group;
                        pers.db.Status = a.Status;
                        db.SaveChanges();
                        break;
                    }
                case "Списки":
                    {
                        

                        break;
                    }
                case "Приватность":
                    {


                        break;
                    }
            }

            ViewBag.click = click;
            ViewBag.save_changes = true;
            if (string.IsNullOrEmpty(click))
                ViewBag.click = "Основное";

            ViewBag.list_menu = new string[] { "Основное", "Списки", "Приватность" };



            return PartialView("Edit_group_record_info_load_ajax", pers.db);
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
        public ActionResult Add_new_comment(HttpPostedFileBase[] uploadImage, string comment_text, string id_memes,string Stikers_id)
        {
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var photo_list = Get_photo_post(Get_photo_post(uploadImage), "");
            var comment = new Comment(comment_text, check_id) { Memes_id = id_memes, Stikers_id = Stikers_id };
            db.Comments.Add(comment);
            db.SaveChanges();
            if (photo_list != null)
            {
                foreach (var i in photo_list)
                {
                    
                    db.Images.Add(i);
                    db.SaveChanges();
                    var mem = new Memes(check_id) { Source_type = "Personal_record", Image_id = i.Id.ToString() };
                    db.Memes.Add(mem);
                    db.SaveChanges();
                    db.Mem_comment_connected.Add(new Relationship_mem_comment(comment.Id.ToString(), mem.Id.ToString(), "Comment"));
                    //db.Images_mem_connected.Add(new Relationship_with_images(i.Id.ToString(), mem.Id.ToString()));
                    db.SaveChanges();
                }


            }
            db.Mem_comment_connected.Add(new Relationship_mem_comment(id_memes, comment.Id.ToString(), "Memes"));
            db.SaveChanges();
            return RedirectToAction("Personal_record", "Home", new { id = check_id });
            //return View("Personal_record",Record(check_id, "Personal_record"));
        }
         
        [HttpPost]
        public ActionResult Add_new_message(HttpPostedFileBase[] uploadImage, string message_text, string Id_dialog)
        {
           var dialog= db.Messages_obg.First(x1=>x1.Id.ToString()== Id_dialog);
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pers = (Personal_record)Record(check_id, "DB");

            try
            {
                var relation = db.Messages_dialog_person_connected.First(x1 => x1.Something_one_id == Id_dialog && x1.Something_two_id == check_id);
               var photo_list = Get_photo_post(Get_photo_post(uploadImage),"");

                var mes = new Message(message_text, check_id);
                
                db.Messages.Add(mes);
                db.SaveChanges();
                db.Messages_one_dialog_connected.Add(new Relationship_string_string_Messages_one_dialog_connected(dialog.Id.ToString(), mes.Id.ToString()));
                //dialog.Messages_id += mes.Id + ",";
                db.SaveChanges();

                if (photo_list != null)
                {
                    foreach (var i in photo_list)
                    {
                        db.Images.Add(i);
                        db.SaveChanges();
                        var mem = new Memes(check_id) { Source_type = "Personal_record", Image_id = i.Id.ToString() };
                        db.Memes.Add(mem);
                        db.SaveChanges();
                        db.Message_mem_connected.Add(new Relationship_string_string_mes_mem_connected(mes.Id.ToString(),mem.Id.ToString()));
                        
                        
                    }


                }

                //не уверен что нужно обращаться
                var res = Message_person_block(id: Id_dialog, person_id: null, bool_fullness: "Messages_one_dialog");

                ViewBag.Id_dialog = res.db.Id.ToString();
                return View("Messages_one_dialog", res);
            }
            catch { }

            return View();

        }

         
        [HttpPost]
        public ActionResult Add_new_group(Group a)
        {
            

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
                    db.Friends_connected.Add(new Relationship_with_admin_group(a.Id.ToString(),check_id,false,true));
                    db.Groups_connected.Add(new Relationship_string_string_Groups_connected(a.Id.ToString(), check_id,true));
                    pers.db.Groups_count += 1;
                    
                    db.SaveChanges();

                    ViewBag.add_success = "Группа создана";
                    ViewBag.id=a.Id;
                }

            }
            
            
            

            return PartialView("Add_new_group_ajax");
        }
        [HttpPost]
        public ActionResult Add_new_image(HttpPostedFileBase[] uploadImage, string for_what,string from,string Album_name="",string id="")
        {
            Album_name = Album_name == "Все Альбомы" ?"":Album_name;
            IPage_view res_page = null;
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var photo_list = Get_photo_post(Get_photo_post(uploadImage), Album_name);
            
            //ViewBag.My_page = false;
            if (from == "Personal_record")
            {
                //string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                try
                {
                    var test = db.Albums.First(x1 => x1.Name == Album_name && x1.Source_id == check_id);
                }
                catch {
                    db.Albums.Add(new Album(Album_name,check_id,true)); }
                res_page  = Record(check_id, "DB");
                //ViewBag.My_page = true;
                if (photo_list != null)
                {
                    foreach (var i in photo_list)
                    {
                        i.Main = for_what == "main_img" ? true : false;
                        db.Images.Add(i);
                        db.SaveChanges();
                        var mem = new Memes(check_id) { Source_type = "Personal_record", Image_id = i.Id.ToString() };
                        db.Memes.Add(mem);
                        db.SaveChanges();
                        
                        db.Wall_memes_connected.Add(new Relationship_with_memes(mem.Id.ToString(),check_id,false, "Personal_record",true));
                    }


                }
               
                
                
                ((Personal_record)res_page).db.Images_count += 1;
                
                db.SaveChanges();

                //res_page = Record(check_id, "Personal_record");
                return RedirectToAction("Personal_record", "Home", new { id=((Personal_record)res_page).db.Id });
                //return View("Personal_record", res_page);
            }
            if (from == "Group_record")
            {
                if (!string.IsNullOrEmpty(id))
                {

                    //string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    try
                    {
                        var test = db.Albums.First(x1 => x1.Name == Album_name && x1.Source_id == id&& x1.Person_bool==false);
                    }
                    catch
                    {
                        db.Albums.Add(new Album(Album_name, id, false));
                    }
                    //res_page = Group(id, "DB");
                    //ViewBag.My_page = true;
                    if (photo_list != null)
                    {
                        foreach (var i in photo_list)
                        {
                            i.Main = for_what == "main_img" ? true : false;
                            db.Images.Add(i);
                            db.SaveChanges();
                            var mem = new Memes(id) { Source_type = "Group_record", Image_id = i.Id.ToString() };
                            db.Memes.Add(mem);
                            db.SaveChanges();

                            db.Wall_memes_connected.Add(new Relationship_with_memes(mem.Id.ToString(), id, null, "Group_record", true));
                        }


                    }
                
                    db.SaveChanges();
                    //res_page = Group(id, "Group_record");
                    return RedirectToAction("Group_record", "Home", new { id = id });
                    //return View("Group_record", res_page);
                }

            }
            //не нужно просто что бы было
            //return RedirectToAction("Personal_record", "Home", new { ((Personal_record)res_page).db.Id });
            res_page = Record(check_id, "Personal_record");
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
                            res_db = new Memes(id)
                            {
                                Description = Description_mem
                            };
                            var photo_list = Get_photo_post(Get_photo_post(uploadImage),"");
                            
                            
                                
                            
                            res_db.Source_type = "Personal_record";
                            db.Memes.Add(res_db);
                            db.SaveChanges();
                            

                            foreach (var i in photo_list)
                                {
                                    
                                    
                                    db.Images.Add(i);
                                    db.SaveChanges();
                                var mem = new Memes() {Source_type= "Personal_record",Source_id=check_id,Image_id=i.Id.ToString() };
                                db.Memes.Add(mem);
                                db.SaveChanges();
                                db.Images_mem_connected.Add(new Relationship_with_images(res_db.Id.ToString(), mem.Id.ToString()));
                                db.SaveChanges();

                            }

                            db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), pers.db.Id.ToString(), false, "Personal_record",false));
                            
                            pers.db.Wall_count += 1;

                            db.SaveChanges();
                            //раскидать друзьям подписчикам
                            {
                                // var mass_fr = pers.db.Friends_id.Split(',');
                                //TODO еще и всем фоловерам отправлять
                                {
                                    var mass_fr = db.Friends_connected.Where(x1 => (x1.Something_one_id == check_id || x1.Something_two_id == check_id)&& x1.Person).ToList();
                                    foreach (var i in mass_fr)
                                    {
                                        if (i.Something_one_id == check_id)
                                        {
                                            db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), i.Something_two_id, true, "Personal_record", false));
                                        }
                                        else
                                        {
                                            db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), i.Something_one_id, true, "Personal_record", false));
                                        }


                                    }
                                }
                                {
                                    var mass_fr = db.Followers_connected.Where(x1 => x1.Something_one_id == check_id).ToList();
                                    foreach (var i in mass_fr)
                                    {

                                        db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), i.Something_two_id, true, "Personal_record", false));

                                    }
                                }

                                db.SaveChanges();
                            }

                            res_page = Record(id, "Personal_record"); ;
                            //res.db = res_db;
                        }
                        return RedirectToAction("Personal_record", "Home", new { id = id });
                    }

                    break;


                case "group"://добавление записи со страницы группы +проверки
                    //TODO проверять можно ли добавить пост туда куда требуют и в списке админов то  ViewBag.My_page = true;
                    {
                        var pers = (Group_record)Group(id, "DB");
                        res_db = new Memes( id)
                        {
                            Description = Description_mem
                        };
                        var photo_list = Get_photo_post(Get_photo_post(uploadImage),"");
                        res_db.Source_type = "Group_record";
                        db.Memes.Add(res_db);
                        db.SaveChanges();

                        foreach (var i in photo_list)
                            {
                            db.Images.Add(i);
                            db.SaveChanges();
                            var mem = new Memes() { Source_type = "Group_record", Source_id = id, Image_id = i.Id.ToString() };
                            db.Memes.Add(mem);
                            db.SaveChanges();
                            db.Images_mem_connected.Add(new Relationship_with_images(res_db.Id.ToString(), mem.Id.ToString()));
                            db.SaveChanges();   
                            }
                        
                        
                        

                        db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), pers.db.Id.ToString(), false, "Group_record", false));

                        pers.db.Wall_count += 1;

                        db.SaveChanges();

                        //раскидать  подписчикам группы
                        {
                            var mass_fr = db.Friends_connected.Where(x1 => x1.Something_one_id == id&&!x1.Person).ToList();
                            foreach (var i in mass_fr)
                            {
                                
                                    db.Wall_memes_connected.Add(new Relationship_with_memes(res_db.Id.ToString(), i.Something_two_id, true, "Personal_record", false));
                                


                            }
                        }
                        db.SaveChanges();
                       

                        res_page = (IPage_view)pers;
                        //res.db = res_db;
                        return RedirectToAction("Group_record", "Home", new { id = id });
                        //return View("Group_record",(Group_record)Group(id));

                        //сейчас
                        //return View("Group_record", res_page);
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
                    var tmp = db.Messages_dialog_person_connected.Where(x1 => x1.Something_two_id == check_id).ToList();
                    if (person_id == check_id)
                    {
                        
                        
                        for(int i = 0, stop=1; i < tmp.Count()&&stop==1; ++i)
                        {
                            string str = tmp[i].Something_one_id;
                            var tmp2=db.Messages_dialog_person_connected.Where(x1=>x1.Something_one_id== str).Count();
                            if (tmp2 == 1)
                            {
                                stop = 0;
                                id = tmp[i].Something_one_id;
                                int id_int = Convert.ToInt32(id);
                                res = new Message_obg_record(db.Messages_obg.First(x1 => x1.Id == id_int));
                            }
                                
                        }
                        if(res==null)
                            throw new Exception();

                    }
                    else
                    {
                        for (int i = 0, stop = 1; i < tmp.Count() && stop == 1; ++i)
                        {
                            string str = tmp[i].Something_one_id;
                            var tmp2 = db.Messages_dialog_person_connected.Where(x1 => x1.Something_one_id == str).ToList();
                            try
                            {
                                var tmp3=tmp2.First(x1 => x1.Something_two_id == person_id);
                                if (tmp3 != null)
                                {
                                    stop = 0;
                                    id = tmp[i].Something_one_id;
                                    int id_int = Convert.ToInt32(id);
                                    res = new Message_obg_record(db.Messages_obg.First(x1 => x1.Id == id_int));
                                }
                            }
                            catch { }
                            
                        }
                        if (res == null)
                            throw new Exception();
                    }
                }
                catch
                {
                    var t = new Message_obg() ;
                    db.Messages_obg.Add(t);
                    db.SaveChanges();
                    id = t.Id.ToString();
                    db.Messages_dialog_person_connected.Add(new Relationship_string_string_Messages_dialog_person_connected( t.Id.ToString(), check_id));
                    if(person_id != check_id&&person_id!=null)
                    db.Messages_dialog_person_connected.Add(new Relationship_string_string_Messages_dialog_person_connected(t.Id.ToString(), person_id));
                    db.SaveChanges();
                    res = new Message_obg_record(t);
                }
                
            }

            try
            {
                string int_id= res.db.Id.ToString();
                var pers_list = db.Messages_dialog_person_connected.Where(x1 => x1.Something_one_id == int_id).ToList();

                foreach(var i in pers_list)
                {
                    res.Person.Add((Person_short)Record(i.Something_two_id, "Person_short"));
                }

            }
            catch
            {

            }
            if (bool_fullness== "Messages")
            {

                try
                {
                   
                    var str = db.Messages_one_dialog_connected.Where(x1 => x1.Something_one_id == id).ToList().Last();

                 
                    int tmp =Convert.ToInt32( str.Something_two_id);
                    var a = db.Messages.First(x1 => x1.Id == tmp);
                        res.Messages.Add(new Message_record(a) { Source_person =(Person_short)Record(a.Person_id, "Person_short") });
                    

                }
                catch
                {

                }
            }
            if (bool_fullness == "Messages_one_dialog")
            {

                try
                {
                    
                    var str = db.Messages_one_dialog_connected.Where(x1 => x1.Something_one_id == id).ToList();
                    str.Reverse();
                    str = str.Take(30).ToList();
                    str.Reverse();
                    
                    
                    foreach (var i in str)
                    {
                        int tmp = Convert.ToInt32(i.Something_two_id);
                        var a = db.Messages.First(x1 => x1.Id == tmp);
                        if (a.New)
                        {
                            if (a.Person_id != check_id)
                            {
                                a.New = false;
                                db.SaveChanges();
                            }

                        }
                        Message_record rec = new Message_record(a)
                        {
                            Source_person = (Person_short)Record(a.Person_id, "Person_short")
                        };
                       
                        string str_id = a.Id.ToString();
                        var mes_rel = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == str_id&&x1.Who== "Message").ToList();
                       
                        foreach(var i1 in mes_rel)
                        {
                           
                            var mem = Memes(i1.Something_one_id);
                            rec.Images.Add(mem);
                        }
                        
                        
                        res.Messages.Add(rec);
                    }
                    
                }
                catch
                {

                }
            }
                

            return res;

        }
        
        public Memes_record Memes(string id, string bool_fullness = "full")
        {
            
            var not_res = db.Memes.First(x1 => x1.Id.ToString() == id);

            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //string id_mem = id;
            Memes_record res = new Memes_record(not_res);
            if (not_res.Image_id == null)
            {
                //обычный мем
                try
                {

                    var mass = db.Images_mem_connected.Where(x1 => x1.Something_one_id == id).ToList();
                    //if(mass)
                    foreach (var i in mass)
                    {
                        res.Images.Add(Memes(i.Something_two_id));

                    }

                }
                catch
                {

                }
            }
            else
            {
                //мем=картинка
                
                int id_img_mem = Convert.ToInt32(id);
                var id_img = Convert.ToInt32(db.Memes.First(x1 => x1.Id == id_img_mem && x1.Image_id != null).Image_id);
                var img = db.Images.First(x1 => x1.Id == id_img);
                res.Image = img;

            }
            //short source
            if (res.db.Source_type == "Personal_record")
            {
                res.Person_source = (Person_short)Record(res.db.Source_id, "Person_short");
            }
            else if(res.db.Source_type == "Group_record")
            {
                res.Group_source = (Group_short)Group(res.db.Source_id, "Group_short");
            }

            
            


            ViewBag.like = false;
            ViewBag.repost = false;
            var mass_liked = db.Liked_connected.Where(x1=>x1.Something_one_id== id && x1.What_one == "Memes").ToList();
            ViewBag.Count_like = mass_liked.Count;
            //список лайкнувших +репостнувших также доделать
            //ViewBag.Like_list = mass_liked.Take(5);
            //проверка на лайк
            foreach (var i in mass_liked)
            {
                if (!string.IsNullOrEmpty(i.Something_two_id))
                {
                    if (i.Something_two_id == check_id)
                        ViewBag.like = true;
                    
                }
            }

            
            var mass_repost = db.Repost_connected.Where(x1 => x1.Something_one_id== id).ToList();
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
                    //заполнение подписчиков
                    var str = db.Friends_connected.Where(x1 => x1.Something_one_id== id&&!x1.Person).ToList();
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
                    //заполнение админов
                    var str = db.Friends_connected.Where(x1 => x1.Something_one_id == id&&x1.Admin_group&&!x1.Person).ToList();
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
                    //альбомы
                    //альбомы на странице
                    ((Group_record)res).Albums = Albums_function(id, count: 2, from_type: "Group_record");
                }
                catch
            {

            }

            try
                {
                    //заполнение картинок
                    //var ggg = db.Wall_memes_connected.ToList();
                    var str = db.Wall_memes_connected.Where(x1=>x1.Something_two_id==id&&x1.Image&&x1.Who== "Group_record").ToList();

                    try
                    {
                        //main img 
                        foreach(var i in str)
                        {
                            int int_id1 = Convert.ToInt32(i.Something_one_id);
                            var mem=db.Memes.First(x1 => x1.Id == int_id1&&x1.Source_id!=null);
                            int int_id2= Convert.ToInt32(mem.Image_id);
                            var img=db.Images.First(x1 => x1.Id == int_id2);
                            if (img.Main==true)
                            {
                                ((Group_record)res).Main_images.Add(Memes(i.Something_one_id));
                            }
                            

                        }
                    }
                    catch { }

                    str.Reverse();
                    foreach (var i in str.Take(5))
                    {
                        ((Group_record)res).Images.Add(Memes(i.Something_one_id));
                    }

                }
                catch
                {

                }
                /*try
                {

                    var str = db.Images_connected.Where(x1 => x1.Something_one_id == id&&x1.Main).ToList().Last();
                    //var str = db.Images_connected.Where(x1 => x1.Something_one_id == id && x1.Main).Reverse().First();
                    int int_id_img = Convert.ToInt32(str.Something_two_id);
                    ((Group_record)res).Main_images.Add(db.Images.First(x1 => x1.Id == int_id_img));
                    

                }
                catch
                {

                }*/
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
            if (bool_fullness == "groups_friends")
            {
                res = new Group_record(not_res);


                var lst = db.Groups_connected.Where(x1 => x1.Something_two_id == id && !x1.Person).ToList();
                //ViewBag.Count_followers = lst.Count;
                foreach (var i in lst)
                {
                    if (!string.IsNullOrEmpty(i.Something_one_id))
                    {
                        
                        ((Group_record)res).Groups.Add((Group_short)Record(i.Something_one_id, "Person_short"));
                    }

                }
            }
            if (bool_fullness == "Followers")
            {
                res = new Group_record(not_res);


                var lst = db.Friends_connected.Where(x1=>x1.Something_one_id==id&&!x1.Person).ToList();
                ViewBag.Count_followers = lst.Count;
                foreach (var i in lst)
                {
                    if (!string.IsNullOrEmpty(i.Something_two_id))
                    {
                        ((Group_record)res).Followers.Add((Person_short)Record(i.Something_two_id, "Person_short"));
                    }
                    
                }
            }
            if (bool_fullness == "Admins")
            {
                res = new Group_record(not_res);

                var lst = db.Friends_connected.Where(x1 => x1.Something_one_id == id && x1.Admin_group&&!x1.Person).ToList();
                ViewBag.Count_followers = lst.Count;
                foreach (var i in lst)
                {
                    if (!string.IsNullOrEmpty(i.Something_two_id))
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
                var count_fl = db.Friends_connected.Where(x1 => x1.Something_one_id == id&&!x1.Person).Count();
                
                res = new Group_short(not_res, count_fl,null);
                var main_ph=db.Wall_memes_connected.Where(x1=>x1.Something_two_id== id&& x1.Who== "Group_record"&&x1.News==null&&x1.Image);
                main_ph.Reverse();
                
                try
                {
                    foreach (var i in main_ph.ToList())
                    {
                        int int_id1 = Convert.ToInt32(i.Something_one_id);
                        var mem = db.Memes.First(x1 => x1.Id == int_id1);
                        int int_id_img = Convert.ToInt32(mem.Image_id);

                        var img = db.Images.First(x1 => x1.Id == int_id_img);
                        if (img.Main == true)
                        {
                            ((Group_short)res).Image = img.bytes;
                            throw new Exception();
                        }
                    }
                }
                catch { }
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


            //TODO сравнивать id и если одинаковые то и меню слева отправлять иначе нет
           
            Personal_record res = null;
            if (id != null)//убрать условие? проверять есть ли такой человек в базе
            {
                var res_ap = db.Users.First(x1 => x1.Id == id);
                res = new Personal_record(res_ap);
                
                
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
                            
                            var img_list = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == id&&x1.Image).ToList();
                            //img_list.Reverse();

                            foreach (var i in img_list)
                            {
                                int int_id = Convert.ToInt32(i.Something_one_id);
                                var mem=db.Memes.First(x1 => x1.Id == int_id);
                                if (mem.Image_id != null)
                                {
                                    int int_id1= Convert.ToInt32(mem.Image_id);
                                    var img=db.Images.First(x1 => x1.Id == int_id1);
                                    if (img.Main==true)
                                    {
                                        a_b_tmp = img.bytes;
                                        // a_b_tmp = db.Images.Add(Memes(i.Something_one_id));
                                        throw new Exception();
                                    }
                                }
                            }
                            
                        }
                        catch
                        {

                        }
                            return new Person_short(res.db.Id, a_b_tmp, res.db.Name);
                    }
                        
                        if (bool_fullness == "Personal_record")
                    {
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        try
                        {

                            var img_list = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == id && x1.Image).ToList();
                            img_list.Reverse();
                            //var img = db.Images_connected.Where(x1 => x1.Something_two_id == id && x1.Main).Reverse().First();
                            foreach (var i in img_list)
                            {
                                int int_id = Convert.ToInt32(i.Something_one_id);
                                var mem = db.Memes.First(x1 => x1.Id == int_id);
                                if (mem.Image_id != null)
                                {
                                    int int_id1 = Convert.ToInt32(mem.Image_id);
                                    var img = db.Images.First(x1 => x1.Id == int_id1);
                                    if (img.Main == true)
                                    {
                                        res.Main_images.Add(Memes(i.Something_one_id));
                                        //a_b_tmp = img.bytes;
                                        // a_b_tmp = db.Images.Add(Memes(i.Something_one_id));
                                    }
                                }
                            }


                            
                        }
                        catch
                        {

                        }
                        try
                        {
                            //ФОТО, потом под 1 засунуть мб
                            var img_list = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == id && x1.Image).ToList();
                            img_list.Reverse();
                            //var img = db.Images_connected.Where(x1 => x1.Something_two_id == id && x1.Main).Reverse().First();
                            foreach (var i in img_list)
                            {
                                    res.Images.Add(Memes(i.Something_one_id));
                            }

                        }
                        catch
                        {

                        }
                        try
                        {
                            //заполнение групп
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
                            var lst = db.Friends_connected.Where(x1 => (x1.Something_one_id == id|| x1.Something_two_id == id)&&x1.Person).Take(6).ToList();

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
                        try
                        {
                            //альбомы на странице
                            res.Albums = Albums_function(id,count:2);
                        }
                        catch { }

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
                        var lst = db.Friends_connected.Where(x1 => (x1.Something_one_id == id || x1.Something_two_id == id)&&x1.Person).ToList();

                        foreach (var i in lst)
                        {
                            if (i.Something_one_id != id)
                                res.Friends.Add((Person_short)Record(i.Something_one_id, "Person_short"));
                            else
                                res.Friends.Add((Person_short)Record(i.Something_two_id, "Person_short"));
                        }

                    }
                    if (bool_fullness == "Followers_ignore")//
                    {
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        var lst = db.Followers_connected.Where(x1 => x1.Something_one_id == id&&x1.Ignored).ToList();

                            foreach (var i in lst)
                            {

                                res.Followers_ignore.Add((Person_short)Record(i.Something_two_id, "Person_short"));

                            }
                        
                        
 

                    }
                    
                    if (bool_fullness == "Followers")//
                    {
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        var lst = db.Followers_connected.Where(x1 => x1.Something_one_id == id && !x1.Ignored).ToList();
                        //сейчас
                        var tmp = db.Followers_connected.ToList();

                        foreach (var i in lst)
                        {

                            res.Followers.Add((Person_short)Record(i.Something_two_id, "Person_short"));

                        }
                    }
                    if (bool_fullness == "Send_follow")//
                    {
                        ViewBag.My_page = false;
                        if (id == check_id)
                            ViewBag.My_page = true;
                        var lst = db.Followers_connected.Where(x1 => x1.Something_two_id == id).ToList();
                        
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

                }
                catch
                {

                }
    
            }




            return res;
        }
        public List<Img> Get_photo_post(IEnumerable<byte[]> Images,string Album_name)
        {
            List<Img> res = new List<Img>();
            foreach(var i in Images)
            {
                res.Add(new Img(i,null) { Albums= Album_name });
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

        void Like_something(string id,string What)
        {
            //What== Memes Comment
            string check_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            try
            {
                var mass_liked = db.Liked_connected.Where(x1 => x1.Something_one_id == id && x1.What_one == What&&x1.Something_two_id==check_id).ToList();
                db.Liked_connected.Remove(db.Liked_connected.First(x1 => x1.Something_one_id == id && x1.Something_two_id == check_id && x1.What_one == What));
                if(What== "Comment")
                {
                    int int_id = Convert.ToInt32(id);
                    var com = db.Comments.First(x1=>x1.Id== int_id);
                    com.Count_like -= 1;
                    db.SaveChanges();
                }
                ViewBag.like = false;
                ViewBag.count_like -= 1;
            }
            catch {
                db.Liked_connected.Add(new Relationship_string_string_Liked_connected(id, check_id) { What_one = What});
                if (What == "Comment")
                {
                    int int_id = Convert.ToInt32(id);
                    var com = db.Comments.First(x1 => x1.Id == int_id);
                    com.Count_like += 1;
                    db.SaveChanges();
                }
                ViewBag.like = true;
                ViewBag.count_like += 1;
            }
            

            /*
            var mass_liked = db.Liked_connected.Where(x1 => x1.Something_one_id == id && x1.What == What).ToList();
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
            }*/
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

                        var tmp = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == id && x1.News==false).ToList();
                        tmp.Reverse();

                        foreach (var i in tmp.Skip(start).Take(10))
                        {
                            not_res.Add(i.Something_one_id);
                        }

                    }
                    catch { }
                    res = not_res;

                    ViewBag.Count = start + res.Count();

                    return res;
                    break;

                case "Group_record":


                    try
                    {

                        var tmp = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == id && x1.News == false).ToList();
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

                    try
                    {

                        var tmp = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == check_id && x1.News==true).ToList();
                        tmp.Reverse();
                        foreach (var i in tmp.Skip(start).Take(10))
                        {
                            not_res.Add(i.Something_one_id);
                        }

                    }
                    catch { }
                    res = not_res;

                    ViewBag.Count = start + res.Count();
                    return res;
                    break;
            }
        }
        List<Memes_record> Albums_function(string id, string album_name = "", string from_type = "Personal_record",int count=-1)
        {
            id = string.IsNullOrEmpty(id) ? System.Web.HttpContext.Current.User.Identity.GetUserId() : id;
            var list_photo_mem_all_id = db.Wall_memes_connected.Where(x1 => x1.Something_two_id == id && x1.Image && x1.Who == from_type).ToList();
            //var albums =db.Albums.Where(x1=>x1.Source_id== id&&x1.Person_bool==(from_type== "Personal_record" ? true:false)).ToList();
            var res = new List<Memes_record>();


            foreach (var i in list_photo_mem_all_id)
            {
                int int_id = Convert.ToInt32(i.Something_one_id);
                var mem_photo = db.Memes.First(x1 => x1.Id == int_id);
                int int_id_ph = Convert.ToInt32(mem_photo.Image_id);
                var ph = db.Images.First(x1 => x1.Id == int_id_ph);
                try
                {
                    res.First(x1 => x1.Image.Albums == ph.Albums);
                }
                catch
                {
                    res.Add(Memes(i.Something_one_id));
                }

            }
            if (count > 0)
                return res.Take(count).ToList();
            else
                return res;
            
        }
        Memes Remove_memes(string id_mem,string pers_id)
        {

            


            
            int int_id = Convert.ToInt32(id_mem);
            var mem = db.Memes.First(x1=>x1.Id== int_id);
            if (!string.IsNullOrEmpty(mem.Image_id))
            {
                //если мем картинка удаление картинки
                int int_id_img = Convert.ToInt32(mem.Image_id);
                db.Images.Remove(db.Images.First(x1 => x1.Id == int_id_img));
            }
            try
            {
                // удаление лайков мема
                db.Liked_connected.RemoveRange(db.Liked_connected.Where(x1 => x1.Something_one_id == id_mem));

            }
            catch { }
            try
            {
                // удаление репостов
                db.Repost_connected.RemoveRange(db.Repost_connected.Where(x1 => x1.Something_one_id == id_mem));

            }
            catch { }
            try
            {
                // удаление связи со стенами
                db.Wall_memes_connected.RemoveRange(db.Wall_memes_connected.Where(x1 => x1.Something_one_id == id_mem));

            }
            catch { }
            try
            {
                // удаление комментов
                var comment_rel = db.Mem_comment_connected.RemoveRange(db.Mem_comment_connected.Where(x1 => x1.Something_one_id == id_mem && x1.What_one == "Memes"));

                foreach (var i in comment_rel)
                {
                    int int_id_tmp = Convert.ToInt32(i.Something_two_id);
                   var comment= db.Comments.Remove(db.Comments.First(x1=>x1.Id== int_id_tmp));
                    var com=db.Mem_comment_connected.Remove(db.Mem_comment_connected.First(x1 => x1.Something_one_id == i.Something_two_id && x1.What_one == "Comment"));
                    Remove_memes(com.Something_two_id, "");
                }
            }
            catch { }
            
            
            try
            {
                // удаление картинок
                db.Images_mem_connected.RemoveRange(db.Images_mem_connected.Where(x1 => x1.Something_one_id == id_mem));

            }
            catch { }

            db.SaveChanges();
            return mem;

        }

        }
}


