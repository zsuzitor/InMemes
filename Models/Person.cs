using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Im.Models
{

    public abstract class rec
    {
        public int Id { get; set; }
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



        public string Black_list { get; set; }//id людей
        //настройки о том что слева и остальное

        public string Menu_left { get; set; }
    }









    
    public class Person_short
    {
        public int Person_id { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
    }
    public class Group : rec
    {












    }
    public class Group_short
    {
        public int Group_id { get; set; }
        public int Count_followers { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
    }
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
        
        public int Person_id { get; set; }
        public Person_short Person { get; set; }
        public List<string> Messages { get; set; }//TODO нет прикрепленных файлов к сообщению
        public byte[] Image { get; set; }
    }
}