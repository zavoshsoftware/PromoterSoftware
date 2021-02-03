 

function locationStart(id) {
    if (navigator.geolocation) {
        alert("it is supported");
        navigator.geolocation.getCurrentPosition(function (position) {
            console.log(position);
            console.log(position.coords.latitude);
            console.log(position.coords.longitude);

            SubmitLocation(position.coords.latitude, position.coords.longitude, 'start',id);

        });

    }
    else {
        console.log("geo location is not supported");
    }
}

function locationEnd(id)  {

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            console.log(position);
            console.log(position.coords.latitude);
            console.log(position.coords.longitude);

            SubmitLocation(position.coords.latitude, position.coords.longitude, 'end',id);

        });

    }
    else {
        console.log("geo location is not supported");
    }
} 



function SubmitLocation(latitude, longitude, requestType,id) {
    $.ajax(
        {
            url: "/DailyPromoterPlans/SubmitLocation",
            data: { latitude: latitude, longitude: longitude, requestType: requestType, id: id },
            type: "POST"
        }).done(function (result) {
        if (result === "true") {
            location.reload();
        }
        else  {
            alert("خطایی رخ داده است");
        }
    });

}


function updateSell(productId, dailyPromoterPlanId) {
    var qty = $('#qty_' + productId).val();
    var desc = $('#desc_' + productId).val();

    if (qty === '') {
        $('#alert-success_' + productId).css('display', 'none');
        $('#alert-danger_' + productId).css('display', 'inline-block');
    } else {
        $.ajax(
            {
                url: "/DailyPromoterProductSales/SubmitSale",
                data: { productId: productId, dailyPromoterPlanId: dailyPromoterPlanId, qty: qty, desc: desc },
                type: "POST"
            }).done(function(result) {
            if (result === "true") {
                $('#alert-success_' + productId).css('display', 'inline-block');
                $('#alert-danger_' + productId).css('display', 'none');
            } else {
                $('#alert-success_' + productId).css('display', 'none');
                $('#alert-danger_' + productId).css('display', 'inline-block');
            }
        });
    }
}