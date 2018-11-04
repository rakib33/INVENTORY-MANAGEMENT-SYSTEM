//http://www.c-sharpcorner.com/UploadFile/4d9083/creating-simple-cascading-dropdownlist-in-mvc-4-using-razor/


function EmptySubCatagory(e) {
    var id = "#Sale_" + e + "__SubCatagoryId";
    $(id).empty();
}

function EmptyProductId(e) {
    var Prodid = "#Sale_" + e + "__Product_Id";
    $(Prodid).empty();
}

// add by rakiul 03/10/18
function EmptyQtyInStock(e) {
    var QtyInStock = "Sale[" + e + "]Stock";
    $(QtyInStock).empty();
}

Function 

function GetCustomerInfo(e)
{
    var eid = document.getElementById(e);
    var UserId = eid.options[eid.selectedIndex].value;

        try {
            $.ajax({
                type: 'POST',
                url: '/Data/GetCustomerInfo',

                dataType: 'json',
                data: { userId: UserId },

                success: function (data) {
                    if (data.message == "true") {

                        //alert(data.List.CostPrice + " Sale Price:" + data.List.SalePrice);
                        //document.getElementById(BuyRate).value = data.List.CostPrice;
                        //document.getElementById(BuyTotal).value = qty * data.List.CostPrice;

                        document.getElementById("CustomerName").value = data.user.Name;
                        document.getElementById("CustomerPhone").value = data.user.Phone;

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

function GetSubCatagory(e) {

    var eid = document.getElementById(e);
    var MainId = eid.options[eid.selectedIndex].value;

    var array = e.split(']');
    array = array[0].split('[');
  
    //EmptySubCatagory(array[1]);
    //EmptyProductId(array[1]);
    //EmptyQtyInStock(array[1]);

    var id = "#Sale_" + array[1] + "__SubCatagoryId";
    var Prodid = "#Sale_" + array[1] + "__Product_Id";

    $(id).empty();
    $(Prodid).empty();
    EmptyQtyInStock(array[1]);
    try {
        $.ajax({
            type: 'POST',
            url: '/Data/getSubCatagory',

            dataType: 'json',
            data: { id: MainId },

            success: function (lists) {

                $(id).removeClass('loadinggif');
                $.each(lists, function (i, list) {
                    $(id).append('<option value="' + list.Value + '">' + list.Text + '</option>'); //"#Catagory_Id"
                    ClearAll(array[1]);
                });
            },
            error: function (ex) {
                $(id).removeClass('loadinggif');
                alert('Failed to retrieve sub catagory.' + ex);
                ClearAll(array[1]); //add by Rakibul 03/10/18
            }
        });
    } catch (err) {
        $(id).removeClass('loadinggif');
        alert('Exp: ' + err.message);
    }
    return false;
}

function getProduct(e) {

    var eid = document.getElementById(e);
    var CatId = eid.options[eid.selectedIndex].value;

    var array = e.split('__');
    array = array[0].split('_');

    var id = "#Sale_" + array[1] + "__Product_Id";   
    $(id).empty();

 

    try {
        $.ajax({
            type: 'POST',
            url: '/Data/getProduct',

            dataType: 'json',
            data: { Catid: CatId },

            success: function (lists) {
                $.each(lists, function (i, list) {

                    $(id).append('<option value="' + list.Value + '">' + list.Text + '</option>'); //"#Catagory_Id"
                    ClearAll(array[1]);
                });
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

function getRate(e) {
  
   
    var qty = document.getElementById(e).value;  

    var array = e.split(']');
    array = array[0].split('[');

    if (qty.length == 0) {
        ClearAll(array[1]);
        alert('Insert Quantity');

    }
    else if (qty.length > 0 || qty != undefined) {
        var id = "Sale_" + array[1] + "__Product_Id";     

       

            var SaleRate = "Sale[" + array[1] + "]SaleRate";
            var SaleTotal = "Sale[" + array[1] + "]SaleTotal";
            var SaleQty = "Sale[" + array[1] + "]Qty";

          //  alert('Product Id='+id);
            var eid = document.getElementById(id);

           // alert('eid=' + eid);
            var ProductValue = eid.options[eid.selectedIndex].value;
           // alert('Product Value' + ProductValue);

            if (ProductValue == null && ProductValue == '') {
                alert("Select a product!");
                return false;
            }
            else {

                //  alert(CheckProductAlreadyTake(ProductValue));
                var result = CheckProductAlreadyTake(ProductValue);
               // alert('result ='+ result);
            if(result == 1)
             {
                try {
                    $.ajax({
                        type: 'POST',
                        url: '/Data/getSaleRate',

                        dataType: 'json',
                        data: { productId: ProductValue, SaleQty: qty },

                        success: function (data) {

                            if (data.message == "true") {

                                if (data.List == null) {

                                    document.getElementById(SaleRate).value = "";
                                    document.getElementById(SaleTotal).value = "";
                                    document.getElementById(SaleQty).value = "";

                                    alert('Maximum Quantity exceed');

                                } else {
                                    document.getElementById(SaleRate).value = data.List.SalePrice;
                                    document.getElementById(SaleTotal).value = qty * data.List.SalePrice;
                                }

                            } else {

                                document.getElementById(SaleRate).value = "";
                                document.getElementById(SaleTotal).value = "";

                                alert("data retraive failed.ex-" + data.message);
                            }
                        },
                        error: function (ex) {

                            document.getElementById(SaleRate).value = "";
                            document.getElementById(SaleTotal).value = "";

                            alert('Failed to retrieve data.' + ex);
                        }
                    });
                } catch (err) {

                    document.getElementById(SaleRate).value = "";
                    document.getElementById(SaleTotal).value = "";

                    alert('Exp: ' + err.message);
                }
            }
            else
            {
                document.getElementById(SaleRate).value = "";
                document.getElementById(SaleTotal).value = "";
                document.getElementById(SaleQty).value = "";

                alert('This Product already taken.');

               }
            }
        

        return false;
    }
}

function ChangeTotalAmt(e)
{
    var Rate = document.getElementById(e).value;

    var array = e.split(']');
    array = array[0].split('[');

    if (Rate.length == 0) {
        ClearAll(array[1]);
        alert('Insert Sale Rate.');

    }
    else if (Rate.length > 0 || Rate != undefined) {
        var id = "Sale_" + array[1] + "__Product_Id";

        var SaleTotal = "Sale[" + array[1] + "]SaleTotal";
        var SaleId = "Sale[" + array[1] + "]Qty";


        var eid = document.getElementById(id);  
        var ProductValue = eid.options[eid.selectedIndex].value;

        var SaleQty = document.getElementById(SaleId).value;

        //check Is Product selected
        if (ProductValue == null && ProductValue == '') {
            alert("Select a product!");
            return false;
        }
        else if (SaleQty == 0 || isNaN(SaleQty))
        {

            alert('Insert Product Quantity..');
        }
        else {

            var result = CheckProductAlreadyTake(ProductValue);
          

            if (result == 1) {
                try {

                //    alert('Sale Rate =' + Rate + ' SaleQty=' + SaleQty);
                   
                    document.getElementById(SaleTotal).value = SaleQty * Rate;
                  
                } catch (err) {

                    document.getElementById(SaleRate).value = "";
                    document.getElementById(SaleTotal).value = "";

                    alert('Exp: ' + err.message);
                }
            }
            else {
                document.getElementById(SaleRate).value = "";
                document.getElementById(SaleTotal).value = "";
                //document.getElementById(SaleQty).value = "";
                $(SaleQty).val('');

                alert('This Product already taken.');

            }
        }


        return false;
    }

}

function CheckProductAlreadyTake(e)
    {
        //alert('given Product: ' + e);
        var count = 0;
        $('.myclass').each(function (i, obj) {

            var Prodid = "Sale_" + i + "__Product_Id";       

            var eid = document.getElementById(Prodid);
           // alert('check product eid i'+i);
            try{
                var productId = eid.options[eid.selectedIndex].value;

             //   alert('given Product: ' + e + ' and ' + productId)

                if (e == productId) {
                    count++;
                
                }
            }
            catch(err)
            {
            }
       

        });
 
        return count;
   
    }

function ResetAll(e) {

        var eid = document.getElementById(e);
        var productId = eid.options[eid.selectedIndex].value;


        var array = e.split('__');
        array = array[0].split('_');
        ClearAll(array[1]);

        var QtyInStock = "Sale[" + array[1] + "]Stock";

        if (productId == null && productId == '') {
            alert("Select a product!");
            return false;
        } else {

            try {
                $.ajax({
                    type: 'POST',
                    url: '/Data/ProductInStock',

                    dataType: 'json',
                    data: { productId: productId },

                    success: function (data) {

                        if (data.message == "true") {

                            document.getElementById(QtyInStock).value = data.Stock;                       
                        

                        } else {
                            document.getElementById(QtyInStock).value = "";
                            alert("data retraive failed.ex-" + data.message);
                        }
                    },
                    error: function (ex) {
                        document.getElementById(QtyInStock).value = "";
                        alert('Failed to retrieve data.' + ex);
                    }
                });
            } catch (err) {
                document.getElementById(QtyInStock).value = "";
                alert('Exp: ' + err.message);
            }

        }
    }

function ClearAll(index) {

        var Qty = "Sale[" + index + "]Qty";
     
        var SaleRateId = "Sale[" + index + "]SaleRate";   
        var SaleTotal = "Sale[" + index + "]SaleTotal";

      
        document.getElementById(Qty).value = "";    

        document.getElementById(SaleRateId).value = "";
        document.getElementById(SaleTotal).value = "";


    }

//1. Add new row for Brand
$("#addNew").click(function (e) {
        e.preventDefault();

        var prev;
        var change;
        var RemoveCol;
        var newHtml = '<tr>';


        var table = document.getElementById("SaleTable");

        //get the table last row index
        var lastRowIndex = table.rows.length;
        //if two row 0,1 then lastRowIndex=2        

        // alert('last :' + lastRowIndex);
        //Count number of columns in a table row
        var totalcell = table.rows[lastRowIndex - 1].cells.length;

        RemoveCol = totalcell - 1;
        //alert('total cell: ' + totalcell);


        for (var i = 0; i < totalcell; i++) {
            //get per cell inner html
            var cellInnerHtml = table.rows[lastRowIndex - 1].cells[i].innerHTML;

            // alert('cell:[' + i + '] inner html:   ' + cellInnerHtml);
            //we know first cell is index so just incriment it

            //first column add Index Number
            if (i == 0) {
                var index = parseInt(cellInnerHtml);
                index = index + 1;
                newHtml += '<td>';
                newHtml += index;
                newHtml += '</td>';
            }
            else {

                //if (i == RemoveCol) //last cell remove button
                //{                
                //    newHtml += '<td id="btn">';
                //   // newHtml += '<a href="#" id="' + lastRowIndex + '" class="btn btn-danger ra-100"  onclick="DeleteRow(this)"><i class="glyph-icon icon-minus-square" title="" data-original-title=".icon-minus-square" aria-describedby="tooltip941007">  Remove</i></a>';
                //    newHtml += ' <a class="btn btn-sm btn-danger ra-100" href="#" onclick="DeleteRow(this)"><i class="glyph-icon icon-minus" title="" data-original-title=".icon-minus" aria-describedby="tooltip941007">Remove</i></a>';
                //    newHtml += '</td>';
                //}
                //else {
                //though first row contaain text (<th>Sl No</th>
                //<th>Contact Name</th>
                //<th>Contact No</th>)
                var p = lastRowIndex - 2;
                var q = lastRowIndex - 1;

                prev = '[' + p + ']';

                change = '[' + q + ']';

                newHtml += '<td>';
                newHtml += cellInnerHtml.replace(prev, change);

                newHtml = newHtml.replace(prev, change);

                newHtml += '</td>';
                if (i == 3 || i == 4) {
                    //sub catagory and product dropdown replace id as Sale_0__InvoiceNo
                    prev = '_' + p + '__';
                    change = '_' + q + '__';
                    newHtml = newHtml.replace(prev, change);
                    //empty this two dropdown
                    var MainCat = "#Sale_" + q + "__SubCatagoryId";
                    var SubCat = "#Sale_" + q + "__Product_Id";

                    $(MainCat).empty();
                    $(SubCat).empty();

                }

                // }
            }
        }

        newHtml += '</tr>';
        //alert(newHtml);

        $('#SaleTable').append(newHtml);
    });

    //delete a row
function DeleteRow(o) {
        //alert('hited');            
        //no clue what to put here?
        var p = o.parentNode.parentNode;
        p.parentNode.removeChild(p);
    }


