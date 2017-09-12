using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Im.Models
{
    public interface IPage_view
    {

    }
    public class Img
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] bytes { get; set; }

        public Img()
        {
            Id = -1;
            bytes = null;
        }
        public Img(byte[] a)
        {
            Id = -1;
            bytes = a;
        }

    }
    public class Group
    {

        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime Birthday { get; set; }
        // заявки 
        public string Followers_id { get; set; }//id не принятых(отвергнутых) друзей

        //стена 
        public string Wall_id { get; set; }//id записей

        //фото //
        public string Images_id { get; set; }//id фото


        //авы
        public string Main_images_id { get; set; }//id фото

        //группы
        public string Groups_id { get; set; }
        //админы
        public string Admins_id { get; set; }


        public Group()
        {
            Id = -1;
            Name = "";
            Admins_id = "";
            Status = "";
            Birthday = DateTime.Now;
            Followers_id = "";
            Wall_id = "";
            Images_id = "";
            Main_images_id = "";
            Groups_id = "";
        }

            public Group(string a,string id)
        {
            Id = -1;
            Name = a;
            Admins_id = id;
            Status = "";
            Birthday = DateTime.Now;
            Followers_id = "";
            Wall_id = "";
            Images_id = "";
            Main_images_id = "";
            Groups_id = "";

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

        public string Liked_id { get; set; }//id кто лайкнул
        public string Repost_id { get; set; }//id кто репостнул
        public DateTime Birthday { get; set; }

        public Memes()
        {
            Id = -1;
            this.Source = null;
            this.Source_id = null;
            Description = null;
            Image = null;
            Images_id = null;

            Liked_id = "";
            Repost_id = "";
            Birthday = DateTime.Now;
        }
        public Memes(string Source, string Source_id)
        {
            Id = -1;
            this.Source = Source;
            this.Source_id = Source_id;
            Description = null;
            Image = null;
            Images_id = null;

            Liked_id = "";
            Repost_id = "";
            Birthday = DateTime.Now;


        }
    }




    public class Message_obg
    {
        public int Id { get; set; }
        public string Person_id { get; set; }
        
        public string Messages_id { get; set; }//id
        
        public byte[] Image { get; set; }
        public int New_message_count { get; set; }

        public Message_obg()
        {
            Id = -1;
            this.Person_id = null;

            Messages_id = "";

            Image = null;
            New_message_count = 0;
        }
            public Message_obg(string Person_id)
        {
            Id = -1;
            this.Person_id = Person_id;
            
            Messages_id = "";
            
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
        public Message()
        {
            Id = -1;
            this.Message_text = null;
            this.Person_id = null;
            Image = null;
        }

            public Message(string Message_text, string Person_id)
        {
            Id = -1;
            this.Message_text = Message_text;
            this.Person_id = Person_id;
            Image = null;

        }

    }

    //------------------------------------------------------------------------------------------------------------------------


        public class Message_obg_record
    {
        Message_obg db;
        
        public List<Person_short> Person { get; set; }
       
        public List<Message> Messages { get; set; }//TODO нет прикрепленных файлов к сообщению
        public Message_obg_record()
        {
            db = null;

            Person = new List<Person_short>();

            Messages = new List<Message>();
        }

            public Message_obg_record(Message_obg a)
        {
            db = a;
            
            Person = new List<Person_short>();
            
            Messages = new List<Message>();
            
        }
    }

    



    public class Group_record : IPage_view
    {
        public Group db;

        // заявки 

        public List<Person_short> Followers { get; set; }
        //стена 

        public List<Memes_record> Wall { get; set; }
        //фото //

        public List<byte[]> Images { get; set; }

        //авы

        public List<byte[]> Main_images { get; set; }
        //группы

        public List<Group_short> Groups { get; set; }

        public Group_record(Group a)
        {
            db = a;
            Followers = new List<Person_short>();
            Wall = new List<Memes_record>();
            Images = new List<byte[]>();
            Main_images = new List<byte[]>();
            Groups = new List<Group_short>();
        }

    }

    public class Personal_record : IPage_view
    {
        public ApplicationUser db;
        // заявки 
        public List<Person_short> Followers { get; set; }
        //друзья
        public List<Person_short> Friends { get; set; }
        // подписчики
        public List<Person_short> Followers_ignore { get; set; }
        //семья 
        public List<Person_short> Family { get; set; }
        //сообщения 
        public List<Message_obg_record> Message { get; set; }
        //новости 
        public List<Memes_record> News { get; set; }
        //стена 
        public List<Memes_record> Wall { get; set; }
        //фото //
        public List<byte[]> Images { get; set; }
        //авы
        public List<byte[]> Main_images { get; set; }
        //группы
        public List<Group_short> Groups { get; set; }
        public List<Person_short> Black_list { get; set; }



        public Personal_record()
        {
            db = null;

             Followers = new List<Person_short>();
            Friends = new List<Person_short>();
            Followers_ignore = new List<Person_short>();
            Family = new List<Person_short>();
            Message = new List<Message_obg_record>();
            News = new List<Memes_record>();
            Wall = new List<Memes_record>();
            Images = new List<byte[]>();
            Main_images = new List<byte[]>();
            Groups = new List<Group_short>();
            Black_list = new List<Person_short>();
        }
        public Personal_record(ApplicationUser a)
        {
            /* Name = a.Name;
             Status = a.Status;
             Birthday = a.Birthday;
             Age = a.Age;
             Login = a.Login;
             Nickname = a.Nickname;
             Name = a.Name;
             Name = a.Name;
             Name = a.Name;
             Name = a.Name;
             Name = a.Name;
             Name = a.Name;
             Name = a.Name;
             Name = a.Name;
             Name = a.Name;*/

            db = a;

            Followers = new List<Person_short>();
            Friends = new List<Person_short>();
            Followers_ignore = new List<Person_short>();
            Family = new List<Person_short>();
            Message = new List<Message_obg_record>();
            News = new List<Memes_record>();
            Wall = new List<Memes_record>();
            Images = new List<byte[]>();
            Main_images = new List<byte[]>();
            Groups = new List<Group_short>();
            Black_list = new List<Person_short>();
        }

    }

    public class Memes_record{
        public Memes db;
        public List<byte[]> Images { get; set; }
        public Memes_record()
        {
            db = null;
            Images = null;


        }
        public Memes_record(Memes a)
        {
            db = a;
            Images = null;


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
        public Person_info_short(Personal_record a)
        {
            Age = a.db.Age;
            Country = a.db.Country;
            Town = a.db.Town;
            Street = a.db.Street;
            Description = a.db.Description;
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
        public string Status { get; set; }

        public Group_short(string Status, string Group_id, int Count_followers, byte[] Image, string Name)
        {
            Id = -1;
            this.Status = Status;
            this.Group_id_s = Group_id;
            this.Count_followers = Count_followers;
            this.Image = Image;
            this.Name = Name;


        }
    }
   
}