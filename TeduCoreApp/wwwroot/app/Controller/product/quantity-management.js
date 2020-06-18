var QuantityManagement = function () {
    var cachedObj = {
        colors: [],
        sizes: []
    };

    this.initialize = function () {
        loadColors();
        loadSizes();
        registerEvents();
    };
    
    function registerEvents() {
        $('body').on('click', '.btn-quantity', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hidId').val(that);
            loadQuantities();
            $('#modal-quantity-management').modal('show');
        });

        $('#btn-add-quantity').on('click', function () {
            var template = $('#template-table-quantity').html();
            var render = Mustache.render(template, {
                Id: 0,
                Colors: getColorOptions(null),
                Sizes: getSizeOptions(null),
                Quantity: 0
            });
            $('#tbl-quantity-content').append(render);
        });

        $("#btnSaveQuantity").on("click", function () {
            var quantityList = [];
            $.each($('#tbl-quantity-content').find('tr'), function (i, item) {
                quantityList.push({
                    Id: $(item).data('id'),
                    ProductId: $("#hidId").val(),
                    Quantity: $(item).find('input.txtQuantity').first().val(),
                    SizeId: $(item).find('select.ddlSizeId').first().val(),
                    ColorId: $(item).find('select.ddlColorId').first().val()
                });
            });

            $.ajax({
                url: "/Admin/Product/SaveQuantities",
                data: {
                    productId: $("#hidId").val(),
                    quantities: quantityList
                },
                type: "post",
                dataType: "json",
                success: function (response) {
                    $("#modal-quantity-management").modal("hide");
                    $("tbl-quantity-content").html();
                    tedu.notify('Save quantity success', 'success');
                }
            });
        });

        $("body").on("click", ".btn-delete-quantity", function (e) {
            $(this).parent().parent().remove();
        });
    }

    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetColors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                tedu.notify('Has load color an error in progress', 'error');
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetSizes",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                tedu.notify('Has load size an error in progress', 'error');
            }
        });
    }

    function loadQuantities() {
        var template = $('#template-table-quantity').html();
        var render = '';
        $.ajax({
            type: 'GET',
            data: {
                productId: $('#hidId').val()
            },
            dateType: 'json',
            url: '/admin/product/GetQuantities',
            success: function (response) {
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Colors: getColorOptions(item.ColorId),
                        Sizes: getSizeOptions(item.SizeId),
                        Quantity: item.Quantity
                    });
                    
                });
                $('#tbl-quantity-content').html(render);
            }
        });
    }

    function getColorOptions(selectedId) {
        var colors = "<select class='form-control ddlColorId'>";
        $.each(cachedObj.colors, function (i, color) {
            if (selectedId === color.Id) {
                colors += '<option value="' + color.Id + '" selected="select">' + color.Name + '</option>';
            } else {
                colors += '<option value="' + color.Id + '">' + color.Name + '</option>';
            }
        });
        colors += '</select>';
        return colors;
    }

    function getSizeOptions(selectedId) {
        var sizes = "<select class='form-control ddlSizeId'>";
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === size.Id) {
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Name + '</option>';
            } else {
                sizes += '<option value="' + size.Id + '">' + size.Name + '</option>';
            }
        });
        sizes += '</select>';
        return sizes;
    }
};