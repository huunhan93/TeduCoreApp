using System;
using System.Collections.Generic;
using System.Text;

namespace TeduCoreApp.Application.ViewModels.System
{
    public class PermissionViewModel
    {
        public int Id { get; set; }

        public bool CanCreate { get; set; }

        public bool CanDelete { get; set; }

        public bool CanRead { get; set; }

        public bool CanUpdate { get; set; }

        public string FunctionId { get; set; }

        public Guid RoleId { get; set; }

        public AppRoleViewModel AppRole { get; set; }

        public FunctionViewModel Function { get; set; }

    }
}
