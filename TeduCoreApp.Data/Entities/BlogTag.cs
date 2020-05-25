using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TeduCoreApp.Infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("BlogTags")]
    public class BlogTag : DomainEntity<int>
    {
        public BlogTag() { }

        public int BlogId { get; set; }
        public int TagId { get; set; }

        [ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; }

        [ForeignKey("TagId")]
        public virtual Tag tag { get; set; }
    }
}
