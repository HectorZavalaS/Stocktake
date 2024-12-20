function queryItem(serial) {
    block();
    $.ajax({
        method: "POST",
        data: {serial : serial},
        url: getVirtDir() + "/Controllers/queryItem.ashx",
        async: false,
        success: function (data) {
            r = jQuery.parseJSON(data);
            if (r.result === "true") {
                unblock();
                var res = existDjGroup(r.batchid);
                if (res === "true") {
                    $("#fldScannedSerial").val(r.serial);
                    $("#fldDJGroup").val(r.batchid);
                    $("#fldCurrentOperation").val(r.operation);
                    $("#fldModelBin").val(r.partNumber);
                    $("#fldSemiFinish").val(r.semifinish);
                    $("#fldCogiscanRoute").val(r.route);
                    $("#fldOperationStatus").val(r.status);
                    $("#fldQuantity").focus();
                }
            }
            else {
                clearFields();
                alertE("<div class='alert alert-danger error' role='alert'>El número de serie: " + serial + " no existe!</div>");
            }
            unblock();
            return false;
        },
        error: function () { }
    })
}

function queryItems() {
    block();
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/queryItems.ashx",
        async: false,
        success: function (data) {
            r = jQuery.parseJSON(data);
            if (r.result === "true") {
                unblock();
                var res = existDjGroup(r.batchid);
                if (res === "true") {
                    $("#fldScannedSerial").val(r.serial);
                    $("#fldDJGroup").val(r.batchid);
                    $("#fldCurrentOperation").val(r.operation);
                    $("#fldModelBin").val(r.partNumber);
                    $("#fldSemiFinish").val(r.semifinish);
                    $("#fldCogiscanRoute").val(r.route);
                    $("#fldOperationStatus").val(r.status);
                    $("#fldQuantity").focus();
                }
            }
            else {
                clearFields();
                alertE("<div class='alert alert-danger error' role='alert'>El número de serie: " + serial + " no existe!</div>");
            }
            unblock();
            return false;
        },
        error: function () { }
    })
}
function updateSemi(fecha) {
    block();
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/processSemifinish.ashx",
        data: {fecha:fecha},
        success: function (data) {
            r = jQuery.parseJSON(data);
            if (r.result === "true") {
                unblock();
                alertS("<div class='alert alert-danger error' role='alert'>" + r.MessageSuccess + " </div>");
            }
            else {
                alertE("<div class='alert alert-danger error' role='alert'>" + r.MessageError + " </div>");
            }
            unblock();
            return false;
        },
        error: function () { }
    })
}

function existDjGroup(djG) {
    var result = "false";
    $.ajax({
        method: "POST",
        data: { djG: djG },
        url: getVirtDir() + "/Controllers/existDjGroup.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                result = "true";
            }
            else {
                clearFields();
                result = "false";
                alertE("<div class='alert alert-danger error' role='alert'>El número de Grupo: " + djG + " no existe! <br> No se puede utilizar la PCB para conteo.</div>");
            }

        },
        error: function () { }
    });
    return result;
}
function updateChecked(lblQr) {
    var result = "false";
    $.ajax({
        method: "POST",
        data: { lblQr: lblQr },
        url: getVirtDir() + "/Controllers/updateChecked.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                $("#divSavedRecord").html("<div class='alert alert-success' role='alert'>Saved successfully!</div>");
                $("#divSavedRecord").show();
                $("#fldFinalStockTakeLabel").val("");
                $("#fldFinalStockTakeLabel").focus();
                result = "true";
            }
            else {
                clearFields();
                result = "false";
                $("#divSavedRecord").html("<div class='alert alert-danger error' role='alert'>" + re.MessageError + "</div>");
                $("#divSavedRecord").show();
                $("#fldFinalStockTakeLabel").val("");
                $("#fldFinalStockTakeLabel").focus();
            }
            setTimeout(function () {
                $("#divSavedRecord").hide();
            }, 2000);

        },
        error: function () { }
    });
    return result;
}
function updateCheckedMI(lblQr) {
    var result = "false";
    $.ajax({
        method: "POST",
        data: { lblQr: lblQr },
        url: getVirtDir() + "/Controllers/updateCheckedMI.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                $("#divSavedRecord").html("<div class='alert alert-success' role='alert'>Saved successfully!</div>");
                $("#divSavedRecord").show();
                $("#fldFinalStockTakeLabel").val("");
                $("#fldFinalStockTakeLabel").focus();
                result = "true";
            }
            else {
                clearFields();
                result = "false";
                $("#divSavedRecord").html("<div class='alert alert-danger error' role='alert'>" + re.MessageError + "</div>");
                $("#divSavedRecord").show();
                $("#fldFinalStockTakeLabel").val("");
                $("#fldFinalStockTakeLabel").focus();
            }
            setTimeout(function () {
                $("#divSavedRecord").hide();
            }, 2000);

        },
        error: function () { }
    });
    return result;
}

function updateLabelInfoST(lblQr,qty) {
    var result = "false";
    $.ajax({
        method: "POST",
        data: { qr: lblQr, qty: qty },
        url: getVirtDir() + "/Controllers/updateLabelInfoST.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                $("#divQuantityStatus").html("<div class='alert alert-success' role='alert'>Saved successfully!</div>");
                $("#divQuantityStatus").show();
                $("#fldQuantity").val($("#fldNewQuantity").val());
                rePrintLabelST();
                $("#fldWrongLabel").val("");
                $("#fldNewQuantity").val("");
                $("#fldWrongLabel").focus();
                result = "true";
            }
            else {
                //clearFields();
                result = "false";
                $("#divQuantityStatus").html("<div class='alert alert-danger error' role='alert'>" + re.MessageError + "</div>");
                $("#divQuantityStatus").show();
                //$("#fldNewQuantity").val("");
                $("#fldNewQuantity").focus();
            }
            setTimeout(function () {
                $("#divQuantityStatus").hide();
            }, 2000);

        },
        error: function () { }
    });
    return result;
}

function updateLabelInfoMI(lblQr, qty) {
    var result = "false";
    $.ajax({
        method: "POST",
        data: { qr: lblQr, qty: qty },
        url: getVirtDir() + "/Controllers/updateLabelInfoMI.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                $("#divQuantityStatus").html("<div class='alert alert-success' role='alert'>Saved successfully!</div>");
                $("#divQuantityStatus").show();
                $("#fldQuantity").val($("#fldNewQuantity").val());
                rePrintLabelMI();
                $("#fldWrongLabel").val("");
                $("#fldNewQuantity").val("");
                $("#fldWrongLabel").focus();
                result = "true";
            }
            else {
                //clearFields();
                result = "false";
                $("#divQuantityStatus").html("<div class='alert alert-danger error' role='alert'>" + re.MessageError + "</div>");
                $("#divQuantityStatus").show();
                //$("#fldNewQuantity").val("");
                $("#fldNewQuantity").focus();
            }
            setTimeout(function () {
                $("#divQuantityStatus").hide();
            }, 2000);

        },
        error: function () { }
    });
    return result;
}

function existPrinterST() {
    var result = "false";
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/existPrinter.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                $("#slPrinter").html(re.html);
                result = "true";
            }
            else {
                result = "false";
                alertE("<div class='alert alert-danger error' role='alert'>No existe ninguna impresora TSC Alpha-3 instalada.</div>");
            }

        },
        error: function () { }
    });
    return result;
}
function getLabelInfoST(label) {
    var result = "false";
    $.ajax({
        method: "POST",
        data: { qr: label },
        url: getVirtDir() + "/Controllers/getLabelInfoST.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                $("#divQuantityStatus").html("<div class='alert alert-success' role='alert'>Get info successfully!</div>");
                $("#divQuantityStatus").show();
                $("#fldNewQuantity").val(re.qty);

                $("#fldScannedSerial").val(re.serial);
                $("#fldDJGroup").val(re.batchid);
                $("#fldCurrentOperation").val(re.operation);
                $("#fldModelBin").val(re.partNumber);
                $("#fldSemiFinish").val(re.semifinish);
                $("#fldCogiscanRoute").val(re.route);
                $("#fldOperationStatus").val(re.status);
                $("#fldQuantity").val(re.qty);
                $("#var2").val(re.mag);
                

                //$("#fldWrongLabel").val("");
                $("#fldNewQuantity").focus();
                result = "true";
            }
            else {
                clearFields();
                result = "false";
                $("#divQuantityStatus").html("<div class='alert alert-danger error' role='alert'>" + re.MessageError + "</div>");
                $("#divQuantityStatus").show();
                $("#fldWrongLabel").val("");
                $("#fldWrongLabel").focus();
            }
            setTimeout(function () {
                $("#divQuantityStatus").hide();
            }, 2000);

        },
        error: function () { }
    });
    return result;
}

function getLabelInfoMI(label) {
    var result = "false";
    $.ajax({
        method: "POST",
        data: { qr: label },
        url: getVirtDir() + "/Controllers/getLabelInfoMI.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                $("#divQuantityStatus").html("<div class='alert alert-success' role='alert'>Get info successfully!</div>");
                $("#divQuantityStatus").show();
                $("#fldNewQuantity").val(re.qty);

                $("#fldScannedSerial").val(re.serial);
                $("#fldDJGroup").val(re.batchid);
                $("#fldCurrentOperation").val(re.operation);
                $("#fldModelBin").val(re.partNumber);
                $("#fldSemiFinish").val(re.semifinish);
                $("#fldCogiscanRoute").val(re.route);
                $("#fldOperationStatus").val(re.status);
                $("#fldQuantity").val(re.qty);
                $("#var2").val(re.mag);


                //$("#fldWrongLabel").val("");
                $("#fldNewQuantity").focus();
                result = "true";
            }
            else {
                clearFields();
                result = "false";
                $("#divQuantityStatus").html("<div class='alert alert-danger error' role='alert'>" + re.MessageError + "</div>");
                $("#divQuantityStatus").show();
                $("#fldWrongLabel").val("");
                $("#fldWrongLabel").focus();
            }
            setTimeout(function () {
                $("#divQuantityStatus").hide();
            }, 2000);

        },
        error: function () { }
    });
    return result;
}
function clearFields() {
    $("#fldScannedSerial").val("");
    $("#fldDJGroup").val("");
    $("#fldCurrentOperation").val("");
    $("#fldModelBin").val("");
    $("#fldSemiFinish").val("");
    $("#fldCogiscanRoute").val("");
    $("#fldOperationStatus").val("");
}

function printLabel() {
    var result = "false";
    if ($("#fldScannedSerial").val() === "" || $("#fldScannedSerial").val() === "0") {
        alertE("<div class='alert alert-danger error' role='alert'>No has escaneado ningun serial!</div>");
        $("#fldQuantity").focus();
        return "false";
    }
    if ($("#fldQuantity").val() === "" || $("#fldQuantity").val() === "0") {
        alertE("<div class='alert alert-danger error' role='alert'>La cantidad no debe estar vacia ni ser 0!</div>");
        $("#fldQuantity").focus();
        return "false";
    }
    //if ($("#slPrinter option:selected").text()==="") {
    //    alertE("<div class='alert alert-danger error' role='alert'>No has seleccionado una impresora.</div>");
    //    $("#fldQuantity").focus();
    //    return "false";
    //}

    $.ajax({
        method: "POST",
        data: {
            dj_group: $("#fldDJGroup").val(), model: $("#fldModelBin").val(), semi: $("#fldSemiFinish").val(),
            qty: $("#fldQuantity").val(), cgs_route: $("#fldCogiscanRoute").val(), name_printer: $("#slPrinter option:selected").text(),
            serial: $("#fldScannedSerial").val(), idU: $("#var1").val()
        },
        url: getVirtDir() + "/Controllers/printLabel.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                alertS("<div class='alert alert-success' role='alert'>Se imprimio la etiqueta.</div>");
                clearFields();
                $("#fldQuantity").val("");
                result = "true";
            }
            else {
                result = "false";
                alertE("<div class='alert alert-danger error' role='alert'>" + re.MessageError + "</div>");
            }

        },
        error: function () { }
    });
}

function rePrintLabelST() {

    $.ajax({
        method: "POST",
        data: {
            dj_group: $("#fldDJGroup").val(), model: $("#fldModelBin").val(), semi: $("#fldSemiFinish").val(),
            qty: $("#fldQuantity").val(), cgs_route: $("#fldCogiscanRoute").val(), name_printer: $("#slPrinter option:selected").text(),
            serial: $("#fldScannedSerial").val(), idU: $("#var1").val(), mag: $("#var2").val()
        },
        url: getVirtDir() + "/Controllers/rePrintLabel.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                alertS("<div class='alert alert-success' role='alert'>Se imprimio la etiqueta.</div>");
                clearFields();
                $("#fldQuantity").val("");
                result = "true";
            }
            else {
                result = "false";
                alertE("<div class='alert alert-danger error' role='alert'>" + re.MessageError + "</div>");
            }

        },
        error: function () { }
    });
}

function printLabelMI() {
    var result = "false";
    if ($("#fldScannedSerial").val() === "" || $("#fldScannedSerial").val() === "0") {
        alertE("<div class='alert alert-danger error' role='alert'>No has escaneado ningun serial!</div>");
        $("#fldQuantity").focus();
        return "false";
    }
    if ($("#fldQuantity").val() === "" || $("#fldQuantity").val() === "0") {
        alertE("<div class='alert alert-danger error' role='alert'>La cantidad no debe estar vacia ni ser 0!</div>");
        $("#fldQuantity").focus();
        return "false";
    }
    if ($("#slPrinter option:selected").text() === "") {
        alertE("<div class='alert alert-danger error' role='alert'>No has seleccionado una impresora.</div>");
        $("#fldQuantity").focus();
        return "false";
    }

    $.ajax({
        method: "POST",
        data: {
            dj_group: $("#fldDJGroup").val(), model: $("#fldModelBin").val(), semi: $("#fldSemiFinish").val(),
            qty: $("#fldQuantity").val(), cgs_route: $("#fldCogiscanRoute").val(), name_printer: $("#slPrinter option:selected").text(),
            serial: $("#fldScannedSerial").val(), idU: $("#var1").val()
        },
        url: getVirtDir() + "/Controllers/printLabelMID.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                alertS("<div class='alert alert-success' role='alert'>Se imprimio la etiqueta.</div>");
                clearFields();
                $("#fldQuantity").val("");
                result = "true";
            }
            else {
                result = "false";
                alertE("<div class='alert alert-danger error' role='alert'>" + re.MessageError + "</div>");
            }

        },
        error: function () { }
    });
}
function rePrintLabelMI() {

    $.ajax({
        method: "POST",
        data: {
            dj_group: $("#fldDJGroup").val(), model: $("#fldModelBin").val(), semi: $("#fldSemiFinish").val(),
            qty: $("#fldQuantity").val(), cgs_route: $("#fldCogiscanRoute").val(), name_printer: $("#slPrinter option:selected").text(),
            serial: $("#fldScannedSerial").val(), idU: $("#var1").val(), mag: $("#var2").val()
        },
        url: getVirtDir() + "/Controllers/rePrintLabelMI.ashx",
        async: false,
        success: function (data) {
            re = jQuery.parseJSON(data);
            if (re.result === "true") {
                alertS("<div class='alert alert-success' role='alert'>Se imprimio la etiqueta.</div>");
                clearFields();
                $("#fldQuantity").val("");
                result = "true";
            }
            else {
                result = "false";
                alertE("<div class='alert alert-danger error' role='alert'>" + re.MessageError + "</div>");
            }

        },
        error: function () { }
    });
}
function fixLblQtyST() {
    BootstrapDialog.show({
        title: 'Fix Label Qty',
        message: "<div class='window-content'>"
            + "<form id='reprintLabelForm' name ='reprintLabel' class='form-horizontal' method='POST'>"
            + "    <div class='form-group'>"
            + "         <label class='control-label col-sm-3'>Scan wrong label:</label>"
            + "         <div class='col-sm-9'>"
            + "              <input id='fldWrongLabel' name='fldWrongLabel' type='text' class='form-control' placeholder='Label code' onKeyUp='" + "if(validateEnter(event)){getLabelInfoST($(\"#fldWrongLabel\").val());}"+ "'>"
            + "         </div>"
            + "    </div>"
            + "    <div class='form-group'>"
            + "         <label class='control-label col-sm-3'>Quantity:</label>"
            + "         <div class='col-sm-9'>"
            + "             <input  id='fldNewQuantity' name='fldNewQuantity' type='text' class='form-control' placeholder='Quantity' onKeyUp='" + "if(validateEnter(event)){updateLabelInfoST($(\"#fldWrongLabel\").val(),$(\"#fldNewQuantity\").val());}" + "'>"
            + "         </div>"
            + "    </div>"
            + "    <div id='divQuantityStatus'>"
            + "    </div>"
            + "</form>"
            + "</div>"
    });
}

function fixLblQtyMI() {
    BootstrapDialog.show({
        title: 'Fix Label Qty',
        message: "<div class='window-content'>"
            + "<form id='reprintLabelForm' name ='reprintLabel' class='form-horizontal' method='POST'>"
            + "    <div class='form-group'>"
            + "         <label class='control-label col-sm-3'>Scan wrong label:</label>"
            + "         <div class='col-sm-9'>"
            + "              <input id='fldWrongLabel' name='fldWrongLabel' type='text' class='form-control' placeholder='Label code' onKeyUp='" + "if(validateEnter(event)){getLabelInfoMI($(\"#fldWrongLabel\").val());}" + "'>"
            + "         </div>"
            + "    </div>"
            + "    <div class='form-group'>"
            + "         <label class='control-label col-sm-3'>Quantity:</label>"
            + "         <div class='col-sm-9'>"
            + "             <input  id='fldNewQuantity' name='fldNewQuantity' type='text' class='form-control' placeholder='Quantity' onKeyUp='" + "if(validateEnter(event)){updateLabelInfoMI($(\"#fldWrongLabel\").val(),$(\"#fldNewQuantity\").val());}" + "'>"
            + "         </div>"
            + "    </div>"
            + "    <div id='divQuantityStatus'>"
            + "    </div>"
            + "</form>"
            + "</div>"
    });
}
window.setTimeout(function () {
    $(".alert").fadeTo(300, 0).slideUp(300, function () {
        $(this).remove();
    });
}, 4000);