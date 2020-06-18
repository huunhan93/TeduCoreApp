using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.Infrastructure.Interfaces;

namespace TeduCoreApp.Application.Implementation
{
    public class FunctionService : IFunctionService
    {
        IFunctionRepository _functionRepository;
        IUnitOfWork _unitOfWork;
        public FunctionService(IFunctionRepository functionRepository, IUnitOfWork unitOfWork)
        {
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(FunctionViewModel functionVm)
        {
            var function = Mapper.Map<FunctionViewModel, Function>(functionVm);
            _functionRepository.Add(function);
        }

        public bool CheckExistedId(string id)
        {
            return _functionRepository.FindById(id) != null;
        }

        public void Delete(string id)
        {
            _functionRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IEnumerable<FunctionViewModel> GatAllWithParentId(string parentId)
        {
            return _functionRepository.FindAll().Where(x => x.ParentId == parentId).ProjectTo<FunctionViewModel>();
        }

        public Task<List<FunctionViewModel>> GetAll()
        {
            return _functionRepository.FindAll().ProjectTo<FunctionViewModel>().ToListAsync();
        }

        public Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter));
            }
            return query.OrderBy(x => x.ParentId).ProjectTo<FunctionViewModel>().ToListAsync();
        }

        public FunctionViewModel GetById(string id)
        {
            var functionVm = _functionRepository.FindSingle(x => x.Id == id);
            return Mapper.Map<Function, FunctionViewModel>(functionVm);
        }

        public void ReOrder(string sourceId, string targetId)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(FunctionViewModel functionVm)
        {
            throw new NotImplementedException();
        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            throw new NotImplementedException();
        }
    }
}
