using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Extensions;
using TeduCoreApp.Utilities.Constants;

namespace TeduCoreApp.Areas.Admin.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private IFunctionService _functionService;

        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAll();
            }
            else
            {
                //TODO: Get by permision
                functions = new List<FunctionViewModel>();
            }
            return View(functions);
        }
    }
}