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
            let list = "<ul>";
            data.forEach(product => {
                const content = `Name: ${product.name}, Description: ${product.description}, Price: $${product.price}, Quantity: ${product.quantity}`;
                const listItem = `<li>${content}</li>`;
                list += listItem;
            });
            list += "</ul>";

            element.find(".spinner-border").hide();
            element.find(".accordion-body").append(list);
            element.data("fetched", true);
        }

    });
}

$(function () {
    $(".accordion-item").on("click", function(evt) {
        const element = $(this);
        if (element.data("fetched")) {
            return;
        }
        if (evt.target.className === "accordion-button") {
            const id = element.data("id");
            getProductsForOrder(id, element);
        }
        
    });
})
