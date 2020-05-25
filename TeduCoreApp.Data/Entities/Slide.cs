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
    [Table("Slides")]
    public class Slide : DomainEntity<int>, 
        ISortable, ISwitchable
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [MaxLength(256)]
        public string Url { get; set; }

        [MaxLength(256)]
        [Required]
        public string Image { get; set; }

        public int SortOrder { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { get; set; }
    }
}
