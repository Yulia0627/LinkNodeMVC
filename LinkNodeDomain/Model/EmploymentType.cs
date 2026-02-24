using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class EmploymentType : Entity
{
    public string EmpType { get; set; } = null!;

    public virtual ICollection<Freelancer> Freelancers { get; set; } = new List<Freelancer>();

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
