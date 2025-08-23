$(document).ready(function () {
  if ($(".toast-container").length === 0) {
    $("body").append(
      '<div class="toast-container position-fixed top-0 end-0 p-3" style="z-index:9999;"></div>'
    );
  }

  const csrfToken = $('input[name="__RequestVerificationToken"]').val();


  $(document).on(
    "click",
    ".card-actions .btn, .card-actions .quantity-selector, .card-actions input",
    function (e) {
      e.stopPropagation();
    }
  );

  $(document).on("click", ".add-to-cart-btn", function (e) {
    e.stopPropagation(); 
    var productId = $(this).data("product-id");

    $(".quantity-selector").stop(true, true).fadeOut(150);

    $(".add-to-cart-btn").show();

    $('.quantity-selector[data-product-id="' + productId + '"]')
      .stop(true, true)
      .fadeIn(200);
    $(this).hide();
  });

  $(document).on("click", ".cancel-add-btn", function (e) {
    e.stopPropagation(); 
    var productId = $(this).data("product-id");
    $('.quantity-selector[data-product-id="' + productId + '"]')
      .stop(true, true)
      .fadeOut(150);
    $('.add-to-cart-btn[data-product-id="' + productId + '"]').show();
  });

  $(document).on("click", ".confirm-add-btn", function (e) {
    e.stopPropagation(); 
    var productId = $(this).data("product-id");
    var quantity = $(
      '.quantity-input[data-product-id="' + productId + '"]'
    ).val();

    $.ajax({
      type: "POST",
      url: "/Customer/Home/AddToCart",
      data: {
        productId: productId,
        count: quantity,
        __RequestVerificationToken: csrfToken,
      },
      success: function (response) {
        if (response.success) {
          $(".cart-counter").text(response.cartCount);
          showToast(response.message, "success");
          $.cartCounter(response.cartCount);
        } else {
          showToast(response.message, "error");
        }

        // Hide selector again
        $('.quantity-selector[data-product-id="' + productId + '"]')
          .stop(true, true)
          .fadeOut(150);
        $('.add-to-cart-btn[data-product-id="' + productId + '"]').show();
      },
      error: function (xhr) {
        if (xhr.status === 401) {
          window.location.href =
            "/Identity/Account/Login?returnUrl=" +
            encodeURIComponent(window.location.pathname);
        } else {
          showToast("Something went wrong. Please try again.", "error");
        }
      },
    });
  });
});

function showToast(message, type) {
  const icon = type === "success" ? "check-circle" : "exclamation-circle";
  const bgClass = type === "success" ? "bg-success" : "bg-danger";

  const toastHtml = `
        <div class="toast align-items-center text-white ${bgClass} border-0 mb-2" role="alert" aria-live="assertive" aria-atomic="true">
          <div class="d-flex">
            <div class="toast-body">
              <i class="bi bi-${icon} me-2"></i>${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
          </div>
        </div>
    `;

  const $toast = $(toastHtml);
  $(".toast-container").append($toast);
  $toast[0].offsetHeight;

  const toast = new bootstrap.Toast($toast[0], { delay: 3000 });
  toast.show();

  $toast.on("hidden.bs.toast", function () {
    $(this).remove();
  });
}
