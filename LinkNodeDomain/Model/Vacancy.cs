using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class Vacancy : Entity
{
    public int ClientId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Назва вакансії")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Тип зайнятості")]
    public int EmpTypeId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Категорія")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Погодинна оплата")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Опис")]
    public string Description { get; set; } = null!;

    [Display(Name = "Дата створення")]
    public DateTime CreatedDate { get; set; }

    [Display(Name = "Дата закриття")]
    public DateTime? ClosedDate { get; set; }

    public virtual ICollection<AdminAction> AdminActions { get; set; } = new List<AdminAction>();

    [Display(Name = "Категорія")]  
    public virtual Category Category { get; set; } = null!;

    [Display(Name = "Клієнт")]
    public virtual Client Client { get; set; } = null!;

    [Display(Name = "Тип зайнятості")]
    public virtual EmploymentType EmpType { get; set; } = null!;

    public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();

    public virtual ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
}
