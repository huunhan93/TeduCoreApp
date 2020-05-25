using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;

namespace TeduCoreApp.Data.Entities
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser<Guid>, 
        ISwitchable, IDateTracking
    {
        [Required]
        [StringLength(250)]
        public string FullName { get; set; }

        public DateTime? BirthDay { get; set; }

        public decimal Balance { get; set; }

        [StringLength(250)]
        public string Avatar { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

    }
}
