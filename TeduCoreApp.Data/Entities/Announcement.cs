using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.Infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Announcements")]
    public class Announcement : DomainEntity<string>,
        ISwitchable, IDateTracking
    {
        public Announcement() { }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(256)]
        public string Content { get; set; }

        [StringLength(450)]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        public Status Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DateCreated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DateModified { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
