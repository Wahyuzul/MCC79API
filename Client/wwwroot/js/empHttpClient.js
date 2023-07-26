$(document).ready(function () {
    moment.locale('id');
    $('#indexTable').DataTable({
        dom: "<'ui grid'" +
            "<'row'" + "<'col-3'l>" + "<'col-6 mt--2'B>" + "<'col-3'f>" + ">" +
            "<'row dt-table'" + "<'col'tr>" + ">" +
            "<'row'" + "<'col-4'i>" + "<'col-8'p>" + ">" +
            ">",
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
    });
});