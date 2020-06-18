using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.Infrastructure.Interfaces;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Implementation
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _orderRepository;
        private readonly IBillDetailRepository _orderDetailRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BillService(IBillRepository orderRepository, IBillDetailRepository orderDetailRepository,
            IColorRepository colorRepository, ISizeRepository sizeRepository, 
            IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        public void Create(BillViewModel billVm)
        {
            var order = Mapper.Map<BillViewModel, Bill>(billVm);
            var orderDetails = Mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billVm.BillDetails);
            foreach (var detail in orderDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
            }
            order.BillDetails = orderDetails;
            _orderRepository.Add(order);
        }

        public BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm)
        {
            var billDetail = Mapper.Map<BillDetailViewModel, BillDetail>(billDetailVm);
            _orderDetailRepository.Add(billDetail);
            return billDetailVm;
        }

        public void DeleteDetail(int productId, int billId, int colorId, int sizeId)
        {
            var bill = _orderDetailRepository.FindSingle(x => x.ProductId == productId && x.BillId == billId
            && x.ColorId == colorId && x.SizeId == sizeId);
            _orderDetailRepository.Remove(bill);
        }

        public PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword, int pageIndex, int pageSize)
        {
            var query = _orderRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated >= start);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated >= end);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.CustomerName.Contains(keyword) 
                || x.CustomerMobile.Contains(keyword));
            }
            

            int totalRow = query.Count();

            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<BillViewModel>()
                .ToList();

            var paginationSet = new PagedResult<BillViewModel>()
            {
                Results = data,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public List<BillDetailViewModel> GetBillDetails(int billId)
        {
            return _orderDetailRepository
                .FindAll(x => x.BillId == billId, c => c.Bill, c => c.Color, c => c.Size, c => c.Product)
                .ProjectTo<BillDetailViewModel>().ToList();
        }

        public List<ColorViewModel> GetColors()
        {
            return _colorRepository.FindAll().ProjectTo<ColorViewModel>().ToList();
        }

        public BillViewModel GetDetail(int billId)
        {
            var bill = _orderRepository.FindSingle(x => x.Id == billId);
            var billVm = Mapper.Map<Bill, BillViewModel>(bill);
            var billDetailVm = _orderDetailRepository.FindAll(x => x.BillId == billId);
            billVm.BillDetails = billDetailVm.ProjectTo<BillDetailViewModel>().ToList();
            return billVm;
        }
        
        public List<SizeViewModel> GetSizes()
        {
            return _sizeRepository.FindAll().ProjectTo<SizeViewModel>().ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(BillViewModel billVm)
        {
            //Mapping to order domain
            var order = Mapper.Map<BillViewModel, Bill>(billVm);

            // Get order Detail 
            var newDetails = order.BillDetails;

            // new details added
            var addDetails = newDetails.Where(x => x.Id == 0).ToList();

            // get updated details
            var updateDetails = newDetails.Where(x => x.Id != 0).ToList();

            // Existed details
            var existedDetails = _orderDetailRepository.FindAll(x => x.BillId == billVm.Id);

            // Clear db
            order.BillDetails.Clear();

            foreach (var detail in updateDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _orderDetailRepository.Update(detail);
            }

            foreach (var detail in addDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _orderDetailRepository.Add(detail);
            }

            _orderDetailRepository.RemoveMultiple(existedDetails.Except(updateDetails).ToList());

            _orderRepository.Update(order);

        }

        public void UpdateStatus(int orderId, BillStatus status)
        {
            var order = _orderRepository.FindById(orderId);
            order.BillStatus = status;
            _orderRepository.Update(order);
        }
    }
}
