﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraveLust.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<UserInGroupchat> UserInGroupchats { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }
    }
}
