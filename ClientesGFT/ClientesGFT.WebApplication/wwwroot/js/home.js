(function ($) {

    const startDateElement = document.getElementById("Filter_StartDate");
    const endDateElement = document.getElementById("Filter_EndDate");

    const table = $('#fluxosTable').DataTable({
        language: {
            lengthMenu: "Mostrando _MENU_ registros por página",
            zeroRecords: "Desculpa, nada encontrado.",
            info: "Mostrando página _PAGE_ de _PAGES_",
            infoEmpty: "Nenhum registro disponível.",
            search: "Buscar:",
            paginate: {
                first: "Primeiro",
                last: "Último",
                next: ">",
                previous: "<",
            },
        },
        order: [],
        searching: false
    });;


    $('#btnSearch').on('click', function (evt) {

        evt.preventDefault();
        evt.stopPropagation();

        const $containerFluxos = $('#containerFluxos');
        const url = $containerFluxos.data('url');

        const filter = {
            CPF: document.getElementById("Filter_CPF").value,
            Name: document.getElementById("Filter_Name").value,
            StartDate: startDateElement.value,
            EndDate: endDateElement.value,
            Status: Number(document.getElementById("Filter_Status").value)
        };

        $.ajax({
            type: "POST",
            url,
            data: JSON.stringify(filter),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                table.clear();
                if (data != null) {
                    data.forEach(row => {
                        table.row.add([row.userName,row.clientCPF, row.clientName,row.status, row.createDate]);
                    })
                }
                table.draw();

                verifyIfCanDownload();
            }
        });

    });

    startDateElement.addEventListener('change', (event) => { fixDates(); });
    endDateElement.addEventListener('change', (event) => { fixDates(); });

    function initializeDates() {
        const date = new Date();
        const year = date.getFullYear();
        const month = date.getMonth();
        const day = date.getDate();

        const now = new Date(year, month, day);
        const nowLess100years = new Date(year - 100, month, day);

        startDateElement.min = nowLess100years.toISOString().split("T")[0];
        endDateElement.max = now.toISOString().split("T")[0];
    }

    function fixDates() {
        endDateElement.min = startDateElement.value;
        startDateElement.max = endDateElement.value;
    }

    function verifyIfCanDownload() {
        const downloadButton = document.getElementById("btnDownload");

        const hasRows = $("tbody tr .dataTables_empty").length === 0;
        if (hasRows) {
            downloadButton.disabled = false;
        } else {
            downloadButton.disabled = true;
        }
    }

    initializeDates();
    fixDates();
    verifyIfCanDownload();


    $("#filterForm").on("keypress", function (event) {
        var keyPressed = event.keyCode || event.which;
        if (keyPressed === 13) {
            event.preventDefault();
            $('#btnSearch').click();
            return false;
        }
    });

})(jQuery)