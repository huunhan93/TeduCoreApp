﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Utilities.Extensions;
using TeduCoreApp.Utilities.Helpers;

namespace TeduCoreApp.Areas.Admin.Controllers
{
    public class BillController : BaseController
    {
        private readonly IBillService _billService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public BillController(IBillService billService, IHostingEnvironment hostingEnvironmet)
        {
            _billService = billService;
            _hostingEnvironment = hostingEnvironmet;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GetById
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _billService.GetDetail(id);
            return new OkObjectResult(model);
        }
        // UpdateStatus
        [HttpGet]
        public IActionResult UpdateStatus(int billId, BillStatus status)
        {
            _billService.UpdateStatus(billId, status);
            return new OkResult();
        }

        // GetAppPaging
        [HttpGet]
        public IActionResult GetAllPaging(string startDate, string endDate, string keyword, int pageIndex, int pageSize)
        {
            var model = _billService.GetAllPaging(startDate, endDate, keyword, pageIndex, pageSize);
            return new OkObjectResult(model);
        }

        // SaveEntity
        [HttpPost]
        public IActionResult SaveEntity(BillViewModel billVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (billVm.Id == 0)
            {
                _billService.Create(billVm);
            }
            else
            {
                _billService.Update(billVm);
            }
            _billService.Save();

            return new OkObjectResult(billVm);
        }

        // GetPaymentMethod
        [HttpGet]
        public IActionResult GetPaymentMethod()
        {
            List<EnumModel> enums = ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return new OkObjectResult(enums);
        }

        // GetBillStatus
        [HttpGet]
        public IActionResult GetBillStatus()
        {
            List<EnumModel> enums = ((BillStatus[])Enum.GetValues(typeof(BillStatus)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return new OkObjectResult(enums);
        }

        // GetColors
        [HttpGet]
        public IActionResult GetColors()
        {
            var model = _billService.GetColors();
            return new OkObjectResult(model);
        }

        // GetSizes
        [HttpGet]
        public IActionResult GetSizes()
        {
            var model = _billService.GetSizes();
            return new OkObjectResult(model);
        }
        // ExportExcel
        [HttpPost]
        public IActionResult ExportExcel(int billId)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = $"Bills_{billId}.xlsx";
            // Template File
            string templateDocument = Path.Combine(sWebRootFolder, "templates", "BillTemplate.xlsx");
            string fileUrl = $"{Request.Scheme}://{Request.Host}/{"export-files"}/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, "export-files", sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            
            using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["TEDUOrder"];
                    // Data Access, load order header data.
                    var billDetail = _billService.GetDetail(billId);

                    // Insert customer data into template
                    worksheet.Cells[4, 1].Value = "Customer Name: " + billDetail.CustomerName;
                    worksheet.Cells[5, 1].Value = "Address: " + billDetail.CustomerAddress;
                    worksheet.Cells[6, 1].Value = "Phone: " + billDetail.CustomerMobile;
                    // Start Row for Detail Rows
                    int rowIndex = 9;

                    // Load order details
                    var orderDetails =  _billService.GetBillDetails(billId);
                    int count = 1;
                    foreach (var orderDetail in orderDetails)
                    {
                        // Cell 1, Carton Count
                        worksheet.Cells[rowIndex, 1].Value = count.ToString();
                        // Cell 2, Product Name
                        worksheet.Cells[rowIndex, 2].Value = orderDetail.Product.Name;
                        // Cell 3, Quantity
                        worksheet.Cells[rowIndex, 3].Value = orderDetail.Quantity.ToString();
                        // Cell 4, Price
                        worksheet.Cells[rowIndex, 4].Value = orderDetail.Price.ToString("N0");
                        // Cell 5, Amount
                        worksheet.Cells[rowIndex, 3].Value = (orderDetail.Price * orderDetail.Quantity).ToString("N0");
                        // Increment Row Counter
                        rowIndex++;
                        count++;
                    }
                    decimal total = (decimal)(orderDetails.Sum(x => x.Quantity * x.Price));
                    worksheet.Cells[24, 5].Value = total.ToString("N0");

                    var numberWord = "Total amount (by word): " + TextHelper.ToString(total);
                    worksheet.Cells[26, 1].Value = numberWord;
                    var billDate = billDetail.DateCreated;
                    worksheet.Cells[28, 3].Value = billDate.Day + ", " + billDate.Month + ", " + billDate.Year;

                    //worksheet.Cells.AutoFitColumns();
                    package.SaveAs(file); //Save the workbook
                }
            }
            
            return new OkObjectResult(fileUrl);
        }

    }
}