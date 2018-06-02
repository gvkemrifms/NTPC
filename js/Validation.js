function CheckLength(text, long) {
    var maxlength = new Number(long); // Change number to your max length.
    if (text.value.length > maxlength) {
        text.value = text.value.substring(0, maxlength);
        alert(" Only " + long + " chars");
    }
}

function numeric(event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode === 190 || charCode > 31 && (charCode < 48 || charCode > 57)) {
        var txtBox = document.getElementById(event.srcElement.id);
        return txtBox.value.indexOf(".") === -1;
    } else return true;
}

function numeric_only(e) {
    var keycode;
    if (window.event || event || e) keycode = window.event.keyCode;
    else return true;
    return keycode >= 48 && keycode <= 57;
}

function OnlyAlphabets(myfield, e, dec) {
    var keycode;
    if (window.event || event || e) keycode = window.event.keyCode;
    else return true;
    return (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122) || (keycode === 32);
}

function OnlyAlphaNumeric(e) {
    var keycode;
    if (window.event || event || e) keycode = window.event.keyCode;
    else return true;
    return (keycode >= 47 && keycode <= 57) || (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122);
}


function numericOnly(elementRef) {

    var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;
    if ((keyCodeEntered >= 48) && (keyCodeEntered <= 57)) {
        return true;
    }
    // '.' decimal point...  
    else if (keyCodeEntered === 46) {
        // Allow only 1 decimal point ('.')...  
        if ((elementRef.value) && (elementRef.value.indexOf('.') >= 0))
            return false;
        else
            return true;
    }
    return false;
}

function isValidPAN(pan) {
    if (pan.match(/^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/)) {
        return true;
    } else {
        alert("Enter a valid PAN eg - 'BBAPM6454J'");
        return false;
    }
}

function isValidEmail(email) {
    if (email.match(/^(?:\w+\.?)*\w+@(?:\w+\.)+\w+$/)) {
        return true;
    } else {
        alert("Enter a valid Email Address");
        return false;
    }
}

function validateDecimal(value) {
    var re = "^\d*\.?\d{0,2}$";
    if(re.test(value)){
        return true;
    }else{
        return false;
    }
}
function OnlyNumPeriod(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (evt.shiftKey === 1 ||
        (charCode >= 48 && charCode <= 57) ||
        (charCode > 95 && charCode < 106) ||
        charCode === 8 ||
        charCode === 46 ||
        charCode === 190 ||
        charCode === 9 ||
        charCode === 110) {
        alert("Shift Key is not allowed");
    }
    return false;
}

function OnlyAlphabets(myfield, e, dec) {
    var key;
    if (window.event || e)
        key = window.event.keyCode;
    else return true;
    var keychar = String.fromCharCode(key);
    return (" !@#$%^&*()_+=-';{}[]|?<>:,/\".1234567890").indexOf(keychar) <= -1;
}

function OnlyNumbers(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    return charCode <= 31 || (charCode >= 48 && charCode <= 57);
}

function trim(value) {
    value = value.replace(/^\s+/, "");
    value = value.replace(/\s+$/, "");
    return value;

}

function Integersonly(event) {

    var decimalvalidate = ApplyDecimalFilter(document.all(event.target.id).value, event);
    return decimalvalidate !== false;
}

function ApplyDecimalFilter(id, event) {
    try {
        return NewDecimalFilter(id, event);
    } catch (e) {
        alert(e.message);
    }
    return true;
}

function NewDecimalFilter(o, event) {
    var key = event.keyCode || event.which;
    var keychar = String.fromCharCode(key);
    if (event.which > 47 && event.which < 58) {
        return true;
    }
    if (key === 37 || key === 38 || key === 39 || key === 40) {
        return ("1234567890").indexOf(keychar) > -1;
    }

    if (event.which === 50 || (event.keyCode === 8 || key === 9 || event.keyCode === 46) && o.indexOf(".") === -1) {
        switch (navigator.appName) {
        case "Microsoft Internet Explorer":
            if (("1234567890").indexOf(keychar) > -1)
                return true;
            else
                return false;
        default:
            return true;
        }
    }
    return false;
}

function DisableRightClick(event) {
    //For mouse right click
    switch (event.button) {
    case 2:
        alert("Right Clicking not allowed!");
        break;
    }
}

function DisableCtrlKey(e) {
    var code = (document.all) ? event.keyCode : e.which;
    var message = "Ctrl key functionality is disabled!";
    // look for CTRL key press
    switch (parseInt(code)) {
    case 17:
        alert(message);
        window.event.returnValue = false;
        break;
    }
}

function isDecimalNumberKey(event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode === 190 || charCode === 46 || charCode > 31 && (charCode < 48 || charCode > 57)) {
        var txtBox = document.getElementById(event.srcElement.id);
        return txtBox.value.indexOf(".") === -1;
    } else return true;
}

function alphanumeric_only(e) {
    var keycode;
    if (window.event || event || e) keycode = window.event.keyCode;
    else return true;
    return (keycode >= 48 && keycode <= 57) || (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122);
}

function alphanumeric_only_withspace(e) {
    var keycode;
    if (window.event || event || e) keycode = window.event.keyCode;
    else return true;
    return (keycode >= 48 && keycode <= 57) ||
        (keycode >= 65 && keycode <= 90) ||
        (keycode >= 97 && keycode <= 122) ||
        (keycode === 32);
}

function DecimalValidate(control) {
    var rgexp = new RegExp("^\d*([.]\d{2})?$");
    var input = document.getElementById(control).value;
    return input.match(rgexp);
}

function alpha_only(e) {
    var keycode;
    if (window.event || event || e) keycode = window.event.keyCode;
    else return true;
    return (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122);
}

function isValidVehicleNumber(vehicleNo) {
    if (!vehicleNo.match(/^[A-Z]{2}[0-9]{2}[A-Z]{1,2}[0-9]{1,4}$/)) {
        alert("Enter a valid Vehicle T/R Number eg - 'AP27HY9834' or 'AP27H8'");
        return false;
    } else {
        return true;
    }
}

function alpha_only_withspace(e) {
    var keycode;
    if (window.event || event || e) keycode = window.event.keyCode;
    else return true;
    return (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122) || (keycode === 32);
}

function alphanumeric_withspace_only(e) {
    var keycode;
    if (window.event || event || e) keycode = window.event.keyCode;
    else return true;
    return (keycode >= 48 && keycode <= 57) ||
        (keycode >= 65 && keycode <= 90) ||
        (keycode >= 97 && keycode <= 122) ||
        (keycode == 32);
}


function remark(e) {
    var keycode;
    if (window.event || event || e) keycode = window.event.keyCode;
    else return true;
    return (keycode !== 34) && (keycode !== 39);
}

function isValidDate(subject) {
    return subject.match(/^(?:(0[1-9]|1[012])[\- \/.](0[1-9]|[12][0-9]|3[01])[\- \/.](19|20)[0-9]{2})$/);
}

function RequiredValidation(ctrl, msg) {
    switch (trim(ctrl.value)) {
    case "":
        alert(msg);
        ctrl.focus();
        return false;
    default:
        return true;
    }
}

function ValidateIssueQty(issQtyId, reqQty) {
    var objIssQty = document.getElementById(issQtyId);
    if (parseInt(objIssQty.value) > parseInt(reqQty)) {
        alert("Issued Quantity Cannot be more than Request Quantity");
        objIssQty.focus();
        return false;
    }
    return true;
}

function isValidVehicleNumber(vehicleNo) {
    return !!vehicleNo.match(/^[A-Z]{2}[0-9]{2}[A-Z]{1,2}[0-9]{1,4}$/);
}

function ConfirmApprove(btnId, remarkId) {
    var btnApprove = document.getElementById(btnId);
    if (remarkId) {
        alert("Please enter remarks!");
        (document.getElementById(remarkId)).focus();
    } else {
        if (confirm("Are you sure you want to Approve"))
            btnApprove.click();
    }
}

function ConfirmReject(btnId, remarkId) {
    var btnReject = document.getElementById(btnId);
    if (remarkId) {
        alert("Please enter remarks!");
        (document.getElementById(remarkId)).focus();
    } else {
        if (confirm("Are you sure you want to Reject"))
            btnReject.click();
    }
}

function EnableRemarks() {
    document.getElementById("<%= gvVerification.ClientID %>");
    return true;
}