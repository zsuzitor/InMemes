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
        public DateTime Birthday { get; set; }
        public string Albums { get; set; }

        public Img()
        {
            Id = 0;
            bytes = null;
            Birthday = DateTime.Now;
            Albums = "";
        }
        public Img(byte[] a)
        {
            Id = 0;
            bytes = a;
            Birthday = DateTime.Now;
            Albums = "";
        }

    }
    public class Group
    {

        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime Birthday { get; set; }
        public byte[] Image { get; set; }
        public bool Open_group { get; set; }
        public bool Add_memes_private { get; set; }
        public int Wall_count { get; set; }




        public Group()
        {
            Id = 0;
            Name = "";
            Wall_count = 0;
            Status = "";
            Birthday = DateTime.Now;
            
            
            Open_group = true;
            Add_memes_private = true;

        }

            public Group(string a,string id)
        {
            Id = 0;
            Name = a;
            Wall_count = 0;
            Status = "";
            Birthday = DateTime.Now;
            
            Open_group = true;
            Add_memes_private = true;

        }

    }

    public class Memes
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Source_type { get; set; }
        public string Source_id { get; set; }
        public byte[] Image { get; set; }
        
        
        public DateTime Birthday { get; set; }

        public Memes()
        {
            Id = 0;
            this.Source = null;
            this.Source_type = null;
            this.Source_id = null;
            Description = null;
            Image = null;
            
            
            Birthday = DateTime.Now;
        }
        public Memes(string Source, string Source_id)
        {
            Id = 0;
            this.Source = Source;
            this.Source_id = Source_id;
            Source_type = null;
            Description = null;
            Image = null;
            
            Birthday = DateTime.Now;


        }
    }




    public class Message_obg
    {
        public int Id { get; set; }
        public string Person_id { get; set; }
        
       
        
        public byte[] Image { get; set; }
        public int New_message_count { get; set; }

        public Message_obg()
        {
            Id = 0;
            this.Person_id = null;

           

            Image = null;
            New_message_count = 0;
        }
            public Message_obg(string Person_id)
        {
            Id = -1;
            this.Person_id = Person_id;
            
           
            
            Image = null;
            New_message_count = 0;
        }
    }

    public class Message
    {
        public int Id { get; set; }
        public string Message_text { get; set; }
        public string Person_id { get; set; }
         public string Person_Name { get; set; }//размутить
        public byte[] Image { get; set; }
        public DateTime Birthday { get; set; }
        public Message()
        {
            Id = 0;
            this.Message_text = null;
            this.Person_id = null;
            Image = null;
            //Person_Name = ""; размутить
        }

        public Message(string Message_text, string Person_id,string Name)
        {
            Id = 0;
            this.Message_text = Message_text;
            this.Person_id = Person_id;
            Image = null;
             Person_Name = Name; //размутить
            Birthday = DateTime.Now;
        }

    }

    //------------------------------------------------------------------------------------------------------------------------


        public class Message_obg_record
    {
        public Message_obg db;
        
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
        public List<Person_short> Friends { get; set; }
        //стена 

        public List<Memes_record> Wall { get; set; }
        //фото //

        public List<byte[]> Images { get; set; }

        //авы

        public List<byte[]> Main_images { get; set; }
        //группы

        public List<Group_short> Groups { get; set; }

        //админы
        public List<Person_short> Admins { get; set; }
        public Group_record(Group a)
        {
            db = a;
            Followers = new List<Person_short>();
            Wall = new List<Memes_record>();
            Images = new List<byte[]>();
            Main_images = new List<byte[]>();
            Groups = new List<Group_short>();
            Admins = new List<Person_short>();
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

    

    





        public class Person_short: IPage_view
    {
        
        public string Person_id { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }

        public Person_short(string id, byte[] Image, string Name)
        {
            
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

        public class Group_short: IPage_view
    {
        public int Id { get; set; }
        //public List<string> Group_id { get; set; }
        
        public int Count_followers { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Group_short(Group a,int Count_followers,IEnumerable<string> Group_id)
        {
            Id = a.Id;
            this.Status = a.Status;
            //this.Group_id = Group_id.ToList();
            this.Count_followers = Count_followers;
            this.Image = a.Image;
            this.Name = a.Name;


        }
            public Group_short(int id,string Status, IEnumerable<string> Group_id, int Count_followers, byte[] Image, string Name)
        {
            Id = id;
            this.Status = Status;
            //this.Group_id = Group_id.ToList();
            this.Count_followers = Count_followers;
            this.Image = Image;
            this.Name = Name;


        }
    }





    public abstract class Relationship_string_string
    {
        public int Id { get; set; }
        public string Something_one_id { get; set; }//"главное" если связь не между людьми то засовывать сюда
        public string Something_two_id { get; set; }
        public Relationship_string_string()
        {
            Something_one_id = "";
            Something_two_id = "";
        }
        public Relationship_string_string(string a,string b)
        {
            Something_one_id = a;
            Something_two_id = b;
        }
    }
    public  class Relationship_string_string_Followers_connected : Relationship_string_string
    {

        public Relationship_string_string_Followers_connected() : base()
        {
        }
            public Relationship_string_string_Followers_connected(string a, string b) : base(a, b)
        {

        }
    }
    public class Relationship_string_string_Followers_ignore_connected : Relationship_string_string
    {

        public Relationship_string_string_Followers_ignore_connected() : base()
        {
        }
        public Relationship_string_string_Followers_ignore_connected(string a, string b) : base(a, b)
        {

        }
    }
    public class Relationship_string_string_Family_connected : Relationship_string_string
    {

        public Relationship_string_string_Family_connected() : base()
        {
        }
        public Relationship_string_string_Family_connected(string a, string b) : base(a, b)
        {

        }
    }
    public class Relationship_string_string_Groups_connected : Relationship_string_string
    {

        public Relationship_string_string_Groups_connected() : base()
        {
        }
        public Relationship_string_string_Groups_connected(string a, string b) : base(a, b)
        {

        }
    }
    public class Relationship_string_string_Liked_connected : Relationship_string_string
    {

        public Relationship_string_string_Liked_connected() : base()
        {
        }
        public Relationship_string_string_Liked_connected(string a, string b) : base(a, b)
        {

        }
    }
    public class Relationship_string_string_Repost_connected : Relationship_string_string
    {

        public Relationship_string_string_Repost_connected() : base()
        {
        }
        public Relationship_string_string_Repost_connected(string a, string b) : base(a, b)
        {

        }
    }
    public class Relationship_string_string_Messages_one_dialog_connected : Relationship_string_string
    {

        public Relationship_string_string_Messages_one_dialog_connected() : base()
        {
        }
        public Relationship_string_string_Messages_one_dialog_connected(string a, string b) : base(a, b)
        {

        }
    }
    public class Relationship_string_string_Messages_dialog_person_connected : Relationship_string_string
    {

        public Relationship_string_string_Messages_dialog_person_connected() : base()
        {
        }
        public Relationship_string_string_Messages_dialog_person_connected(string a, string b) : base(a, b)
        {

        }
    }
    public class Relationship_string_string_Black_list_connected : Relationship_string_string
    {

        public Relationship_string_string_Black_list_connected() : base()
        {
        }
        public Relationship_string_string_Black_list_connected(string a, string b) : base(a, b)
        {

        }
    }
    public class Relationship_with_admin_group
    {
        public int Id { get; set; }
        public string Something_one_id { get; set; }//"главное" если связь не между людьми то засовывать сюда
        public string Something_two_id { get; set; }
        public bool Admin_group { get; set; }
        public Relationship_with_admin_group()
        {
            Something_one_id = "";
            Something_two_id = "";
            Admin_group = false;
        }
        public Relationship_with_admin_group(string a, string b,bool c)
        {
            Something_one_id = a;
            Something_two_id = b;
            Admin_group = c;
        }
    }
    public class Relationship_with_memes
    {
        public int Id { get; set; }
        public string Something_one_id { get; set; }//"главное" если связь не между людьми то засовывать сюда
        public string Something_two_id { get; set; }
        public bool News { get; set; }
        public Relationship_with_memes()
        {
            Something_one_id = "";
            Something_two_id = "";
            News = false;
        }
        public Relationship_with_memes(string a, string b, bool c)
        {
            Something_one_id = a;
            Something_two_id = b;
            News = c;
        }
    }
    public class Relationship_with_images
    {
        public int Id { get; set; }
        public string Something_one_id { get; set; }//"главное" если связь не между людьми то засовывать сюда
        public string Something_two_id { get; set; }
        public bool Main { get; set; }
        public Relationship_with_images()
        {
            Something_one_id = "";
            Something_two_id = "";
            Main = false;
        }
        public Relationship_with_images(string a, string b, bool c)
        {
            Something_one_id = a;
            Something_two_id = b;
            Main = c;
        }
    }
}