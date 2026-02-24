using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class Invite : Entity
{
    public int FreelancerId { get; set; }

    public int StatusId { get; set; }

    public int VacancyId { get; set; }

    public virtual Freelancer Freelancer { get; set; } = null!;

    public virtual InviteStatus Status { get; set; } = null!;

    public virtual Vacancy Vacancy { get; set; } = null!;
}
