using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class Freelancer : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Категорія")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Погодинна ставка")]
    public decimal HourlyRate { get; set; }

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Тип зайнятості")]
    public int EmpTypeId { get; set; }

    [Display(Name = "Категорія")]
    public virtual Category Category { get; set; } = null!;

    [Display(Name = "Тип зайнятості")]
    public virtual EmploymentType EmpType { get; set; } = null!;

    public virtual User FreelancerNavigation { get; set; } = null!;

    public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();

    public virtual ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
}
