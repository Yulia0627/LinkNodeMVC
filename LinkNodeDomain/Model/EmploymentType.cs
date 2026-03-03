using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkNodeDomain.Model;

public partial class EmploymentType : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
    [Display(Name = "Тип зайнятості")]
    public string EmpType { get; set; } = null!;

    public virtual ICollection<Freelancer> Freelancers { get; set; } = new List<Freelancer>();

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
