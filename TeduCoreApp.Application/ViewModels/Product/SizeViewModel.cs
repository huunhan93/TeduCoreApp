using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeduCoreApp.Application.ViewModels.Product
{
    public class SizeViewModel
    {
        public int Id { set; get; }

        [StringLength(250)]
        public string Name
        {
            get; set;
        }
    }
}
