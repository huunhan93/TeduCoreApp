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
    public class FunctionController : BaseController
    {
        private readonly IFunctionService _functionService;
        public FunctionController(IFunctionService functionService)
        {
            _functionService = functionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Get Data API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _functionService.GetAll(string.Empty);
            var rootFunctions = model.Where(c => c.ParentId == null);
            var items = new List<FunctionViewModel>();
            foreach (var function in rootFunctions)
            {
                //add the parent category to the item list
                items.Add(function);
                //now get all its children (separate Category in case you need recursion)
                GetByParentId(model.ToList(), function, items);
            }
            return new ObjectResult(items);
        }

        [HttpGet]
        public IActionResult GetById(string id)
        {
            var model = _functionService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(FunctionViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                //productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
                if (productVm.Id == null)
                {
                    _functionService.Add(productVm);
                }
                else
                {
                    _functionService.Update(productVm);
                }
                _functionService.Save();
                return new OkObjectResult(productVm);

            }
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _functionService.Delete(id);
                _functionService.Save();
                return new OkObjectResult(id);
            }

        }



        #endregion

        #region Private Functions
        private void GetByParentId(IEnumerable<FunctionViewModel> allFunctions,
            FunctionViewModel parent, IList<FunctionViewModel> items)
        {
            var functionsEntities = allFunctions as FunctionViewModel[] ?? allFunctions.ToArray();
            var subFunctions = functionsEntities.Where(c => c.ParentId == parent.Id);
            foreach (var cat in subFunctions)
            {
                //add this category
                items.Add(cat);
                //recursive call in case your have a hierarchy more than 1 level deep
                GetByParentId(functionsEntities, cat, items);
            }
        }
        #endregion
    }
}