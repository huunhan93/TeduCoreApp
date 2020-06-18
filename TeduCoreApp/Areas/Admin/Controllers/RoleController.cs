using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.System;

namespace TeduCoreApp.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            return View();
        }

        //GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();
            return new OkObjectResult(model);
        }

        //GetById
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var model = await _roleService.GetByIdAsync(id);
            return new OkObjectResult(model);
        }

        //GetAllPaging
        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _roleService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        //SaveEntity
        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (!roleVm.Id.HasValue)
                {
                    await _roleService.AddAsync(roleVm);
                }
                else
                {
                    await _roleService.UpdateAsync(roleVm);
                }
                return new OkObjectResult(roleVm);
            }
        }

        //Delete
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                await _roleService.DeleteAsync(id);

                return new OkObjectResult(id);
            }
        }

        [HttpPost]
        public IActionResult ListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }

        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> listPermission, Guid roleId)
        {
            _roleService.SavePermisstion(listPermission, roleId);
            return new OkResult();
        }
    }
}