

//Carrega Modal
function EditCentroCustoCarregaModal(id) {

    $.ajax({
        contentType: "html",
        type: "GET",
        url: '/CentroCusto/Edit/?id=' + id,
        success: function (data) {
            $("#modal-edit-centrocusto").html(data);
        }
    });
};

function CreateCentroCustoCarregaModal() {

    $.ajax({
        contentType: "html",
        type: "GET",
        url: '/CentroCusto/Create',
        success: function (data) {
            $("#modal-create-centrocusto").html(data);
        }
    });
};

//Salva Alterações
function EditCentroCusto() {

    var Id = $('#Id').val();
    var FkPai = $("#CentroCusto :selected").val();
    var Nome = $('#Nome').val();
    var Classificacao = $('#Classificacao').val();

    if (Id == null || Id == undefined) {
        toastr.error('Erro! ID Inexistente!', 'Centro de Custo');
        return;
    } else if (Nome == null || Nome == undefined || Nome == "") {
        toastr.error('Informe o Nome!', 'Centro de Custo');
        $('#Nome').focus();
        return;
    } else if (Classificacao == null || Classificacao == undefined || Classificacao == "") {
        toastr.error('Informe a Classificação!', 'Centro de Custo');
        $('#Classificacao').focus();
        return;
    }

    $.ajax({
        type: "POST",
        url: '/CentroCusto/Edit',
        data: { 'Id': Id, 'FkPai': FkPai, 'Nome': Nome, 'Classificacao': Classificacao },
        success: function (data) {
            if (data['Status'].includes('Sucesso!')) {

                toastr.success('Salvo com sucesso!', 'Centro de Custo');

                setTimeout(function () {
                    $("[data-dismiss=modal]").trigger({ type: "click" });
                    window.location.reload();
                }, 1200);

            } else {
                toastr.error(data['Message'], 'Centro de Custo');
            }
        }
    });
};

function CreateCentroCusto() {

    var Id = $('#Id').val();
    var FkPai = $("#CentroCusto :selected").val();
    var Nome = $('#Nome').val();
    var Classificacao = $('#Classificacao').val();

    if (Nome == null || Nome == undefined || Nome == "") {
        toastr.error('Informe o Nome!', 'Centro de Custo');
        $('#Nome').focus();
        return;
    } else if (Classificacao == null || Classificacao == undefined || Classificacao == "") {
        toastr.error('Informe a Classificação!', 'Centro de Custo');
        $('#Classificacao').focus();
        return;
    }

    $.ajax({
        type: "POST",
        url: '/CentroCusto/Create',
        data: { 'Id': Id, 'FkPai': FkPai, 'Nome': Nome, 'Classificacao': Classificacao },
        success: function (data) {
            if (data['Status'].includes('Sucesso!')) {

                toastr.success('Salvo com sucesso!', 'Centro de Custo');

                setTimeout(function () {
                    $("[data-dismiss=modal]").trigger({ type: "click" });
                    window.location.reload();
                }, 1200);

            } else {
                toastr.error(data['Message'], 'Centro de Custo');
            }
        }
    });
};

function ChangeStatus(id) {
    $.ajax({
        type: "POST",
        url: '/CentroCusto/ChangeStatus',
        data: { 'Id': id },
        success: function (data) {
            if (data['Status'].includes('Sucesso!')) {

                toastr.success('Salvo com sucesso!', 'Centro de Custo');

                setTimeout(function () {
                    $("[data-dismiss=modal]").trigger({ type: "click" });
                    window.location.reload();
                }, 1200);

            } else {
                toastr.error(data['Message'], 'Centro de Custo');
            }
        }
    });
};

