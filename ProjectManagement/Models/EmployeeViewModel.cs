using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjectManagement.Logic.Common;

namespace ProjectManagement.Presentation.Models
{
    public class EmployeeProjectRelationChange
    {
        [Required]
        public int? employeeID { get; set; }
        [Required]
        public int? projectID { get; set; }
        [Required]
        public bool? isFromProject { get; set; }
    }

    public class ProjectEmployeeViewModel
    {
        public int ID { get; set; }
        [Display(Name = "ФИО")]
        public string FullName { get; set; }
    }

    public class EmployeeProjectViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Название проекта")]
        public string Name { get; set; }
    }

    public class EmployeeViewModel
    {
        public int ID { get; set; }
        [Display(Name = "ФИО")]
        public string FullName { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Проекты")]
        public IEnumerable<EmployeeProjectViewModel> Projects { get; set; }
        [Display(Name = "Должность")]
        public Role Role { get; set; }
    }

    public class EmployeeDetailsViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Не указано имя сотрудника")]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указана фамилия сотрудника")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Не указано отчество сотрудника")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "ФИО")]
        public string FullName
        {
            get
            {
                return Surname + " " + Name + " " + Patronymic;
            }
        }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Должность")]
        public Role Role { get; set; }
        [Display(Name = "Проекты")]
        public IEnumerable<EmployeeProjectViewModel> Projects { get; set; }
    }

    public class EditEmployeeViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Не указано имя сотрудника")]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указана фамилия сотрудника")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Не указано отчество сотрудника")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "ФИО")]
        public string FullName
        {
            get
            {
                return Surname + " " + Name + " " + Patronymic;
            }
        }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }

    public class CreateEmployeeViewModel
    {
        [Required(ErrorMessage = "Не указано имя сотрудника")]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указана фамилия сотрудника")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Не указано отчество сотрудника")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Required(ErrorMessage = "Не указана Должность сотрудника")]
        [Display(Name = "Должность")]
        public Role Role { get; set; }

        [Required(ErrorMessage = "Не указан e-mail сотрудника")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }

    public class DeleteEmployeeViewModel
    {
        [Required]
        public int ID { get; set; }
        public string FullName { get; set; }
        public IEnumerable<EmployeeProjectViewModel> LinkedProjects { get; set; }
    }
}
