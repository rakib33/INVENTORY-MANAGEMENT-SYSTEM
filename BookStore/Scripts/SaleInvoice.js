
function CheckDiscount() {

    var paid = parseFloat($('#Paid').val());
    var Discount = parseFloat($('#CurrentDue').val());

    var Payable = parseFloat($('#Payable').val());
    var CurrentDiscount = 0;

    if (paid > 0 && paid <= Payable) {


        CurrentDiscount = parseFloat(Discount - paid);
        $('#Due').val(CurrentDiscount);

    } else {

        $('#Due').val(Discount);

        if (paid > Payable) {
            $('#Paid').val("");
            alert('Pay amount must equals Total amount.');
        }
    }




}

function Check() {

    var paid = parseFloat($('#Paid').val());
    var Discount = parseFloat($('#CurrentDue').val());

    var Payable = parseFloat($('#Payable').val());

    var CurrentDiscount = Discount - paid;

    if (paid <= 0) {
        $('#Paid').val("");
        return false;
    } else if (paid > Payable) {
        $('#Paid').val("");
        alert('Pay amount must equals Total amount.');
    } else if (paid <= Payable) {
        return true;
    }
    else {
        alert('Unexpected Error:Add Pay.');
        return false;
    }

}