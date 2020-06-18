using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Data.EF.Repositories;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.Utilities.Constants;

namespace TeduCoreApp.Application.Implementation
{
    public class CommonService : ICommonService
    {
        private readonly IFooterRepository _footerRepository;
        private readonly ISlideRepository _slideRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
        public CommonService(IFooterRepository footerRepository, ISlideRepository slideRepository,
            ISystemConfigRepository systemConfigRepository)
        {
            _footerRepository = footerRepository;
            _slideRepository = slideRepository;
            _systemConfigRepository = systemConfigRepository;
        }
        public FooterViewModel GetFooter()
        {
            Footer footer = _footerRepository.FindSingle(x => x.Id == CommonConstants.DefaultFooterId);
            return Mapper.Map<Footer, FooterViewModel>(footer);
        }

        public List<SlideViewModel> GetSlides(string groupAlias)
        {
            return _slideRepository.FindAll(x=> x.Status && x.GroupAlias == groupAlias)
                .ProjectTo<SlideViewModel>().ToList();
        }

        public SystemConfigViewModel GetSystemConfig(string code)
        {
            SystemConfig systemConfig = _systemConfigRepository.FindSingle(x => x.Id == code);
            return Mapper.Map<SystemConfig, SystemConfigViewModel>(systemConfig);
        }
    }
}
