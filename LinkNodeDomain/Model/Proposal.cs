using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class Proposal : Entity
{
    public int VacancyId { get; set; }

    public int FreelancerId { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual Freelancer Freelancer { get; set; } = null!;

    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();

    public virtual Vacancy Vacancy { get; set; } = null!;
}
