$(document).ready(function () {
    var url = window.location.search;
    if (url.includes('inprocess')) {
        LoadProductTable('inprocess');
    } else if (url.includes('pending')) {
        LoadProductTable('pending');
    } else if (url.includes('canelled')) {
        LoadProductTable('cancelled');
    } else if (url.includes('approved')) {
        LoadProductTable('approved');
    } else {
        LoadProductTable('all');
    }
});

function LoadProductTable(status) {
    dataTable = $('#order-table').DataTable({
        ajax: {
            url: '/admin/order/getall?status=' + status
        },
        columns: [
            { data: 'id', "width": "10%" },
            {
                data: null,
                render: function (data) {
                    return `${data.firstName} ${data.lastName}`;
                },
                "width": "20%"
            },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'applicationUser.email', "width": "20%" },
            { data: 'orderStatus', "width": "10%" },
            {
                data: 'orderTotal',
                render: function (data) {
                    return `$${parseFloat(data).toFixed(2)}`;
                },
                "width": "10%"
            },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/admin/order/details?orderId=${data}" class="btn rounded px-3 btn-primary mx-2">
                                <i class="bi bi-info-circle"></i> Details
                            </a>
                        </div>`;
                },
                "width": "15%"
            }
        ]
    });
}
