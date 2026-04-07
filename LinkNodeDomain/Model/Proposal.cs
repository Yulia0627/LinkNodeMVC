using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class Proposal : Entity
{
    public int VacancyId { get; set; }

    public int FreelancerId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Погодинна оплата")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Опис")]
    public string Description { get; set; } = null!;

    [Display(Name = "Дата створення")]
    public DateTime CreatedDate { get; set; }

    [Display(Name = "Фрілансер")]
    public virtual Freelancer Freelancer { get; set; } = null!;

    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();

    [Display(Name = "Вакансія")]
    public virtual Vacancy Vacancy { get; set; } = null!;
}
