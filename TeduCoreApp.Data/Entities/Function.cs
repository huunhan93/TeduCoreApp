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
    [Table("Functions")]
    public class Function : DomainEntity<string>
        ,ISwitchable, ISortable
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Url { get; set; }

        [StringLength(128)]
        public string ParentId { get; set; }

        [StringLength(20)]
        public string IconCss { get; set; }
        public Status Status { get; set; }
        public int SortOrder { get; set; }
    }
}
