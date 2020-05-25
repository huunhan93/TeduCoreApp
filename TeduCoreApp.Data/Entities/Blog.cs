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
    [Table("Blogs")]
    public class Blog : DomainEntity<int>, 
        IHasSeoMetaData, ISwitchable, IDateTracking
    {
        public Blog() { }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string Content { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        public bool? HomeFlag { get; set; }

        public bool? HotFlag { get; set; }

        public int? ViewCount { get; set; }

        public string Tags { get; set; }

        [MaxLength(256)]
        public string SeoPageTitle { get; set; }

        [MaxLength(256)]
        public string SeoAlias { get; set; }

        [MaxLength(256)]
        public string SeoKeywords { get; set; }

        [MaxLength(256)]
        public string SeoDescription { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual ICollection<BlogTag> BlogTags { get; set; }
    }
}
