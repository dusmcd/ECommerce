// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getProductsForOrder(id, element) {
    $.ajax(`/Orders/GetProductsFromOrder/${id}`, {
        dataType: "json",
        method: "GET",
        error: (jqXHR, textStatus, errorThrown) => {
            console.error(errorThrown);
        },
        success: (data, textStatus, jqXHR) => {
            // show data
            element.find(".spinner-border").hide();
            element.find(".accordion-body").append("<p>Order and product data</p>")
            console.log("data from server: ", data);
        }

    })
}

$(function () {
    $(".accordion-item").on("click", function(evt) {
        const element = $(this);
        if (evt.target.className === "accordion-button") {
            const id = element.data("id");
            getProductsForOrder(id, element);
        }
        
    });
})
