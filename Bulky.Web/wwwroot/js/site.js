//(function ($) {
//  $.cartCounter = function (count) {
//    var counter = $(".cart-counter");

//    if (!counter.length) return; // no counter in DOM

//    if (count > 0) {
//      counter.text(count).show().addClass("cart-counter-animate");
//    } else {
//      counter.hide();
//    }

//    // remove animation class after it plays
//    setTimeout(() => counter.removeClass("cart-counter-animate"), 400);
//  };
//})(jQuery);

//$(document).ready(function () {
//  $.get("/Customer/Home/GetCartCount", function (response) {
//    $.cartCounter(response.count);
//  });   
//});
if (parseInt($(".cart-counter").text()) === 0) {
    $(".cart-counter").hide();
} else {
    $(".cart-counter").show();
}   