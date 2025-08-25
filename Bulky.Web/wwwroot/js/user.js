$(document).ready(function () {
    LoadUserTable();
});

function LoadUserTable() {
    dataTable = $('#user-table').DataTable({
        paging: true,
        ajax: {
            url: '/admin/user/getall'
        },
        columns: [
            {
                data: null,
                render: data => `${data.firstName} ${data.lastName}`,
                width: "15%"
            },
            { data: 'email', "width": "15%" },
            { data: 'role', "width": "15%" },
            {
                data: 'company.name',
                "render": function (data) {
                    return data ? data : 'N/A';
                },
                "width": "15%"
            },
            { data: 'phoneNumber', "width": "13%" },
            {
                data: { id: 'id', lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                        <div class="text-center">
                             <a onclick=LockUnlock('${data.id}') class="btn btn-danger my-3 text-white" style="cursor:pointer; width:150px;">
                                    <i class="bi bi-lock-fill"></i>  Lock
                                </a> 
                                <a href="/admin/user/RoleManagement?userId=${data.id}" class="btn btn-danger my-3 text-white" style="cursor:pointer; width:150px;">
                                     <i class="bi bi-pencil-square"></i> Permission
                                </a>
                        </div>
                    `
                    }
                    else {
                        return `
                        <div class="text-center">
                              <a onclick=LockUnlock('${data.id}') class="btn btn-success my-3 text-white" style="cursor:pointer; width:150px;">
                                    <i class="bi bi-unlock-fill"></i>  UnLock
                                </a>
                                <a href="/admin/user/RoleManagement?userId=${data.id}" class="btn my-3 btn-danger text-white" style="cursor:pointer; width:150px;">
                                     <i class="bi bi-pencil-square"></i> Permission
                                </a>
                        </div>
                       `
                    }
                },
                "width": "25%"
            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}