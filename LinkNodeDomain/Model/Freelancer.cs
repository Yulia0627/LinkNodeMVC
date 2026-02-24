using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class Freelancer : Entity
{
    public int CategoryId { get; set; }

    public decimal HourlyRate { get; set; }

    public string? Description { get; set; }

    public int EmpTypeId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual EmploymentType EmpType { get; set; } = null!;

    public virtual User FreelancerNavigation { get; set; } = null!;

    public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();

    public virtual ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
}
