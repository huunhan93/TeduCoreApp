var loginController = function () {
    this.initialize = function () {
        registerEvents();
    };

    var registerEvents = function () {
        $('#frmLogin').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                userName: {
                    required: true
                },
                password: {
                    required: true
                }
            }
        });
        $('#btnLogin').on('click', function (e) {
            e.preventDefault();
            if ($('#frmLogin').valid()) {
                var user = $('#txtUserName').val();
                var pass = $('#txtPassword').val();
                login(user, pass);
            }
            
        });
        $('#txtPassword').on('keypress', function (e) {
            if (e.which === 13) {
                $('#btnLogin').click();
            }
        });
    };

    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                UserName: user,
                Password: pass
            },
            dateType: 'json',
            url: '/admin/login/authen',
            success: function (res) {
                if (res.Success) {
                    window.location.href = "/Admin/Home/Index";
                } else {
                    tedu.notify('Đăng nhập không đúng', 'error');
                }
            }
        });
    };
};