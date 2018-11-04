function click() {

    alert('Hited');
    return false;
}

$("#PaidAmount").keyup(function () {
    var CurrentTotal = parseFloat($('#CurrentTotal').val());
    var AlreadyPaid = parseFloat($('#CurrentPaid').val());

    var CurrentDue = parseFloat($('#CurrentDue').val());

    var duepaid = parseFloat($('#PaidAmount').val());

 
    var CurrentDiscount = 0;

    if (duepaid > 0 && duepaid <= CurrentDue) {
        CurrentDiscount = parseFloat(CurrentDue - duepaid);
        //$('#Due').val(CurrentDiscount);

    } else {      

        if (duepaid > CurrentDue) {
            $('#PaidAmount').val("");
            alert('Due Pay amount must equals Total Due Amount.');
        }
        if (duepaid < 0) {
            $('#PaidAmount').val("");
        }
    }
});


//Called from AddTransaction page
function GetEmployeeSalary(e) {

    var eid = document.getElementById(e);
    var UserId = eid.options[eid.selectedIndex].value;
   // alert(UserId);

    try {
        $.ajax({
            type: 'POST',
            url: '/Data/GetCustomerInfo',

            dataType: 'json',
            data: { userId: UserId },

            success: function (data) {
                if (data.message == "true") {
                    
                    $("#Total").val(data.user.Salary);
                    $("#Payable").val(data.user.Salary);

                    $("#Paid").val('');
                    $("#BonusOrExtra").val(null);
                    $("#MobileBill").val(null);
                    $("#TransportBill").val(null);
                    $("#Discount").val(null);

                    //document.getElementById("Total").value = data.user.Salary;
                    //document.getElementById("Payable").value = data.user.Salary;
                 
                } else {

                    alert("data retraive failed.ex-" + data.message);
                }
            },
            error: function (ex) {
                alert('Failed to retrieve sub catagory.' + ex);
            }
        });
    } catch (err) {
        alert('Exp: ' + err.message);
    }
    return false;
}



function PayableCalculation()
{
   // alert('hited');
   var TotalSalary= parseFloat($("#Total").val());
   var Bonus = parseFloat($("#BonusOrExtra").val());
   var MobileBill = parseFloat( $("#MobileBill").val());
   var Tranbill = parseFloat($("#TransportBill").val());
   var diduction = parseFloat( $("#Discount").val());
    
   if (isNaN(Bonus))
       Bonus = 0;
   if (isNaN(MobileBill))
       MobileBill = 0;
   if (isNaN(Tranbill))
       Tranbill = 0;
   if (isNaN(diduction))
       diduction = 0;

   var Payable = TotalSalary + Bonus + MobileBill + Tranbill - diduction;
   

   $("#Payable").val(Payable);

   $("#Paid").val('');

   return true;

}

/************ Below function is for AddSalary Page Edit Salary Page ***********/
//AddTransactionPage
$("#Paid").keyup(function () {

    var TotalSalary = parseFloat($("#Payable").val());
       
    var Paid = parseFloat($("#Paid").val());

    if (isNaN(TotalSalary)) {
        alert('Amount Payable Must Have a Value.');
        $("#Paid").val('');
    }
    else if (TotalSalary < 1)
    {
        alert('Amount Payable Must Have a Value.');
        $("#Paid").val('');
    }
    else if (TotalSalary > 0) {

        if (Paid <= TotalSalary) {
          
        }
        else if (Paid < 0) {
            $('#Paid').val("");
        }
        else {
            $('#Paid').val("");
            alert('Paid amount must equals Total Salary.');
        }


    } else {
        alert('Invalid Transaction!');
    }

})


$("#BonusOrExtra").keyup(function () {
    //alert('hited');
    var result = PayableCalculation();
})

$("#MobileBill").keyup(function () {
    var result = PayableCalculation();
})

$("#TransportBill").keyup(function () {
    var result = PayableCalculation();
})

$("#Discount").keyup(function () {
    var result = PayableCalculation();
})


/***************** End Add Salary and Edit Salary Page  ********************/


/***************** Below is for Add/Edit Profit Expense Page  ********************/


function ProfitExpenseCalculation() {
    // alert('hited');
   
    var Profit = parseFloat($("#MonthlyProfit").val());

    var RentCost = parseFloat($("#RentExpense").val());
    var ElectricityBill = parseFloat($("#ElectricityBill").val());
    var GassBill = parseFloat($("#GassBill").val());
    var WaterBill = parseFloat($("#WaterBill").val());
    var OtherExpense = parseFloat($("#OtherExpense").val());

    if (isNaN(Profit))
        Profit = 0;

    if (isNaN(RentCost))
        RentCost = 0;

    if (isNaN(ElectricityBill))
        ElectricityBill = 0;

    if (isNaN(GassBill))
        GassBill = 0;

    if (isNaN(WaterBill))
        WaterBill = 0;

    if (isNaN(OtherExpense))
        OtherExpense = 0;

    var Payable = Profit - (RentCost + ElectricityBill + GassBill + WaterBill + OtherExpense);

    $("#Total").val(Payable)
    $("#Payable").val(Payable);
    $("#Paid").val(Payable);

    return true;

}


$("#MonthlyProfit").keyup(function () {

   // alert('hited');
    ProfitExpenseCalculation();

});

$("#RentExpense").keyup(function () {

    ProfitExpenseCalculation()

});

$("#ElectricityBill").keyup(function () {

    ProfitExpenseCalculation()

});
$("#GassBill").keyup(function () {

    ProfitExpenseCalculation()

});
$("#WaterBill").keyup(function () {

    ProfitExpenseCalculation()

});
$("#OtherExpense").keyup(function () {

    ProfitExpenseCalculation()

});

/***************** Below is for Add/Edit Profit Expense Page  ********************/