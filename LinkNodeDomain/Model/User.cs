using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkNodeDomain.Model;

public partial class User : IdentityUser<int>
{
    [Column("name")] 
    public string Name { get; set; } = null!;

    [Column("surname")]
    public string Surname { get; set; } = null!;

    [Column("country")]
    public string Country { get; set; } = null!;

    [Column("isactive")]
    public bool IsActive { get; set; }

    [Column("createddate")]
    public DateTime CreatedDate { get; set; }

    [Column("updateddate")]
    public DateTime UpdatedDate { get; set; }

    public virtual ICollection<AdminAction> AdminTargetUsers { get; set; } = new List<AdminAction>();
    public virtual ICollection<AdminAction> AdminActions { get; set; } = new List<AdminAction>();

    public virtual Client? Client { get; set; }

    public virtual Freelancer? Freelancer { get; set; }
}
