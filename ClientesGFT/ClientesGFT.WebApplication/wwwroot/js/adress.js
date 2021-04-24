(function ($) {
    $.validator.setDefaults({ ignore: null });

    $(document).ready(function () {
        const initialCountryId = $("#CountryId").val();
        const initialStateId = $("#StateId").val();
        const initialCityId = $("#CityId").val();

        if (initialCountryId && initialStateId && initialCityId) {
            $("#countriesList").val(initialCountryId);
            const countryName = $("#countriesList option:selected").text();
            document.getElementById("RG").disabled = (countryName.toLowerCase() != "brasil");

            resetcitiesList(initialCityId);
            resetstatesList(initialStateId);

            let url = `/adress/states/${initialCountryId}`;
            $.getJSON(url, function (data) {
                fillSelectBox(data, "statesList");
                $("#statesList").val(initialStateId);
            })

            url = `/adress/cities/${initialStateId}`;
            $.getJSON(url, function (data) {
                ;
                fillSelectBox(data, "citiesList");
                $("#citiesList").val(initialCityId);
            });
        }
    })





    document.getElementById("countriesList").addEventListener("change", function () {
        const countryId = $("#countriesList").val();
        const countryName = $("#countriesList option:selected").text();

        const url = `/adress/states/${countryId}`;

        $.getJSON(url, function (data) {
            resetcitiesList();
            resetstatesList();

            fillSelectBox(data, "statesList");

            document.getElementById("CountryId").value = countryId;
        });

        document.getElementById("RG").disabled = (countryName.toLowerCase() != "brasil");
    })

    document.getElementById("statesList").addEventListener("change", function () {
        const stateId = $("#statesList").val();
        if (stateId) {
            const url = `/adress/cities/${stateId}`;

            $.getJSON(url, function (data) {
                resetcitiesList();

                fillSelectBox(data, "citiesList");

                document.getElementById("StateId").value = stateId;
            });
        }
    })

    document.getElementById("citiesList").addEventListener("change", function () {
        const cityId = this.value;
        document.getElementById("CityId").value = cityId;
    })



    function fillSelectBox(data, selectBoxId) {
        let html = '';

        $.each(data, function (i, element) {
            html += '<option value="' + element.value + '">' + element.text + '</option>'
        });

        $(`#${selectBoxId}`).append(html);
    }


    function resetcitiesList(defaultCityId = "") {
        const citiesList = $("#citiesList");

        citiesList.empty();
        const item = '<option disabled selected>- Selecione a Cidade -</option>';
        citiesList.html(item);
        document.getElementById("CityId").value = defaultCityId;
    }

    function resetstatesList(defaultStateId = "") {
        const statesList = $("#statesList");

        statesList.empty();
        const item = '<option disabled selected>- Selecione o Estado -</option>';
        statesList.html(item);
        document.getElementById("StateId").value = defaultStateId;
    }
})(jQuery);