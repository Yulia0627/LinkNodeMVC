using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class ActionType : Entity
{
    public string Action { get; set; } = null!;

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();
}
