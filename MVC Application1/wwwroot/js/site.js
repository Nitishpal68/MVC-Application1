
    
 $(".Btn").on("click", function () {
     var id = $(this).attr("id");


     $.ajax({
         type: 'DELETE',
         url: '/Time/Delete/' + id,
         contentType: false,
         processData: false,
         cache: false,
         success: successCallback,
         error: errorCallback
     });


     return false;

 });
function errorCallback() {
    alert("Something went wrong please contact admin.");
}
function successCallback(response) {
    window.location.href = "/Time/Index";
    debugger;
   


}

$(function () {
    $("#addButton").on("click", function () {
        alert("Sending Ajax Call");


        var formData = new FormData();

        formData.append("employeeID", $("#employeeID").val())
        formData.append("Date", $("#Date").val());
        formData.append("StartTime", $("#StartTime").val());

        formData.append("EndTime", $("#EndTime").val());
        formData.append("projectID", $("#projectID").val());
        formData.append("taskID", $("#taskID").val());
        formData.append("Description", $("#Description").val());
        formData.append("WorkStatus", $("#WorkStatus").val());

        $.ajax({
            type: 'POST',
            url: '/Time/Add',
            contentType: false,
            processData: false,
            cache: false,
            data: formData,
            success: successCallback,
            error: errorCallback
        });

        return false;
    });
    function errorCallback() {
        alert("Something went wrong please contact admin.");
    }
    function successCallback(response) {
        window.location.href = "/Time/Index";
        debugger;
        alert("Data Save");
        $('#myForm').trigger("reset");

    }
});



