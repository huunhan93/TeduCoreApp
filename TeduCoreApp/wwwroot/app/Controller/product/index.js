var productController = function () {

    var quantityObj = new QuantityManagement();
    var imageObj = new ImageManagement();
    var wholeObj = new WholePriceManagement();

    this.initialize = function () {
        loadData();
        loadCategories();
        registerEvents();
        registerControls();
        quantityObj.initialize();
        imageObj.initialize();
        wholeObj.initialize();
    };

    function registerEvents() {
        //TODO: binding events to controls
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true },
                ddlCategoryIdM: { required: true },
                txtPriceM: {
                    required: true,
                    number: true
                }
            }
        });

        $('#btnCreate').on('click', function () {
            resetFormMaintainance();
            initTreeDropDownCategory();
            $('#modal-add-edit').modal('show');
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            loadData(true);
        });

        $('#btnSearch').on('click', function () {
            loadData();
        });

        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadData();
            }
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadDetails(that);
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            deleteProduct(that);
        });

        $('#btnSave').on('click', function (e) {
            e.preventDefault();
            saveProduct();
        });

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $('#fileInputImage').on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    $('#txtImage').val(path);
                    tedu.notify('Upload image succesfull', 'success');
                },
                error: function () {
                    tedu.notify('There was error uploading files', 'error');
                }
            });
        });

        $('#btn-import').on('click', function () {
            initTreeDropDownCategory();
            $('#modal-import-excel').modal('show');
        });

        $('#btn-export').on('click', function () {
            $.ajax({
                url: "/Admin/Product/ExportExcel",
                type: "POST",
                beforeSend: function () {
                    tedu.startLoading();
                },
                success: function (response) {
                    window.location.href = response;
                    tedu.stopLoading();
                },
                error: function () {
                    tedu.notify("Has an error in progress", 'error');
                    tedu.stopLoading();
                }
            });
        });

        $('#btnImportExcel').on('click', function () {
            var fileUpload = $('#fileInputExcel').get(0);
            var files = fileUpload.files;

            //Create FormData object
            var fileData = new FormData();
            // Looping over all files and add to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            // Adding one more key to FormData object
            fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));
            $.ajax({
                url: "/Admin/Product/ImportExcel",
                type: "POST",
                data: fileData,
                processData: false, // tell jQuery not to process the data
                contentType: false, // tell jQuery not to set contentType
                success: function (data) {
                    $('#modal-import-excel').modal('hide');
                    loadData();
                }
            });
            return false;
        });
    }

    function registerControls() {
        CKEDITOR.replace('txtContentM', {});

        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    }

    function resetFormMaintainance(){
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');

        $('#txtDescM').val('');
        $('#txtUnitM').val('');

        $('#txtPriceM').val('0');
        $('#txtOriginalPriceM').val('');
        $('#txtPromotionPriceM').val('');

        //$('#txtImageM').val('');

        $('#txtTagM').val('');
        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');

        CKEDITOR.instances.txtContentM.setData('');
        $('#ckStatusM').prop('checked', true);
        $('#ckHotM').prop('checked', false);
        $('#ckShowHomeM').prop('checked', false);
    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/ProductCategory/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = tedu.unflattern(data);
                $('#ddlCategoryIdM').combotree({
                    data: arr
                });
                $('#ddlCategoryIdImportExcel').combotree({
                    data: arr
                });
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    }

    function saveProduct() {
        if ($('#frmMaintainance').valid()) {
            var id = $('#hidIdM').val();
            var name = $('#txtNameM').val();
            var categoryId = $('#ddlCategoryIdM').combotree('getValue');

            var description = $('#txtDescM').val();
            var unit = $('#txtUnitM').val();

            var price = $('#txtPriceM').val();
            var originalPrice = $('#txtOriginalPriceM').val();
            var promotionPrice = $('#txtPromotionPriceM').val();

            //var image = $('#txtImageM').val();

            var tags = $('#txtTagM').val();
            var seoKeyword = $('#txtMetakeywordM').val();
            var seoMetaDescription = $('#txtMetaDescriptionM').val();
            var seoPageTitle = $('#txtSeoPageTitleM').val();
            var seoAlias = $('#txtSeoAliasM').val();

            var content = CKEDITOR.instances.txtContentM.getData();
            var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
            var hot = $('#ckHotM').prop('checked');
            var showHome = $('#ckShowHomeM').prop('checked');

            $.ajax({
                type: "POST",
                url: "/Admin/Product/SaveEntity",
                data: {
                    Id: id,
                    Name: name,
                    CategoryId: categoryId,
                    Image: '',
                    Price: price,
                    OriginalPrice: originalPrice,
                    PromotionPrice: promotionPrice,
                    Description: description,
                    Content: content,
                    HomeFlag: showHome,
                    HotFlag: hot,
                    Tags: tags,
                    Unit: unit,
                    Status: status,
                    SeoPageTitle: seoPageTitle,
                    SeoAlias: seoAlias,
                    SeoKeywords: seoKeyword,
                    SeoDescription: seoMetaDescription
                },
                dataType: "json",
                beforeSend: function () {
                    tedu.startLoading();
                },
                success: function (response) {
                    tedu.notify('Update product successful', 'success');
                    $('#modal-add-edit').modal('hide');
                    resetFormMaintainance();

                    tedu.stopLoading();
                    loadData(true);
                },
                error: function () {
                    tedu.notify('Has an error in save product progress', 'error');
                    tedu.stopLoading();
                }
            });
            return false;
        }
    }

    function loadDetails(id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Product/GetById",
            data: { id: id },
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidIdM').val(data.Id);
                $('#txtNameM').val(data.Name);
                initTreeDropDownCategory(data.CategoryId);

                $('#txtDescM').val(data.Description);
                $('#txtUnitM').val(data.Unit);

                $('#txtPriceM').val(data.Price);
                $('#txtOriginalPriceM').val(data.OriginalPrice);
                $('#txtPromotionPriceM').val(data.PromotionPrice);

                // $('#txtImageM').val(data.ThumbnailImage);

                $('#txtTagM').val(data.Tags);
                $('#txtMetakeywordM').val(data.SeoKeywords);
                $('#txtMetaDescriptionM').val(data.SeoDescription);
                $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                $('#txtSeoAliasM').val(data.SeoAlias);

                CKEDITOR.instances.txtContentM.setData(data.Content);
                $('#ckStatusM').prop('checked', data.Status === 1);
                $('#ckHotM').prop('checked', data.HotFlag);
                $('#ckShowHomeM').prop('checked', data.HomeFlag);

                $('#modal-add-edit').modal('show');
                tedu.stopLoading();

            },
            error: function (status) {
                tedu.notify('Có lỗi xảy ra', 'error');
                tedu.stopLoading();
            }
        });
    }

    function deleteProduct(id){
        tedu.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "GET",
                url: "/Admin/Product/Delete",
                data: { id: id },
                dataType: "json",
                beforeSend: function () {
                    tedu.startLoading();
                },
                success: function (response) {
                    tedu.notify('Delete successful', 'success');
                    tedu.stopLoading();
                    loadData();
                },
                error: function (status) {
                    tedu.notify('Has an error in delete progress', 'error');
                    tedu.stopLoading();
                }
            });
        });
    }

    function loadCategories(){
        $.ajax({
            type: 'GET',
            dateType: 'json',
            url: '/admin/product/GetAllCategories',
            success: function (response) {
                var render = '<option value="">--Select category--</option>';
                $.each(response, function (i, item) {
                    render += "<option value= '" + item.Id + "' >" + item.Name + "</option>";
                });
                $('#ddlCategorySearch').html(render);
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot loading product category data', 'error');
            }
        });
    }

    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = '';
        $.ajax({
            type: 'GET',
            data: {
                categoryId: $('#ddlCategorySearch').val(),
                keyword: $('#txtKeyword').val(),
                page: tedu.configs.pageIndex,
                pageSize: tedu.configs.pageSize
            },
            dateType: 'json',
            url: '/admin/product/GetAllPaging',
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {

                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Image: item.Image === null ? '<img src="/admin-site/images.user.png" style="width=25" /> ' : '<img src="' + item.Image +' style="width=25" />',
                        CategoryName: item.ProductCategory.Name,
                        Price: tedu.formatNumber(item.Price, 0),
                        CreatedDate: tedu.dateTimeFormatJson(item.DateCreated),
                        Status: tedu.getStatus(item.Status)
                    });
                    
                });

                $('#lblTotalRecords').text(response.RowCount);

                if (render !== '') {
                    $('#tbl-content').html(render);
                }
                wrapPaging(response.RowCount, function () {
                    loadData();
                }, isPageChanged);
                tedu.stopLoading();
                //console.log("Load success");
            },
            error: function(status){
                console.log(status);
                tedu.notify('Cannot loading data', 'error');
                tedu.stopLoading();
            }
        });
    }

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / tedu.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind pagination event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                if (tedu.configs.pageIndex !== p) {
                    tedu.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            }
        });
    }
};