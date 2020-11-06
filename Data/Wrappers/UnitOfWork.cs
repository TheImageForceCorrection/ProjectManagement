using System;
using ProjectManagement.Data.Interfaces;
using ProjectManagement.Data.Models;
using ProjectManagement.Data.Contexts;

namespace ProjectManagement.Data.Wrappers
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProjectContext context = new ProjectContext();
        private ProjectRepository projectRepository;
        private PersonRepository personRepository;
        private ManagerRepository managerRepository;
        private DeveloperRepository developerRepository;

        public IRepository<Project> Projects
        {
            get
            {
                if (projectRepository == null)
                    projectRepository = new ProjectRepository(context);
                return projectRepository;
            }
        }

        public IRepository<Person> Persons
        {
            get
            {
                if (personRepository == null)
                    personRepository = new PersonRepository(context);
                return personRepository;
            }
        }

        public IRepository<Manager> Managers
        {
            get
            {
                if (managerRepository == null)
                    managerRepository = new ManagerRepository(context);
                return managerRepository;
            }
        }

        public IRepository<Developer> Developers
        {
            get
            {
                if (developerRepository == null)
                    developerRepository = new DeveloperRepository(context);
                return developerRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
