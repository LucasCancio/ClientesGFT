(function ($) {

    let roles = [];

    const selectRole = document.getElementById("selectRole");
    const btnAddRole = document.getElementById("btnAddRole");

    function deleteRole(id) {
        roles = roles.filter((role) => role.id != id);
        drawTable();
    }

    function drawTable() {
        const table = $("#tbRoles");

        $("#tbRoles tbody").empty();

        roles.forEach((role, index) => {
            table.append(`<tr class="text-center">
                                 <td class="role">
                                    <span>${role.description}</span>
                                    <input id="RolesIds_${index}" name="RolesIds[${index}]" value="${role.id}" class="form-control" readonly type="hidden" />
                                 </td>
                                 <td>
                                 <button data-role-id="${role.id}" type="button" class="deleteRole btn btn-light-outline text-danger">
                                    <i class="fas fa-trash"></i>
                                    Excluir
                                 </button>
                               </td>
                          </tr>`);
        });

        setDeleteEvents();

    }

    function setDeleteEvents() {
        let elements = document.getElementsByClassName("deleteRole");

        Array.from(elements).forEach((element) => {
            const id = element.dataset.roleId;
            element.addEventListener("click", () => deleteRole(id))
        });
    }

    let elements = document.getElementsByClassName("role");

    Array.from(elements).forEach((element) => {
        const role = {};

        element.children.forEach(c => {
            if (c.tagName.toLowerCase() == "span") {
                role.description = c.innerText;
            } else {
                role.id = c.value;
            }
        })

        roles.push(role);
    });

    btnAddRole.addEventListener("click", function () {
        const id = selectRole.value;
        const description = selectRole.options[selectRole.selectedIndex].text;

        const alreadyHasRoleInTable = roles.find((role) => role.id == id);

        if (!alreadyHasRoleInTable) {
            roles.push({ description, id});
            drawTable();
        }

        console.log('roles', roles);
    })

    drawTable();



})(jQuery);