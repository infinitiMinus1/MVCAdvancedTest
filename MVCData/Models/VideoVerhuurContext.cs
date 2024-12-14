using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Models
{
    public class VideoVerhuurContext : DbContext
    {

        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Verhuring> Verhuringen { get; set; }

        public VideoVerhuurContext()
        {
        }
        public VideoVerhuurContext(DbContextOptions options) : base(options){ }
    }
}
