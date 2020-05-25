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
    [Table("Pages")]
    public class Page : DomainEntity<int>,
        ISwitchable
    {
        public Page() { }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Alias { get; set; }

        public string Content { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { set; get; }
    }
}
