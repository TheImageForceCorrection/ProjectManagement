using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ProjectManagement.Data.Interfaces;
using ProjectManagement.Data.Models;
using ProjectManagement.Data.Contexts;

namespace ProjectManagement.Data.Wrappers
{
    public class ProjectRepository : IRepository<Project>
    {
        private ProjectContext context;

        public ProjectRepository(ProjectContext context)
        {
            this.context = context;
        }

        public IEnumerable<Project> GetAll()
        {
            return context.Projects;
        }

        public Project Get(int id)
        {
            return context.Projects.Find(id);
        }

        public void Create(Project project)
        {
            context.Projects.Add(project);
        }

        public void Update(Project project)
        {
            context.Entry(project).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Project project = context.Projects.Find(id);
            if (project != null)
                context.Projects.Remove(project);
        }
    }

    public class PersonRepository : IRepository<Person>
    {
        private ProjectContext context;

        public PersonRepository(ProjectContext context)
        {
            this.context = context;
        }

        public IEnumerable<Person> GetAll()
        {
            return context.Persons;
        }

        public Person Get(int id)
        {
            return context.Persons.Find(id);
        }

        public void Create(Person Person)
        {
            context.Persons.Add(Person);
        }

        public void Update(Person Person)
        {
            context.Entry(Person).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Person employee = context.Persons.Find(id);
            if (!context.Projects.ToList().Any(project => project.ProjectManager == employee)
                && employee != null)
                context.Persons.Remove(employee);
        }
    }

    public class ManagerRepository : IRepository<Manager>
    {
        private ProjectContext context;

        public ManagerRepository(ProjectContext context)
        {
            this.context = context;
        }

        public IEnumerable<Manager> GetAll()
        {
            return context.Managers;

        }

        public Manager Get(int id)
        {
            return context.Managers.Find(id);
        }

        public void Create(Manager manager)
        {
            context.Managers.Add(manager);
        }

        public void Update(Manager manager)
        {
            context.Entry(manager).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Manager manager = context.Managers.Find(id);
            if (manager != null)
                context.Managers.Remove(manager);
        }
    }

    public class DeveloperRepository : IRepository<Developer>
    {
        private ProjectContext context;

        public DeveloperRepository(ProjectContext context)
        {
            this.context = context;
        }

        public IEnumerable<Developer> GetAll()
        {
            return context.Developers;
        }

        public Developer Get(int id)
        {
            return context.Developers.Find(id);
        }

        public void Create(Developer developer)
        {
            context.Developers.Add(developer);
        }

        public void Update(Developer developer)
        {
            context.Entry(developer).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Developer developer = context.Developers.Find(id);
            if (developer != null)
                context.Developers.Remove(developer);
        }
    }
}
