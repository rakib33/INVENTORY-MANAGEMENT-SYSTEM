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
                            //alert(list.Value);
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
        $("#Catagory_Id").change(function () {

            // alert('hited' + $("#MainCatagory").val());
            $("#Brand_Id").empty();
            try {
                $.ajax({
                    type: 'POST',
                    url: '/Data/getBrand',

                    dataType: 'json',
                    data: { id: $("#Catagory_Id").val() },

                    success: function (lists) {
                        $.each(lists, function (i, list) {
                            $("#Brand_Id").append('<option value="' + list.Value + '">' + list.Text + '</option>');
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
