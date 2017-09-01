using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Im.Models
{
    public class All_db_Context : DbContext
    {

        public DbSet<Group> Groups { get; set; }
        public DbSet<Img> Images { get; set; }
        public DbSet<Memes> Memes { get; set; }
        public DbSet<Message_obg> Messages_obg { get; set; }
        public DbSet<Message> Messages { get; set; }

        
    }
}