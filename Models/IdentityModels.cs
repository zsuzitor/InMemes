using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;


namespace Im.Models
{
    public interface IPage_view
    {

    }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser, IPage_view
    {
        //поля

        //в IdentityUser есть  public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? Birthday { get; set; }
        
        

        //


        //TODO меню слева
        public int? Age { get; set; }
        public string Login { get; set; }
        public string Nickname { get; set; }

        public string Surname { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }


        public bool Online { get; set; }

        // заявки 
        public string Followers_id { get; set; }//id не принятых(отвергнутых) друзей
        public List<Person_short> Followers { get; set; }
        public int Followers_count { get; set; }

        //друзья
        public string Friends_id { get; set; }//id друзей
        public List<Person_short> Friends { get; set; }
        public int Friends_count { get; set; }
        // подписчики
        public string Followers_ignore_id { get; set; }//id подписчиков
        public List<Person_short> Followers_ignore { get; set; }
        public int Followers_ignore_count { get; set; }
        //семья 
        public string Family_id { get; set; }
        public List<Person_short> Family { get; set; }
        public int Family_ignore_count { get; set; }
        //сообщения 
        public string Message_id { get; set; }//id объектов
        public List<Message_obg> Message { get; set; }
        public int Message_count { get; set; }
        public int New_message_count { get; set; }
        //новости 
        public string News_id { get; set; }
        public List<Memes> News { get; set; }
        public int News_count { get; set; }
        //стена 
        public string Wall_id { get; set; }//id записей
        public List<Memes> Wall { get; set; }
        public int Wall_count { get; set; }
        //фото //
        public string Images_id { get; set; }//id фото
        public List<byte[]> Images { get; set; }
        public int Images_count { get; set; }
        //авы
        public string Main_images_id { get; set; }//id фото
        public List<byte[]> Main_images { get; set; }
        
        //группы
        public string Groups_id { get; set; }
        public List<Group_short> Groups { get; set; }
        public int Groups_count { get; set; }




        public string Black_list_id { get; set; }//id людей
       
        public List<Person_short> Black_list { get; set; }
        public int Black_list_count { get; set; }
        //о себе 
        public string Description { get; set; }

        //настройки о том что слева и остальное

        public string Menu_left { get; set; }


        public ApplicationUser() : base(){
            Age = null;
            Nickname = null;
            Online = false;
            Surname = null;
            Country = null;
            Town = null;
            Street = null;
            Status = null;
            Birthday = null;
            // заявки
            Followers_id = "";
            Followers = new List<Person_short>();
            Followers_count = 0;


            //друзья
            Friends_id = "";
            Friends = new List<Person_short>();
            Friends_count = 0;

            // подписчики
            Followers_ignore_id = "";
            Followers_ignore = new List<Person_short>();
            Followers_ignore_count = 0;

            //семья 
            Family_id = "";
            Family = new List<Person_short>();
            Family_ignore_count = 0;

            //сообщения 
            Message_id = "";
            Message = new List<Message_obg>();
            Message_count = 0;
            New_message_count = 0;

            //новости 
            News_id = "";
            News = new List<Memes>();
            News_count = 0;

            //стена 
            Wall_id = "";
            Wall = new List<Memes>();
            Wall_count = 0;

            //фото //
            Images_id = "";
            Images = new List<byte[]>();
            Images_count = 0;

            //авы
            Main_images_id = "";
            Main_images = new List<byte[]>();


            //группы
            Groups_id = "";
            Groups = new List<Group_short>();
            Groups_count = 0;
        
         Black_list_id = "";
            Black_list = new List<Person_short>();
            Black_list_count = 0;

            //о себе 
            Description = null;

            //настройки о том что слева и остальное
            //TODO сюда те поля которые слева должны быть
            Menu_left = "Моя Cтраница,Personal_record,Новости,News,Сообщения,Mesages,Друзья,Friends,Фотографии,Albums,Музыка,Music,Видео,Video";
    }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }
    








    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}