using System.Data.Entity;
using ProjectManagement.Data.Models;

namespace ProjectManagement.Data.Contexts
{
    public class ProjectContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Developer> Developers { get; set; }

        public ProjectContext() : base("DBConnection")
        {
            Database.SetInitializer(new ProjectInitializer());
        }
    }
}
