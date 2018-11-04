//http://www.c-sharpcorner.com/UploadFile/4d9083/creating-simple-cascading-dropdownlist-in-mvc-4-using-razor/
    $(document).ready(function () {
        $("#MainCatagory").change(function () {

           // alert('hited' + $("#MainCatagory").val());
            $("#Catagory_Id").empty();
            try {
                $.ajax({
                    type: 'POST',
                    url: '/Data/getSubCatagory',

                    dataType: 'json',
                    data: { id: $("#MainCatagory").val() },

                    success: function (lists) {
                        $.each(lists, function (i, list) {
                            $("#Catagory_Id").append('<option value="' + list.Value + '">' + list.Text + '</option>');
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
        });
    });


//1. Add new row for Brand
$("#addNew").click(function (e) {
                e.preventDefault();
               
                var prev;
                var change;
                var RemoveCol;
                var newHtml = '<tr>';


                var table = document.getElementById("dataTable");

                //get the table last row index
                var lastRowIndex = table.rows.length;
                //if two row 0,1 then lastRowIndex=2        

               // alert('last :' + lastRowIndex);
                //Count number of columns in a table row
                var totalcell = table.rows[lastRowIndex - 1].cells.length;

                RemoveCol = totalcell - 1;
               // alert('total cell: ' + totalcell);


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

                        if (i == RemoveCol) //last cell remove button
                        {
                            // alert('last cell');
                            newHtml += '<td id="btn">';
                            newHtml += '<a href="#" id="' + lastRowIndex + '" class="btn btn-danger ra-100"  onclick="DeleteRow(this)"><i class="glyph-icon icon-minus-square" title="" data-original-title=".icon-minus-square" aria-describedby="tooltip941007">  Remove</i></a>';
                            newHtml += '</td>';
                        }

                        else {
                            //though first row contaain text (<th>Sl No</th>
                            //<th>Contact Name</th>
                            //<th>Contact No</th>)
                            var p = lastRowIndex - 2;
                            var q = lastRowIndex - 1;

                            prev = '[' + p + ']';

                            change = '[' + q + ']';

                           // alert('Prev '+prev+' change '+change);

                            //if (i == 2) { //DatePicker Cell
                            //    newHtml += '<td class="input-group date" style="width:500px;">';
                            //    //datepicker field has id and name both but replace only one at a time so we replace here one time that work for id
                            //    cellInnerHtml = cellInnerHtml.replace(prev, change);
                            //    //alert('replace: ' + cellInnerHtml);
                            //}
                            //else {
                            //    newHtml += '<td>';
                            //}
                           // alert('cellInnerHtml' + cellInnerHtml)
                            newHtml += '<td>';
                            newHtml += cellInnerHtml.replace(prev, change);
                           // alert(newHtml);
                            newHtml = newHtml.replace(prev, change);
                           // alert(newHtml);
                            newHtml += '</td>';
                        }
                    }
                }

                newHtml += '</tr>';
                //alert(newHtml);

                $('#dataTable').append(newHtml);
            });

//delete a row
function DeleteRow(o) {
    //alert('hited');            
    //no clue what to put here?
    var p = o.parentNode.parentNode;
    p.parentNode.removeChild(p);
}
