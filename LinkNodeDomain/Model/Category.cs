using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class Category : Entity
{
    [Required(ErrorMessage ="Поле не повинно бути порожнім.")]
    [Display(Name="Категорія")]
    public string Category1 { get; set; } = null!;

    public virtual ICollection<Freelancer> Freelancers { get; set; } = new List<Freelancer>();

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
