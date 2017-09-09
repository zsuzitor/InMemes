using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;


namespace Im.Models
{
    
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
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
        
        public int Followers_count { get; set; }

        //друзья
        public string Friends_id { get; set; }//id друзей
        
        public int Friends_count { get; set; }
        // подписчики
        public string Followers_ignore_id { get; set; }//id подписчиков
        
        public int Followers_ignore_count { get; set; }
        //семья 
        public string Family_id { get; set; }
        
        public int Family_ignore_count { get; set; }
        //сообщения 
        public string Message_id { get; set; }//id объектов
        
        public int Message_count { get; set; }
        public int New_message_count { get; set; }
        //новости 
        public string News_id { get; set; }
        
        public int News_count { get; set; }
        //стена 
        public string Wall_id { get; set; }//id записей
        
        public int Wall_count { get; set; }
        //фото //
        public string Images_id { get; set; }//id фото
        
        public int Images_count { get; set; }
        //авы
        public string Main_images_id { get; set; }//id фото
        
        
        //группы
        public string Groups_id { get; set; }
        
        public int Groups_count { get; set; }




        public string Black_list_id { get; set; }//id людей
       
        
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
            
            Followers_count = 0;


            //друзья
            Friends_id = "";
           
            Friends_count = 0;

            // подписчики
            Followers_ignore_id = "";
            
            Followers_ignore_count = 0;

            //семья 
            Family_id = "";
            
            Family_ignore_count = 0;

            //сообщения 
            Message_id = "";
            
            Message_count = 0;
            New_message_count = 0;

            //новости 
            News_id = "";
            
            News_count = 0;

            //стена 
            Wall_id = "";
            
            Wall_count = 0;

            //фото //
            Images_id = "";
            
            Images_count = 0;

            //авы
            Main_images_id = "";
            


            //группы
            Groups_id = "";
            
            Groups_count = 0;
        
         Black_list_id = "";
            
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