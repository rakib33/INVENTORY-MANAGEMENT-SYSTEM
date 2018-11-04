//http://www.c-sharpcorner.com/UploadFile/4d9083/creating-simple-cascading-dropdownlist-in-mvc-4-using-razor/


function GetSubCatagory(e)
{    
   
    var eid = document.getElementById(e);
    var MainId = eid.options[eid.selectedIndex].value;
   
    var array = e.split(']');
    array = array[0].split('[');
    var id = "#Purchase_" + array[1] + "__SubCatagoryId";
    var Prodid = "#Purchase_" + array[1] + "__Product_Id";
   
    $(id).empty();
    $(Prodid).empty();
    $(id).addClass('loadinggif');
    try {
        $.ajax({
            type: 'POST',
            url: '/Data/getSubCatagory',

            dataType: 'json',
            data: { id: MainId },

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

function getProduct(e) {
   
    var eid = document.getElementById(e);
    var CatId = eid.options[eid.selectedIndex].value;
  
    var array = e.split('__');
    array = array[0].split('_');
    var id = "#Purchase_" + array[1] + "__Product_Id";

   
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

function getRate(e)
{
    var qty = document.getElementById(e).value;
 
    var array = e.split(']');
    array = array[0].split('[');

    if (qty.length == 0)
    {
        ClearAll(array[1]);
        alert('Insert Quantity');

    }
    else if(qty.length > 0 || qty != undefined)
    {
        var id = "Purchase_" + array[1] + "__Product_Id";
        var BuyRate = "Purchase[" + array[1] + "]BuyRate";
        var BuyTotal = "Purchase[" + array[1] + "]BuyTotal";

        var SaleRate = "Purchase[" + array[1] + "]SaleRate";     
        var SaleTotal = "Purchase[" + array[1] + "]SaleTotal";

      
        var eid = document.getElementById(id);
        var ProductValue = eid.options[eid.selectedIndex].value;
       

        if (ProductValue == null && ProductValue == '') {
            alert("Select a product!");
            return false;
        } else {

            var result = CheckProductAlreadyTake(ProductValue);
            //alert('result ='+ result);
            if (result == 1) {
                try {
                    $.ajax({
                        type: 'POST',
                        url: '/Data/getRate',

                        dataType: 'json',
                        data: { productId: ProductValue },

                        success: function (data) {

                            if (data.message == "true") {

                                //alert(data.List.CostPrice + " Sale Price:" + data.List.SalePrice);

                                document.getElementById(BuyRate).value = data.List.CostPrice;
                                document.getElementById(BuyTotal).value = qty * data.List.CostPrice;

                                document.getElementById(SaleRate).value = data.List.SalePrice;
                                document.getElementById(SaleTotal).value = qty * data.List.SalePrice;

                            } else {

                                alert("data retraive failed.ex-" + data.message);
                            }
                        },
                        error: function (ex) {
                            alert('Failed to retrieve data.' + ex);
                        }
                    });
                } catch (err) {
                    alert('Exp: ' + err.message);
                }
            }
            else {
                alert('This Product already taken.');
            }
        }
        return false;
    } 
}

function ResetAll(e)
{
   

    var eid = document.getElementById(e);
    var CurrentProductId = eid.options[eid.selectedIndex].value;
    //alert(CurrentProductId);

    var array = e.split('__');
    array = array[0].split('_');
    ClearAll(array[1]);

    //check is this product is laready taken for add
    //CheckProductAlreadyTake(CurrentProductId);
  
}


function CheckProductAlreadyTake(e) {
    //alert('given Product: ' + e);
    var count = 0;
    $('.myclass').each(function (i, obj) {

        var Prodid = "Purchase_" + i + "__Product_Id";

        var eid = document.getElementById(Prodid);
        var productId = eid.options[eid.selectedIndex].value;
        //alert('given Product: ' + e + ' and ' + productId)
        if (e == productId) {
            count++;
            // alert(count);
        }


    });

    return count;

}

function ClearAll(index)
{
 
    var Qty = "Purchase[" + index + "]Qty";
    var BuyRateId = "Purchase[" + index + "]BuyRate";
    var SaleRateId = "Purchase[" + index + "]SaleRate";
    var BuyTotal = "Purchase[" + index + "]BuyTotal";
    var SaleTotal = "Purchase[" + index + "]SaleTotal";

   
    //empty all textbox
    document.getElementById(Qty).value = "";
    document.getElementById(BuyRateId).value = "";
    document.getElementById(SaleRateId).value = "";
    document.getElementById(BuyTotal).value = "";
    document.getElementById(SaleTotal).value = "";


}


//1. Add new row for Brand
$("#addNew").click(function (e) {
    e.preventDefault();

    var prev;
    var change;
    var RemoveCol;
    var newHtml = '<tr>';


    var table = document.getElementById("PurchaseTable");

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
            //    newHtml += '<a href="#" id="' + lastRowIndex + '" class="btn btn-danger ra-100"  onclick="DeleteRow(this)"><i class="glyph-icon icon-minus-square" title="" data-original-title=".icon-minus-square" aria-describedby="tooltip941007">  Remove</i></a>';
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
                if (i == 3 || i == 4) 
                {
                    //sub catagory and product dropdown replace id as Purchase_0__InvoiceNo
                    prev = '_' + p + '__';
                    change = '_' + q + '__';
                    newHtml = newHtml.replace(prev, change);
                    //empty this two dropdown
                    var MainCat = "#Purchase_" + q + "__SubCatagoryId";
                    var SubCat = "#Purchase_" + q + "__Product_Id";

                    $(MainCat).empty();
                    $(SubCat).empty();

                }
                
           // }
        }
    }

    newHtml += '</tr>';
    //alert(newHtml);

    $('#PurchaseTable').append(newHtml);
});

//delete a row
function DeleteRow(o) {
    //alert('hited');            
    //no clue what to put here?
    var p = o.parentNode.parentNode;
    p.parentNode.removeChild(p);
}


function ChangeTotalAmt(e) {
    var Rate = document.getElementById(e).value;

    var array = e.split(']');
    array = array[0].split('[');

    if (Rate.length == 0) {
        ClearAll(array[1]);
        alert('Insert Buy Rate.');

    }
    else if (Rate.length > 0 || Rate != undefined) {
        var id = "Purchase_" + array[1] + "__Product_Id";

        var BuyTotal = "Purchase[" + array[1] + "]BuyTotal";
        var BuyId = "Purchase[" + array[1] + "]Qty";

        var BuyRate = "Purchase[" + array[1] + "]BuyRate";

        var eid = document.getElementById(id);
        var ProductValue = eid.options[eid.selectedIndex].value;

        var BuyQty = document.getElementById(BuyId).value;

        //check Is Product selected
        if (ProductValue == null && ProductValue == '') {
            alert("Select a product!");
            return false;
        }
        else if (BuyQty == 0 || isNaN(BuyQty)) {
            alert('Insert Product Quantity..');
        }
        else {

            var result = CheckProductAlreadyTake(ProductValue);


            if (result == 1) {
                try {

                    //alert('Buy Rate =' + Rate + ' Buy Qty=' + BuyQty);

                    document.getElementById(BuyTotal).value = BuyQty * Rate;

                } catch (err) {

                    document.getElementById(BuyRate).value = "";
                    document.getElementById(BuyTotal).value = "";

                    alert('Exp: ' + err.message);
                }
            }
            else {
                document.getElementById(BuyRate).value = "";
                document.getElementById(BuyTotal).value = "";
              
                $(BuyQty).val('');

                alert('This Product already taken.');

            }
        }


        return false;
    }

}