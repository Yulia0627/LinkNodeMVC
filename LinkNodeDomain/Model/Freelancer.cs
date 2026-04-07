using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class Freelancer : Entity
{
    [Required(ErrorMessage = "Будь ласка, оберіть напрямок діяльності.")]
    [Display(Name = "Категорія")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Погодинна ставка")]
    [Range(0, 100000000, ErrorMessage = "Ставка повинна бути в межах від 0 до 100000000.")]
    [DataType(DataType.Currency)]
    public decimal HourlyRate { get; set; }

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Будь ласка, оберіть тип зайнятості.")]
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
