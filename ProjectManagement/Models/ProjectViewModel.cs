using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Presentation.Models
{
    public class ProjectSummaryViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Название проекта")]
        public string Name { get; set; }
        [Display(Name = "Компания-заказчик")]
        public string CustomerCompanyName { get; set; }
        [Display(Name = "Компания-исполнитель")]
        public string ExecutorCompanyName { get; set; }
        [Display(Name = "Руководитель проекта")]
        public int? ProjectManagerID { get; set; }
        [Display(Name = "Руководитель проекта")]
        public string ProjectManagerFullName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        public DateTime FinishDate { get; set; }
        [Display(Name = "Приоритет")]
        public int Priority { get; set; }
    }

    public class ProjectDetailsViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Название проекта")]
        public string Name { get; set; }
        [Display(Name = "Компания-заказчик")]
        public string CustomerCompanyName { get; set; }
        [Display(Name = "Компания-исполнитель")]
        public string ExecutorCompanyName { get; set; }
        [Display(Name = "Руководитель проекта")]
        public int? ProjectManagerID { get; set; }
        [Display(Name = "Руководитель проекта")]
        public string ProjectManagerFullName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        public DateTime FinishDate { get; set; }
        [Display(Name = "Приоритет")]
        public int Priority { get; set; }
        public IEnumerable<ProjectEmployeeViewModel> Developers { get; set; }
    }

    public class EditProjectViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Не указано название проекта")]
        [Display(Name = "Название проекта")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указано название компании-заказчика")]
        [Display(Name = "Компания-заказчик")]
        public string OldName { get; set; }
        public string CustomerCompanyName { get; set; }
        [Required(ErrorMessage = "Не указано название компании-исполнителя")]
        [Display(Name = "Компания-исполнитель")]
        public string ExecutorCompanyName { get; set; }
        [Required(ErrorMessage = "Не указан руководитель проекта")]
        [Display(Name = "Руководитель проекта")]
        public int? ProjectManagerID { get; set; }
        [Required(ErrorMessage = "Не указана дата начала проекта")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Не указана дата окончания проекта")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        public DateTime FinishDate { get; set; }
        [Required(ErrorMessage = "Не указан приоритет проекта")]
        [Display(Name = "Приоритет")]
        public int Priority { get; set; }
    }

    public class CreateProjectViewModel
    {
        [Required(ErrorMessage = "Не указано название проекта")]
        [Display(Name = "Название проекта")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указано название компании-заказчика")]
        [Display(Name = "Компания-заказчик")]
        public string CustomerCompanyName { get; set; }
        [Required(ErrorMessage = "Не указано название компании-исполнителя")]
        [Display(Name = "Компания-исполнитель")]
        public string ExecutorCompanyName { get; set; }
        [Required(ErrorMessage = "Не указан руководитель проекта")]
        [Display(Name = "Руководитель проекта")]
        public int? ProjectManagerId { get; set; }
        [Required(ErrorMessage = "Не указана дата начала проекта")]
        [DataType(DataType.Date, ErrorMessage = "Не указана дата начала проекта")]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Не указана дата окончания проекта")]
        [DataType(DataType.Date, ErrorMessage = "Не указана дата окончания проекта")]
        [Display(Name = "Дата окончания")]
        public DateTime FinishDate { get; set; }
        [Required(ErrorMessage = "Не указан приоритет проекта",AllowEmptyStrings = false)]
        [Display(Name = "Приоритет")]
        public int Priority { get; set; }
    }

    public class DeleteProjectViewModel
    {
        [Required]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
