using System;
using System.Collections.Generic;

namespace LinkNodeDomain.Model;

public partial class InviteStatus : Entity
{
    public string InviteStatus1 { get; set; } = null!;

    public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();
}
