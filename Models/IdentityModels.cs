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
        public  string Id_public { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Sex { get; set; }
        public string Marital_status { get; set; }

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


        public int Followers_count { get; set; }

       
        
        public int Friends_count { get; set; }
        
        
        public int Followers_ignore_count { get; set; }
        

        public int Message_count { get; set; }
        public int New_message_count { get; set; }
        
        
        public int News_count { get; set; }

        public int Wall_count { get; set; }

        public int Images_count { get; set; }
       

        
        public int Groups_count { get; set; }

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
            Marital_status = null;
            Sex = true;

            Followers_count = 0;

            Friends_count = 0;
  
            Followers_ignore_count = 0;

            Message_count = 0;
            New_message_count = 0;

            News_count = 0;

            Wall_count = 0;

            Images_count = 0;

            Groups_count = 0;

            Black_list_count = 0;

            //о себе 
            Description = null;

            //настройки о том что слева и остальное
            //TODO сюда те поля которые слева должны быть
            Menu_left = "Моя Cтраница,Personal_record,Новости,News,Сообщения,Messages,Группы,Groups_personal,Друзья,Friends,Фотографии,Albums,Музыка,Music,Видео,Video";
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
        public DbSet<Group> Groups { get; set; }
        public DbSet<Img> Images { get; set; }
        public DbSet<Memes> Memes { get; set; }
        public DbSet<Message_obg> Messages_obg { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Stiker> Stikers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        //Relationship_string_string  Relationship_with_admin_group Relationship_with_memes Relationship_with_images
        //
        public DbSet<Relationship_string_string_Followers_connected> Followers_connected { get; set; }
       // public DbSet<Relationship_string_string_Followers_ignore_connected> Followers_ignore_connected { get; set; }
        public DbSet<Relationship_string_string_Family_connected> Family_connected { get; set; }
        public DbSet<Relationship_string_string_Groups_connected> Groups_connected { get; set; }
        public DbSet<Relationship_string_string_Liked_connected> Liked_connected { get; set; }
        public DbSet<Relationship_string_string_Repost_connected> Repost_connected { get; set; }
        public DbSet<Relationship_string_string_Messages_one_dialog_connected> Messages_one_dialog_connected { get; set; }
        public DbSet<Relationship_string_string_Messages_dialog_person_connected> Messages_dialog_person_connected { get; set; }
        public DbSet<Relationship_string_string_Black_list_connected> Black_list_connected { get; set; }
        public DbSet<Relationship_with_admin_group> Friends_connected { get; set; }
        public DbSet<Relationship_with_memes> Wall_memes_connected { get; set; }
        public DbSet<Relationship_mem_comment> Mem_comment_connected { get; set; }
        public DbSet<Relationship_with_images> Images_mem_connected { get; set; }

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