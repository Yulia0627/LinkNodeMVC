using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class User : Entity
{
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Country { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public virtual Admin? AdminAdminNavigation { get; set; }

    public virtual ICollection<Admin> AdminTargetUsers { get; set; } = new List<Admin>();

    public virtual Client? Client { get; set; }

    public virtual Freelancer? Freelancer { get; set; }

    public virtual UserRole Role { get; set; } = null!;
}
