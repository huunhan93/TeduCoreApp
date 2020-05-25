using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.Infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Feedbacks")]
    public class Feedback : DomainEntity<int>,
        IDateTracking, ISwitchable
    {
        public Feedback() { }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        
        [StringLength(500)]
        public string Messgae { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { get; set; }
    }
}
