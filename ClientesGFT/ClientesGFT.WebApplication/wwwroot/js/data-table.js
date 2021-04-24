$(document).ready(function () {
    $('table.data-table').each(function () {
        $(this).DataTable({
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
            order: []
        });
    });
});