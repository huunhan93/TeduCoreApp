using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Blog;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.Infrastructure.Interfaces;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Implementation
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BlogService(IBlogRepository blogRepository,
            IBlogTagRepository blogTagRepository,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork)
        {
            _blogRepository = blogRepository;
            _blogTagRepository = blogTagRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public BlogViewModel Add(BlogViewModel product)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<BlogViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public PagedResult<BlogViewModel> GetAllPaging(string keyword, int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public BlogViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<BlogViewModel> GetHotProduct(int top)
        {
            throw new NotImplementedException();
        }

        public List<BlogViewModel> GetLastest(int top)
        {
            return _blogRepository.FindAll(x => x.Status == Status.Active)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)
                .ProjectTo<BlogViewModel>()
                .ToList();
        }

        public List<BlogViewModel> GetList(string keyword)
        {
            throw new NotImplementedException();
        }

        public List<string> GetListByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<BlogViewModel> GetListByTag(string tagId, int page, int pagesize, out int totalRow)
        {
            throw new NotImplementedException();
        }

        public List<BlogViewModel> GetListPaging(int page, int pageSize, string sort, out int totalRow)
        {
            throw new NotImplementedException();
        }

        public List<TagViewModel> GetListTag(string searchText)
        {
            throw new NotImplementedException();
        }

        public List<TagViewModel> GetListTagById(int id)
        {
            throw new NotImplementedException();
        }

        public List<BlogViewModel> GetReatedBlogs(int id, int top)
        {
            throw new NotImplementedException();
        }

        public TagViewModel GetTag(string tagId)
        {
            throw new NotImplementedException();
        }

        public void IncreaseView(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public List<BlogViewModel> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            throw new NotImplementedException();
        }

        public void Update(BlogViewModel product)
        {
            throw new NotImplementedException();
        }
    }
}
