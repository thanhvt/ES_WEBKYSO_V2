function loadjscssfile(filename, filetype) {
    if (filetype == "js") { //if filename is a external JavaScript file
        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", filename);
    }
    else if (filetype == "css") { //if filename is an external CSS file
        var fileref = document.createElement("link");
        fileref.setAttribute("rel", "stylesheet");
        fileref.setAttribute("type", "text/css");
        fileref.setAttribute("href", filename);
    }
    if (typeof fileref != "undefined")
        document.getElementsByTagName("head")[0].appendChild(fileref);
}

loadjscssfile("/Scripts/esSignatureFunctions.js", "js");//dynamically load and add this .js file

//b1 tạo hàm js
//B2 gắn hàm trên nút duyệt tại _QlyHoSo_Chung
//b3 gắn hàm callapp trên trang cshtml 
//b4 tạo Controller: truyền hàm "khi xong thì gọi lại"
//b5 tạo hàm "khi xong thì gọi lại" tại service. //Luu y:khi  thay User.Identity.Name = cad.UserSign


function leaveChange() {
    try {
        var ttthu = document.getElementById("cboTinhTrangThu").value;
        var hoso_id = document.getElementById("gethosoid").value;
        $("#CapNhat").attr("href", "/Home/ChuyenHoSo_NVKS?ID=" + hoso_id + "&TinhTrangThu=" + ttthu);
        $("#kySo").attr("href", "/Home/ChuyenHoSo_NVKS?ID=" + hoso_id + "&callApp=true" + "&TinhTrangThu=" + ttthu);
    } catch (e) {
        alert(e);
    } 
    
}

function showSignFormXacNhanChuyenDoiTruong(e) {
    try {
        document.getElementById("gethosoid").value = e;
        $("#CapNhat").attr("href", "/Home/ChuyenHoSo_NVKS?ID=" + e);
        $("#kySo").attr("href", "/Home/ChuyenHoSo_NVKS?ID=" + e + "&callApp=true");
        $('#myModal').modal('show');
        $('#formTTThu').css('visibility', 'visible');
    } catch (ex) {
        alert(ex);
    }
}

function showSignFormDoiTruongChuyenKiemDuyet(e) {
    try {
        $("#CapNhat").attr("href", "/Home/ChuyenHoSo_DoiTruong?hosoID=" + e);
        $("#kySo").attr("href", "/Home/ChuyenHoSo_DoiTruong?hosoID=" + e + "&callApp=true");
        $('#pContent').text("Xác nhận chuyển cho kiểm duyệt?");
        $('#myModal').modal('show');
    } catch (ex) {
        alert(ex);
    }
}

function showSignFormKinhDoanhChuyenLanhDao(e) {
    try {
        $("#CapNhat").attr("href", "/Home/ChuyenHoSo_PKD?hosoID=" + e);
        $("#kySo").attr("href", "/Home/ChuyenHoSo_PKD?hosoID=" + e + "&callApp=true");
        $('#pContent').text("Xác nhận chuyển lãnh đạo?");
        $('#myModal').modal('show');
    } catch (ex) {
        alert(ex);
    }
}

function showSignFormKinhDoanhChuyenLanhDao_istarget(e) {
    try {
        $("#CapNhat").attr("href", "/Home/ChuyenHoSo_PKD_isTarget?hosoID=" + e);
        $("#kySo").attr("href", "/Home/ChuyenHoSo_PKD_isTarget?hosoID=" + e + "&callApp=true");
        $('#pContent').text("Xác nhận chuyển lãnh đạo?");
        $('#myModal').modal('show');
    } catch (ex) {
        alert(ex);
    }
}

function showSignFormLanhDaoKyPA(e) {
    try {
        $("#CapNhat").attr("href", "/Home/XacNhanHoSoChuyenPKD?hosoID=" + e);
        $("#kySo").attr("href", "/Home/XacNhanHoSoChuyenPKD?hosoID=" + e + "&callApp=true");
        $('#pContent').text("Xác nhận chuyển cho kiểm duyệt?");
        $('#myModal').modal('show');
    } catch (ex) {
        alert(ex);
    }
}

function showSignFormXNhanQToan(e) {
    try {
        $("#CapNhat").attr("href", "/Home/XacNhanQuyetToan_new?hosoID=" + e);
        $("#kySo").attr("href", "/Home/XacNhanQuyetToan_new?hosoID=" + e + "&callApp=true");
        $('#pContent').text("Xác nhận chuyển bộ phận duyệt?");
        $('#myModal').modal('show');
    } catch (ex) {
        alert(ex);
    }
}

function showSignFormDuyetQToan(e) {
    try {
        $("#CapNhat").attr("href", "/Home/DuyetQuyetToan?hosoID=" + e);
        $("#kySo").attr("href", "/Home/DuyetQuyetToan?hosoID=" + e + "&callApp=true");
        $('#pContent').text("Xác nhận, chuyển lãnh đạo duyệt?");
        $('#myModal').modal('show');
    } catch (ex) {
        alert(ex);
    }
}

function showSignFormLanhDaoKyQuyetToan(e) {
    try {
        $("#CapNhat").attr("href", "/Home/XacNhanKyQuyetToan?hosoID=" + e);
        $("#kySo").attr("href", "/Home/XacNhanKyQuyetToan?hosoID=" + e + "&callApp=true");
        $('#pContent').text("Duyệt quyết toán, Xác nhận chuyển lãnh đạo?");
        $('#myModal').modal('show');
    } catch (ex) {
        alert(ex);
    }
}

