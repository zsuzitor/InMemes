using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Im.Models
{
    public class Group: IPage_view
    {

        //в IdentityUser есть  public int Id { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime Birthday { get; set; }
        // заявки 
        public string Followers_id { get; set; }//id не принятых(отвергнутых) друзей
        public List<Person_short> Followers { get; set; }
        //стена 
        public string Wall_id { get; set; }//id записей
        public List<Memes> Wall { get; set; }
        //фото //
        public string Images_id { get; set; }//id фото
        public List<byte[]> Images { get; set; }

        //авы
        public string Main_images_id { get; set; }//id фото
        public List<byte[]> Main_images { get; set; }
        //группы
        public string Groups_id { get; set; }
        public List<Group_short> Groups { get; set; }



        public Group(string a)
        {
            Id = -1;
            Name = a;
            Status = "";
            Birthday = DateTime.Now;
            Followers_id = "";
            Wall_id = "";
            Images_id = "";
            Main_images_id = "";
            Groups_id = "";
            Followers = new List<Person_short>();
            Wall = new List<Memes>();
            Images = new List<byte[]>();
            Main_images = new List<byte[]>();
            Groups = new List<Group_short>();
        }

    }
    public class Img
    {
        public string Name;
        public int Id { get; set; }
        public byte[] bytes;


        public Img(byte[] a)
        {
            Id = -1;
            bytes = a;
        }

    }

    public class Memes
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Source_id { get; set; }
        public byte[] Image { get; set; }
        public string Images_id { get; set; }//id фото
        public List<byte[]> Images { get; set; }
        public string Liked_id { get; set; }//id кто лайкнул
        public string Repost_id { get; set; }//id кто репостнул
        public DateTime Birthday { get; set; }


        public Memes(string Source,string Source_id)
        {
            Id = -1;
            this.Source = Source;
            this.Source_id = Source_id;
            Description = null;
            Image = null;
            Images_id = null;
            Images = null;
            Liked_id = "";
            Repost_id = "";
            Birthday = DateTime.Now;


        }
    }


    public class Message_obg
    {
        public int Id { get; set; }
        public string Person_id { get; set; }
        public List<Person_short> Person { get; set; }
        public string Messages_id { get; set; }//id
        public List<Message> Messages { get; set; }//TODO нет прикрепленных файлов к сообщению
        public byte[] Image { get; set; }
        public int New_message_count { get; set; }

        public Message_obg(string Person_id)
        {
            Id = -1;
            this.Person_id = Person_id;
            Person = new List<Person_short>();
            Messages_id = "";
            Messages = new List<Message>();
            Image = null;
            New_message_count = 0;
        }
    }

    public class Message
    {
        public int Id { get; set; }
        public string Message_text { get; set; }
        public string Person_id { get; set; }
        public byte[] Image { get; set; }

        public Message(string Message_text,string Person_id)
        {
            Id = -1;
            this.Message_text = Message_text;
            this.Person_id = Person_id;
            Image = null;

        }

    }










        public class Person_short
    {
        public int Id { get; set; }
        public string Person_id { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }

        public Person_short(string id, byte[] Image, string Name)
        {
            Id = -1;
            Person_id = id;
            this.Image = Image;
            this.Name = Name;
        }
    }
    public class Person_info_short
    {
        public int? Age { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string Description { get; set; }

        public Person_info_short()
        {

        }
        public Person_info_short(ApplicationUser a)
        {
            Age = a.Age;
            Country = a.Country;
            Town = a.Town;
            Street = a.Street;
            Description = a.Description;
        }
    }

        public class Group_short
    {
        public int Id { get; set; }
        public string Group_id_s { get; set; }
        public int Count_followers { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }

        public Group_short(string Group_id, int Count_followers, byte[] Image, string Name)
        {
            Id = -1;
            this.Group_id_s = Group_id;
            this.Count_followers = Count_followers;
            this.Image = Image;
            this.Name = Name;


        }
    }
    /*
    public abstract class rec
    {

        //в IdentityUser есть  public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime Birthday { get; set; }
        // заявки 
        public string Followers_str { get; set; }//id не принятых(отвергнутых) друзей
        public List<Person_short> Followers { get; set; }
        //стена 
        public string Wall_str { get; set; }//id записей
        public List<Person_short> Wall { get; set; }
        //фото //
        public string Images_str { get; set; }//id фото
        public List<byte[]> Images { get; set; }
        //группы
        public string Group_str { get; set; }
        public List<Group_short> Group { get; set; }
    }
    public class Person:rec
    {
        //TODO меню слева
        public int Age { get; set; }
        public string Login { get; set; }
        public string Nickname { get; set; }
        
        public string Surname{ get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        
       
        public bool Online { get; set; }
        //друзья
        public string Friends_str { get; set; }//id друзей
        public List<Person_short> Friends { get; set; }

        // подписчики
        public string Followers_ignore_str { get; set; }//id подписчиков
        public List<Person_short> Followers_ignore { get; set; }
        
        //сообщения 
        public string Message_str { get; set; }//id объектов
        public List<Person_short> Message { get; set; }
        
        //семья 
        public string Family_str { get; set; }
        public List<Person_short> Family { get; set; }
        
        
        //новости 
        public string news_str { get; set; }
        public List<memes> news { get; set; }
        

        //о себе 
        public string Description { get; set; }



        public string Black_list_str { get; set; }//id людей
        public List<Person_short> Black_list { get; set; }
        //настройки о том что слева и остальное

        public string Menu_left { get; set; }
    }
    */










    /*
    public class Group : rec
    {







    }
    */

    /*
    public class memes
    {
        public string Description { get; set; }
        public string Source { get; set; }
        public string Source_id { get; set; }
        public string Images_str { get; set; }//id фото
        public List<byte[]> Images { get; set; }
        public string Liked_id { get; set; }//id кто лайкнул
        public string Repost_id { get; set; }//id кто репостнул
        public DateTime Birthday { get; set; }

    }
    
    public class Message_obg
    {
        public int Id { get; set; }
        public int Person_id { get; set; }
        public Person_short Person { get; set; }
        public string Messages_str { get; set; }
        public List<string> Messages { get; set; }//TODO нет прикрепленных файлов к сообщению
        public byte[] Image { get; set; }
    }
    */
}