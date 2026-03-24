using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class Invite : Entity
{
    [Display(Name = "Фрілансер")]
    public int FreelancerId { get; set; }

    [Display(Name = "Статус")]
    public int StatusId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Вакансія")]
    public int VacancyId { get; set; }

    public virtual Freelancer Freelancer { get; set; } = null!;

    public virtual InviteStatus Status { get; set; } = null!;

    public virtual Vacancy Vacancy { get; set; } = null!;
}
