using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class Vacancy : Entity
{
    public int ClientId { get; set; }

    public string Title { get; set; } = null!;

    public int EmpTypeId { get; set; }

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? ClosedDate { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual Category Category { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual EmploymentType EmpType { get; set; } = null!;

    public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();

    public virtual ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
}
