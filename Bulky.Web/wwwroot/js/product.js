$(document).ready(function () {
    LoadProductTable();
});

function LoadProductTable() {
    dataTable = $('#product-table').DataTable({
        paging: true,
        processing: true,
        serverSide: true,
        ajax: {
            url: '/admin/product/getall'
        },
        columns: [
            { data: 'title', "width": "15%" },
            { data: 'author', "width": "10%" },
            { data: 'category.name', "width": "10%" },
            { data: 'listPrice', "width": "8%" },
            { data: 'displayOrder', "width": "8%" },
            { data: 'isHidden', "width": "8%" },
            { data: 'isbn', "width": "12%" },
            { data: 'createdAt', "width": "12%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/admin/product/upsert?id=${data}" class="btn rounded px-3 btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit 
                            </a>               
                            <a onClick=Delete("/admin/product/delete?id=${data}") class="btn rounded btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>               
                        </div>`;
                },
                "width": "17%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#325d88",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    Swal.fire({
                        title: "Deleted!",
                        text: data.message || "The item was deleted successfully.",
                        icon: "success"
                    });
                    $('#product-table').DataTable().ajax.reload();
                },
                error: function (xhr) {
                    let errorMsg = "Something went wrong."
                    if (xhr.responseJSON && xhr.responseJSON.message) {
                        errorMsg = xhr.responseJSON.message;
                    } else if (xhr.responseText) {
                        try {
                            const parsed = JSON.parse(xhr.responseText);
                            if (parsed.message) errorMsg = parsed.message;
                        } catch {
                            errorMsg = xhr.responseText;
                        }
                    }
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: errorMsg
                    });
                }
            });
        }
    });
}

