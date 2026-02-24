using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class UserRole : Entity
{
    public string Role { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
