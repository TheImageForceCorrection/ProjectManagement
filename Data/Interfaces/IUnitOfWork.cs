using System;
using ProjectManagement.Data.Models;

namespace ProjectManagement.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Project> Projects { get; }
        IRepository<Person> Persons { get; }
        IRepository<Manager> Managers { get; }
        IRepository<Developer> Developers { get; }
        void Save();
    }
}
