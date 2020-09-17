$(document).ready(function () {
    $("body").on("click", "#tblCustomers .Delete", function () {
        if (confirm("Do you want to delete this row?")) {
            var row = $(this).closest("tr");
            row.find(".Delete").hide();
            row.remove();
            var Id = $(this).attr("data-id");
            $.ajax({
                type: "POST",
                url: "/Home/Delete",
                data: '{Id: ' + Id + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    row.find(".Delete").hide();
                    if ($("#tblCustomers tr").length > 2) {
                        row.remove();
                    } else {
                        row.find(".Delete").hide();
                        row.find("span").html('&nbsp;');
                    }
                }
            });
        }
    });
});