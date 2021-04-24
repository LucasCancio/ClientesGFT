$(function () {
    $("input[mask='cpf']").mask("000.000.000-00");
    $("input[mask='cep']").mask("00000-000");

    const behavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    };

    const options = {
            onKeyPress: function (val, e, field, options) {
                field.mask(behavior.apply({}, arguments), options);
            }
    };

    $("input[mask='telefone']").mask(behavior, options);
});