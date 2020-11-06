using System;
using System.Collections.Generic;
using System.Data.Entity;
using ProjectManagement.Data.Models;

namespace ProjectManagement.Data.Contexts
{
        public class ProjectInitializer: CreateDatabaseIfNotExists<ProjectContext>
    {
        protected override void Seed(ProjectContext context)
        {
            List<Person> initialPersons = new List<Person>();
            List<Manager> initialManagers = new List<Manager>();
            List<Developer> initialDevelopers = new List<Developer>();
            List<Project> initialProjects = new List<Project>();

            initialPersons.Add(new Person()
            {
                Surname = "Фёдоров",
                Name = "Борис",
                Patronymic = "Мкртичевич",
                Email = "borya@gmail.com"
            });

            initialPersons.Add(new Person()
            {
                Surname = "Залесский",
                Name = "Валентин",
                Patronymic = "Викторович",
                Email = "behindTheForest@yandex.ru"
            });
            initialPersons.Add(new Person()
            {
                Surname = "Фадеев",
                Name = "Денис",
                Patronymic = "Димитриевич",
                Email = "fadey@yandex.ru"
            });
            initialPersons.Add(new Person()
            {
                Surname = "Джобс",
                Name = "Стивен",
                Patronymic = "Полович",
                Email = "steveJobs@apple.com"
            });
            initialManagers.Add(new Manager()
            {
                Surname = "Фёдоров",
                Name = "Иван",
                Patronymic = "Сергеевич",
                Email = "f_i@mail.ru"
            });

            initialManagers.Add(new Manager()
            {
                Surname = "Владимиров",
                Name = "Борис",
                Patronymic = "Алексеевич",
                Email = "onetwothree@gmail.com"
            });
            initialManagers.Add(new Manager()
            {
                Surname = "Петров",
                Name = "Георгий",
                Patronymic = "Майклович",
                Email = "petrovgeorge@yandex.ru"
            });
            initialManagers.Add(new Manager()
            {
                Surname = "Коротеев",
                Name = "Джеймс",
                Patronymic = "Валерианович",
                Email = "korj@mail.ru"
            });

            initialDevelopers.Add(new Developer()
            {
                Surname = "Константинов",
                Name = "Владислав",
                Patronymic = "Павлович",
                Email = "konst_vladislav@mail.ru"
            });

            initialDevelopers.Add(new Developer()
            {
                Surname = "Гейтс",
                Name = "Уильям",
                Patronymic = "Генриевич",
                Email = "billgates@microsoft.com"
            });
            initialDevelopers.Add(new Developer()
            {
                Surname = "Петров",
                Name = "Александр",
                Patronymic = "Алексеевич",
                Email = "alexpetrov@mail.ru"
            });
            initialDevelopers.Add(new Developer()
            {
                Surname = "Вахрушев",
                Name = "Станислав",
                Patronymic = "Ростиславович",
                Email = "vsr@yandex.ru"
            });

            context.Persons.AddRange(initialPersons);
            context.Managers.AddRange(initialManagers);
            context.Developers.AddRange(initialDevelopers);
            context.SaveChanges();

            initialProjects.Add(new Project()
            {
                Name = "Project 1",
                CustomerCompanyName = "Google",
                ExecutorCompanyName = "The best dev inc",
                StartDate = DateTime.Now - TimeSpan.FromDays(20),
                FinishDate = DateTime.Now + TimeSpan.FromDays(1),
                ProjectManager = initialManagers[0],
                Priority = 0
            });

            initialProjects[0].Developers.Add(initialDevelopers[1]);
            initialProjects[0].Developers.Add(initialDevelopers[2]);
            initialProjects[0].Developers.Add(initialDevelopers[3]);

            initialProjects.Add(new Project()
            {
                Name = "Project 2",
                CustomerCompanyName = "Google",
                ExecutorCompanyName = "IntelPlay",
                StartDate = DateTime.Now - TimeSpan.FromDays(15),
                FinishDate = DateTime.Now + TimeSpan.FromDays(15),
                ProjectManager = initialManagers[0],
                Priority = 1
            });

            initialProjects[1].Developers.Add(initialDevelopers[1]);
            initialProjects[1].Developers.Add(initialDevelopers[2]);
            initialProjects[1].Developers.Add(initialDevelopers[3]);


            initialProjects.Add(new Project()
            {
                Name = "The best Project",
                CustomerCompanyName = "The most famous company in the world",
                ExecutorCompanyName = "The best dev inc",
                StartDate = DateTime.Now - TimeSpan.FromDays(5),
                FinishDate = DateTime.Now + TimeSpan.FromDays(40),
                ProjectManager = initialManagers[0],
                Priority = 1
            });

            initialProjects[2].Developers.Add(initialDevelopers[1]);
            initialProjects[2].Developers.Add(initialDevelopers[2]);
            initialProjects[2].Developers.Add(initialDevelopers[3]);


            initialProjects.Add(new Project()
            {
                Name = "One more project",
                CustomerCompanyName = "YeahDevelopment",
                ExecutorCompanyName = "IntelPlay",
                StartDate = DateTime.Now - TimeSpan.FromDays(15),
                FinishDate = DateTime.Now + TimeSpan.FromDays(15),
                ProjectManager = initialManagers[0],
                Priority = 0
            });

            initialProjects[3].Developers.Add(initialDevelopers[2]);
            initialProjects[3].Developers.Add(initialDevelopers[3]);

            initialProjects.Add(new Project()
            {
                Name = "Another Project",
                CustomerCompanyName = "Yandex",
                ExecutorCompanyName = "The best dev inc",
                StartDate = DateTime.Now - TimeSpan.FromDays(20),
                FinishDate = DateTime.Now + TimeSpan.FromDays(180),
                ProjectManager = initialManagers[1],
                Priority = 0
            });

            initialProjects[4].Developers.Add(initialDevelopers[1]);
            initialProjects[4].Developers.Add(initialDevelopers[3]);


            initialProjects.Add(new Project()
            {
                Name = "Another one more Proj",
                CustomerCompanyName = "Yandex",
                ExecutorCompanyName = "IntelPlay",
                StartDate = DateTime.Now - TimeSpan.FromDays(15),
                FinishDate = DateTime.Now + TimeSpan.FromDays(60),
                ProjectManager = initialManagers[2],
                Priority = 2
            });

            initialProjects[5].Developers.Add(initialDevelopers[2]);
            initialProjects[5].Developers.Add(initialDevelopers[3]);

            context.Projects.AddRange(initialProjects);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
