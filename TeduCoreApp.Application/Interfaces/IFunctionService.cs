using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeduCoreApp.Application.ViewModels.System;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IFunctionService : IDisposable
    {
        void Add(FunctionViewModel functionVm);

        Task<List<FunctionViewModel>> GetAll();

        Task<List<FunctionViewModel>> GetAll(string filter);

        IEnumerable<FunctionViewModel> GatAllWithParentId(string parentId);

        FunctionViewModel GetById(string id);

        void Update(FunctionViewModel functionVm);

        void Delete(string id);

        bool CheckExistedId(string id);

        void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items);

        void ReOrder(string sourceId, string targetId);

        void Save();
    }
}
