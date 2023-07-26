
$(document).ready(function () {
    moment.locale('id');
    $('#maintable').DataTable({
        ajax: {
            url: "https://localhost:7294/api/employees",
            dataType: "JSON",
            dataSrc: "data" //data source -> butuh array of object
        },
        dom: "<'row'<'col-sm-2'l><'col-sm-5'B><'col-md-5'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-5'p>>",
        buttons: [
            'colvis', 'copy', {
                extend: 'print',
                title: 'Employees',
                text: 'Print',
                //Columns to export
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excelHtml5',
                title: 'Employees',
                className: 'btn btn-success',
                text: 'Excel',
                //Columns to export
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                extend: 'pdfHtml5',
                title: 'Employees',
                text: 'PDF',
                //Columns to export
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                }
            }
        ],
        autoWidth: false,

        columns: [
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: "nik" },
            {
                data: 'fullName',
                render: function (data, type, row) {
                    return row.firstName + ' ' + row.lastName;
                }
            },
            {
                data: 'birthDate',
                render: function (data, type, row) {
                    return moment(data).format("DD MMMM YYYY");
                }
            },
            {
                "data": function (row) {
                    if (row.gender == "0") {
                        return "Female"
                    }
                    else {
                        return "Male"
                    }
                }
            },
            {
                data: "hiringDate",
                render: function (data, type, row) {
                    return moment(data).format("DD MMMM YYYY");
                }
            },
            { data: "email" },
            { data: "phoneNumber" },
            {
                data: null,
                render: function (data, type, row) {
                    return `<button onclick="Update('${row.guid}')" data-bs-toggle="modal" data-bs-target="#modalemp2" class="btn btn-outline-dark" id="actionbtn" style="font-size: 13px;">Update</button>`
                        +
                        `<button id="deleteBtn" onclick="Delete('${row.guid}')" class="btn"><i class="fa fa-trash"></i></button>`;
                }
            }
        ]
    });
});

function Insert() {
    /* //ini ngambil value dari tiap inputan di form nya
     let firstName = $("#firstName").val();
     let lastName = $("#lastName").val();
     let birthDate = $("#birthDate").val();
     let gender = $("input[name='gender']:checked").val();
     let genderEnum;
     if (gender == "Female") {
         genderEnum = 0;
     } else {
         genderEnum = 1;
     }
     let hiringDate = $("#hiringDate").val();
     let email = $("#email").val();
     let phone = $("#phone").val();
     //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
 
     let data = {
         firstName: firstName,
         lastName: lastName,
         birthDate: birthDate,
         gender: genderEnum,
         hiringDate: hiringDate,
         email: email,
         phoneNumber: phone
     };*/

    let data = {
        firstName: $("#firstName").val(),
        lastName: $("#lastName").val(),
        birthDate: $("#birthDate").val(),
        gender: $("input[name='gender']:checked").val() === "Female" ? 0 : 1,
        hiringDate: $("#hiringDate").val(),
        email: $("#email").val(),
        phoneNumber: $("#phone").val(),
    };
    $.ajax({
        url: "https://localhost:7294/api/employees",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),//jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
    }).done((result) => {
        Swal.fire(  //buat alert pemberitahuan jika success
            'Good job!',
            'Data has been successfully inserted!',
            'success'
        ).then(() => {
            location.reload();
        }); 
    }).fail((error) => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Failed to insert data! Please try again.'
        }) //alert pemberitahuan jika gagal
    })
}
function Delete(Guid) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "https://localhost:7294/api/employees/?guid=" + Guid,
                type: "DELETE",
            }).done((result) => {
                Swal.fire(
                    'Deleted!',
                    'Your data has been deleted.',
                    'success'
                ).then(() => {
                    location.reload();
                });
            }).fail((error) => {
                alert("Failed to delete data. Please try again.");
            });
        }
    });
}

function Update(guid) {
    $.ajax({
        url: "https://localhost:7294/api/employees/" + guid,
        type: "GET",
        dataType: "json"
    }).done((result) => {
        // Mengisi nilai form dengan data yang diterima dari server
        $("#guidUpd").val(result.data.guid);
        $("#nikUpd").val(result.data.nik);
        $("#firstNameUpd").val(result.data.firstName);
        $("#lastNameUpd").val(result.data.lastName);
        $("#birthDateUpd").val(moment(result.data.birthDate).format("yyyy-MM-DD"));
        // Melakukan penyesuaian untuk nilai gender
        if (result.data.gender === 0) {
            $("input[name='gender'][value='Female']").prop("checked", true);
        } else {
            $("input[name='gender'][value='Male']").prop("checked", true);
        }
        $("#hiringDateUpd").val(moment(result.data.hiringDate).format("yyyy-MM-DD"));
        $("#emailUpd").val(result.data.email);
        $("#phoneUpd").val(result.data.phoneNumber);

        // Menampilkan modal update data employee
        $("#modalemp2").modal("show");
    }).fail((error) => {
        alert("Failed to fetch employee data. Please try again.");
    });
}

function SaveUpdate() {
    let data = {
        guid: $("#guidUpd").val(),
        nik: $("#nikUpd").val(),
        firstName: $("#firstNameUpd").val(),
        lastName: $("#lastNameUpd").val(),
        birthDate: $("#birthDateUpd").val(),
        gender: $("input[name='gender']:checked").val() === "Female" ? 0 : 1,
        hiringDate: $("#hiringDateUpd").val(),
        email: $("#emailUpd").val(),
        phoneNumber: $("#phoneUpd").val(),
    };
    $.ajax({
        url: "https://localhost:7294/api/employees/",
        type: "PUT",
        contentType: "application/json",
        data: JSON.stringify(data)
    }).done((result) => {
        Swal.fire(  //buat alert pemberitahuan jika success
            'Good job!',
            'Data has been successfully updated!',
            'success'
        ).then(() => {
            location.reload();
        });
    }).fail((error) => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Failed to insert data! Please try again.'
        }) //alert pemberitahuan jika gagal
    })
}
function showGenderChart() {
    // Mendapatkan data dari API
    $.ajax({
        url: "https://localhost:7294/api/employees/", // Sesuaikan URL sesuai dengan endpoint API Anda
        type: "GET",
        dataType: "json"
    }).done(res => {
        // Mendapatkan jumlah jenis kelamin
        let femaleCount = 0;
        let maleCount = 0;
        for (let i = 0; i < res.data.length; i++) {
            if (res.data[i].gender === 0) {
                femaleCount++;
            } else if (res.data[i].gender === 1) {
                maleCount++;
            }
        }
        // Menghitung total data
        let totalCount = femaleCount + maleCount;

        // Menghitung persentase jenis kelamin
        let femalePercentage = (femaleCount / totalCount) * 100;
        let malePercentage = (maleCount / totalCount) * 100;

        // Membuat grafik menggunakan Chart.js
        let ctx = document.getElementById('genderChart').getContext('2d');
        let genderChart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ['Female', 'Male'],
                datasets: [{
                    data: [femalePercentage, malePercentage],
                    backgroundColor: ['#DA1212', '#11468F'],
                    hoverBackgroundColor: ['#FF6384', '#36A2EB']
                }]
            },
            options: {
                responsive: true,
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            let label = data.labels[tooltipItem.index];
                            let value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
                            return label + ': ' + value.toFixed(2) + '% (' + Math.round(value * totalCount / 100) + ')';
                        }
                    }
                }
            }
        });
        // Membuka modal setelah grafik selesai dibuat
        $('#modalChart').modal('show');
    }).fail(error => {
        alert("Failed to fetch data from API.");
    });
}

