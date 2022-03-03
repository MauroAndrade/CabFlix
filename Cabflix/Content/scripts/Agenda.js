

//Carrega Modal
function EditAgendaCarregaModal(id) {

    $.ajax({
        contentType: "html",
        type: "GET",
        url: '/Agenda/Edit/?id=' + id,
        success: function (data) {
            $("#modal-edit-agenda").html(data);
        }
    });
};

function CreateAgendaCarregaModal() {

    $.ajax({
        contentType: "html",
        type: "GET",
        url: '/Agenda/Create',
        success: function (data) {
            $("#modal-create-agenda").html(data);
        }
    });
};

//Salva Alterações
function EditAgenda() {

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
        url: '/Agenda/Edit',
        data: { 'Id': Id, 'FkPai': FkPai, 'Nome': Nome, 'Classificacao': Classificacao },
        success: function (data) {
            if (data['Status'].includes('Sucesso!')) {

                toastr.success('Salvo com sucesso!', 'Agenda');

                setTimeout(function () {
                    $("[data-dismiss=modal]").trigger({ type: "click" });
                    window.location.reload();
                }, 1200);

            } else {
                toastr.error(data['Message'], 'Agenda');
            }
        }
    });
};

function CreateAgenda() {

    var Id = $('#Id').val();
    var Nome = $('#Nome').val();
    var Telefone = $('#Telefone').val()
    var FkClassificacao = $("#ClassificacaoLigacao :selected").val();
    var Individual = $("input[type=checkbox]").prop("checked"); 
   

    if (Nome == null || Nome == undefined || Nome == "") {
        toastr.error('Informe o Nome!', 'Agenda');
        $('#Nome').focus();
        return;
    } else if (Telefone == null || Telefone == undefined || Telefone == "") {
        toastr.error('Informe o Telefone!', 'Agenda');
        $('#Telefone').focus();
        return;
    }
    else if (FkClassificacao == null || FkClassificacao == undefined || FkClassificacao == "") {
        toastr.error('Informe a Classificação!', 'Agenda');
        $('#ClassificacaoLigacao').focus();
        return;
    }

    $.ajax({
        type: "POST",
        url: '/Agenda/Create',
        data: { 'Id': Id, 'Nome': Nome, 'Telefone': Telefone, 'FkClassificacao': FkClassificacao, 'Individual': Individual },
        success: function (data) {
            if (data['Status'].includes('Sucesso!')) {

                toastr.success('Salvo com sucesso!', 'Agenda');

                setTimeout(function () {
                    $("[data-dismiss=modal]").trigger({ type: "click" });
                    window.location.reload();
                }, 1200);

            } else {
                toastr.error(data['Message'], 'Agenda');
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

