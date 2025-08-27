$(document).ready(function () {
    LoadCompanyTable();
});

function LoadCompanyTable() {
    dataTable = $('#store-table').DataTable({
        paging: true,
        ajax: {
            url: '/admin/store/getall'
        },
        columns: [
            { data: 'name', "width": "20%" },           
            { data: 'phoneNumber', "width": "15%" },   
            { data: 'state', "width": "20%" },           
            { data: 'city',  "width": "10%"},
            { data: 'streetAddress', "width": "15%" },      
            {
                data: 'id',                              
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/admin/store/upsert?id=${data}" class="btn rounded px-3 btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit 
                            </a>               
                            <a onClick=Delete("/admin/store/delete?id=${data}") class="btn rounded btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>               
                        </div>`;
                },
                "width": "20%"
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
                        text: data.message || "The company was deleted successfully.",
                        icon: "success"
                    });
                    $('#company-table').DataTable().ajax.reload();
                },
                error: function (xhr) {
                    let errorMsg = "Something went wrong.";
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
