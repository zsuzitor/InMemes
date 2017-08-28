﻿using System.Data.Entity;
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
        public DateTime Birthday { get; set; }
        
        

        //


        //TODO меню слева
        public int Age { get; set; }
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

        //друзья
        public string Friends_id { get; set; }//id друзей
        public List<Person_short> Friends { get; set; }

        // подписчики
        public string Followers_ignore_id { get; set; }//id подписчиков
        public List<Person_short> Followers_ignore { get; set; }

        //семья 
        public string Family_id { get; set; }
        public List<Person_short> Family { get; set; }

        //сообщения 
        public string Message_id { get; set; }//id объектов
        public List<Message_obg> Message { get; set; }

        //новости 
        public string News_id { get; set; }
        public List<Memes> News { get; set; }

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

       


        public string Black_list_id { get; set; }//id людей
        public List<Person_short> Black_list { get; set; }

        //о себе 
        public string Description { get; set; }

        //настройки о том что слева и остальное

        public string Menu_left { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }
    public class Group 
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





    }
    public class Img
    {
        public string Name;
        public int Id { get; set; }
        public byte[] bytes;

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

    }


    public class Message_obg
    {
        public int Id { get; set; }
        public string Person_id { get; set; }
        public List<Person_short> Person { get; set; }
        public string Messages_str { get; set; }
        public List<string> Messages { get; set; }//TODO нет прикрепленных файлов к сообщению
        public byte[] Image { get; set; }
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