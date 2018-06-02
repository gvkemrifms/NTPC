$(function () {
    $('#<%= btnShowReport.ClientID %>').click(function () {
        var ddlDistrict = $('#<%= ddldistrict.ClientID %> option:selected').text().toLowerCase();
        if (ddlDistrict === '--select--') {
            alert("Please select District");
            e.preventDefault("Please select District");
        }
        var ddlVehicle = $('#<%= ddlvehicle.ClientID %> option:selected').text().toLowerCase();
        if (ddlVehicle === '--select--') {
            alert("Please select Vehicle");
            e.preventDefault();
        }
        var txtFirstDate = $('#<%= txtfrmDate.ClientID %>').val();
        var txtToDate = $('#<%= txttodate.ClientID %>').val();
        if (txtFirstDate === "") {
            alert('From Date is Mandatory');
            txtFirstDate.focus();
            e.preventDefault();
        }
        if (txtToDate === "") {
            alert("End Date is Mandatory");
            txtToDate.focus();
            e.preventDefault();
        }
        var fromDate = (txtFirstDate).replace(/\D/g, '/');
        var toDate = (txtToDate).replace(/\D/g, '/');
        var ordFromDate = new Date(fromDate); var ordToDate = new Date(toDate);
        if (ordToDate < ordFromDate) {
            alert("Please select valid date range");
        }
    });
})