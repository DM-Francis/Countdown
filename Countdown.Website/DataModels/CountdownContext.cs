using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Countdown.Website.DataModels
{
    public class CountdownContext : DbContext
    {
        public CountdownContext(DbContextOptions<CountdownContext> options) : base(options)
        {
        }

        public DbSet<Problem> Problems { get; set; }
        public DbSet<Solution> Solutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Problem>()
                .Property(p => p.AvailableNumbers)
                .HasConversion(l => JsonConvert.SerializeObject(l), s => JsonConvert.DeserializeObject<List<int>>(s));
        }
    }
}
