function getVirtDir() {
    var Path = location.host;
    var VirtualDirectory;
    if (Path.indexOf("localhost") >= 0 && Path.indexOf(":") >= 0) {
        VirtualDirectory = "";

    }
    else {
        var pathname = window.location.pathname;
        var VirtualDir = pathname.split('/');
        VirtualDirectory = VirtualDir[1];
        VirtualDirectory = '/' + VirtualDirectory;
    }
    return VirtualDirectory;
}


function block() {
    $.blockUI({
        css: {
            baseZ: 150000,
            border: 'none',
            padding: '15px',
            backgroundColor: 'transparent',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: 'black'
        },
        message: "<div style='padding: 15px'><img src='" + getVirtDir() + "/images/busy.gif' style='width: 60px;' /> Quering serial...<br><label id='loadW'></label></div>"
    });
}

function unblock() {
    $.unblockUI();
}

function alertE(mensaje) {
    var dialog = BootstrapDialog.alert({
        message: mensaje,
        type: BootstrapDialog.TYPE_DANGER, 
        closable: false, 
        draggable: true, 
        buttonLabel: 'Close' 

    });
    setTimeout(function () {
        dialog.close();
        $("#fldPCBSerial").val("");
        $("#fldPCBSerial").focus();
    }, 3000);
}
function alertS(mensaje) {
    var dialog = BootstrapDialog.alert({
        message: mensaje,
        type: BootstrapDialog.TYPE_SUCCESS, 
        closable: false, 
        draggable: true, 
        buttonLabel: 'Close' 

    });
    setTimeout(function () {
        dialog.close();
        $("#fldPCBSerial").val("");
        $("#fldPCBSerial").focus();
    }, 3000);
}
function getDlgLogin() {
    $.ajax({
        method: "GET",
        url: getVirtDir() + "/Account/Login",
        success: function (data) {
            $("#divLogin").html(data);
            return false;
        },
        error: function () { }
    });
}

function getMainView() {
    $.ajax({
        method: "GET",
        url: getVirtDir() + "/stockTake",
        success: function (data) {
            $("#mainView").html(data);
            $("#fldPCBSerial").focus();
            return false;
        },
        error: function () { }
    });
}

function getMainViewMI() {
    $.ajax({
        method: "GET",
        url: getVirtDir() + "/stockTake",
        success: function (data) {
            $("#mainView").html(data);
            $("#fldPCBSerial").focus();
            return false;
        },
        error: function () { }
    });
}
function getScanView() {

    BootstrapDialog.alert({
        title: 'Final Scanning...',
        message: "<div class='window-content'> " +
            "<div class= 'form-horizontal' >" +
                "<div class='form-group' style='width:100%;margin-left:auto;'>" +
                    "<label class='col-sm-4' style='margin-top:10px;'>Scan stocktake label:</label>" +
                    "<div class='col-sm-8'>" + 
                    "    <input id='fldFinalStockTakeLabel' type='text' class='form-control' placeholder='Label code' onkeyup='" + "if (validateEnter(event)){updateChecked($(\"#fldFinalStockTakeLabel\").val());}" + "'/>" +
                    "</div>"+
                "</div>" +
            "</div>" +
            "<div id='divSavedRecord'></div>" +
            "</div>",
        type: BootstrapDialog.TYPE_SUCCESS,
        closable: false, 
        draggable: true, 
        buttonLabel: 'Close' 
    });
}

function getScanViewMI() {

    BootstrapDialog.alert({
        title: 'Final Scanning...',
        message: "<div class='window-content'> " +
            "<div class= 'form-horizontal' >" +
            "<div class='form-group' style='width:100%;margin-left:auto;'>" +
            "<label class='col-sm-4' style='margin-top:10px;'>Scan magazine label:</label>" +
            "<div class='col-sm-8'>" +
            "    <input id='fldFinalStockTakeLabel' type='text' class='form-control' placeholder='Label code' onkeyup='" + "if (validateEnter(event)){updateCheckedMI($(\"#fldFinalStockTakeLabel\").val());}" + "'/>" +
            "</div>" +
            "</div>" +
            "</div>" +
            "<div id='divSavedRecord'></div>" +
            "</div>",
        type: BootstrapDialog.TYPE_SUCCESS,
        closable: false, 
        draggable: true, 
        buttonLabel: 'Close' 
    });
}

function validateEnter(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla === 13) {
        return true;
    } else {
        return false;
    }
}
function loadrptStk(date) {
    block();
    $.ajax({
        url: getVirtDir() + "/Controllers/getReport.ashx",
        type: "POST",
        data: { date: date, dir: getVirtDir() },
        success: function (data) {
            var r = jQuery.parseJSON(data);
            if (r.result === "true") {
                $("#viewRpt").html(r.pdf);
            }
            else {
                alertE("Ocurrio un error: " + r.messageError);
            }
            unblock();
        },
        error: function () {

        }
    });
}
