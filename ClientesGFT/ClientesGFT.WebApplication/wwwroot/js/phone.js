(function ($) {

    let phones = [];

    const inputPhone = document.getElementById("inputPhone");
    const btnAddPhone = document.getElementById("btnAddPhone");

    function deletePhone(number) {
        phones = phones.filter((phone) => phone != number);
        drawTable();
    }

    function drawTable() {
        const table = $("#tbPhones");

        $("#tbPhones tbody").empty();

        phones.forEach((phone, index) => {
            table.append(`<tr>
                              <td>
                                    ${phone}
                                   <input id="PhonesNumbers_${index}" name="PhonesNumbers[${index}]" value="${phone}" class="form-control phone" readonly type="hidden" />
                              </td>
                              <td>
                                   <button data-phone="${phone}" type="button" class="deletePhone btn btn-light-outline text-danger">
                                    <i class="fas fa-trash"></i>
                                     Excluir
                                   </button>
                              </td>
                          </tr>`);
        });

        setDeleteEvents();

    }

    function setDeleteEvents() {
        let elements = document.getElementsByClassName("deletePhone");

        Array.from(elements).forEach((element) => {
            const number = element.dataset.phone;
            element.addEventListener("click", () => deletePhone(number))
        });
    }

    let elements = document.getElementsByClassName("phone");

    Array.from(elements).forEach((element) => {
        const number = element?.value;

        phones.push(number);
    });

    if (btnAddPhone && inputPhone) {
        inputPhone.addEventListener("keyup", function (event) {
            const value = inputPhone.value;
            const fixedValue = value?.replace("(", "").replace(")", "").replace("-", "").replace(" ", "");

            const isValid = fixedValue.length > 9;

            if (isValid) btnAddPhone.disabled = false;
            else btnAddPhone.disabled = true;
        });

        btnAddPhone.addEventListener("click", function () {
            const number = inputPhone.value;

            const alreadyHasPhoneInTable = phones.find((phone) => phone == number);

            if (!alreadyHasPhoneInTable) {
                phones.push(number);
                drawTable();
            }

            inputPhone.value = "";
        })

        drawTable();
    }

    

})(jQuery);