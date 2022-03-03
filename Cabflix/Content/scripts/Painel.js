
$(document).ready(function () {

    //ADICIONA DIAS A UMA DATA
    function addDays(date, days) {
        var result = new Date(date);
        result.setDate(result.getDate() + days);
        return result;
    };

    //DATE RANGE PICKER
    //$('#referencia').daterangepicker({
    //    startDate: moment().subtract(7, 'days'),
    //    endDate: addDays(Date.now(), 7),
    //    autoUpdateInput: true,
    //    locale: {
    //        format: 'DD/MM/YYYY',
    //        applyLabel: 'Confirma',
    //        cancelLabel: 'Cancela',
    //    }
    //});

    $(".datepicker").datepicker({
        format: "mm/yyyy",
        language: "pt-BR",
        startView: "months",
        minViewMode: "months",
        //startDate: "+0d",
        autoclose: true,
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down",
            previous: "fa fa-angle-left",
            next: "fa fa-angle-right",
            today: "fa fa-thumb-tack",
            clear: "fa fa-trash"
        }
    });

    $(".RelatorioDetalhamento").click(function (e) {

        var form = document.createElement("form");

        var data = new Date();
        data = "ViewIndex" + data.getHours() + data.getMinutes() + data.getSeconds() + data.getMilliseconds();

        form.method = "post";
        form.action = '/Relatorio/RelatorioUsuarioDetalhado',
            form.setAttribute("target", data);

        var hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "json");
        hiddenField.setAttribute("name", "pData");
        hiddenField.setAttribute("hidden", true);
        hiddenField.setAttribute("value", JSON.stringify({ filtro: "parametro" }));

        form.appendChild(hiddenField);

        document.body.appendChild(form);
        window.open('', data, '', false);

        form.submit();

    });

});

function RelatorioAcessoDetalhado(id) {

    var conta = $('#conta').val();
    var numero = $('#numero').val();
    var mes = $('#datepicker').val();

    var form = document.createElement("form");

    var data = new Date();
    data = "ViewIndex" + data.getHours() + data.getMinutes() + data.getSeconds() + data.getMilliseconds();

    form.method = "post";
    form.action = '/Relatorio/RelatorioUsuarioDetalhado',
        form.setAttribute("target", data);

    var hiddenField = document.createElement("input");
    hiddenField.setAttribute("type", "json");
    hiddenField.setAttribute("name", "pData");
    hiddenField.setAttribute("hidden", true);
    hiddenField.setAttribute("value", JSON.stringify({ idContato: id, conta: conta, numero: numero, mes: mes }));

    form.appendChild(hiddenField);

    document.body.appendChild(form);
    window.open('', data, '', false);

    form.submit();

};