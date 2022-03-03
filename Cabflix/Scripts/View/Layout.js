
$(document).ready(function () {
    bsCustomFileInput.init();

    var url_atual = window.location.href;

    if (url_atual.match("Fatura/Index")) {
        GetOperadora();
        GetConta();
    }
    if (url_atual.match("NumeroLinha/Create")) {
        GetCentroCusto();
    }
    if (url_atual.match("NumeroLinha/Edit")) {
        setTimeout(function () {
            var id = $('.centro-custo-hidden').val();
            GetCentroCusto(id);
        }, 1000);
    }
    if (url_atual.match("Conta/Create")) {


    }
    if (url_atual.match("Detalhamento/Index")) {

        //ADICIONA DIAS A UMA DATA
        function addDays(date, days) {
            var result = new Date(date);
            result.setDate(result.getDate() + days);
            return result;
        };

        //DATE RANGE PICKER
        $('#referencia').daterangepicker({
            startDate: moment().subtract(7, 'days'),
            endDate: addDays(Date.now(), 7),
            autoUpdateInput: true,
            locale: {
                format: 'DD/MM/YYYY',
                applyLabel: 'Confirma',
                cancelLabel: 'Cancela',
            }
        });

        $(".RelatorioDetalhamento").click(function () {

            var form = document.createElement("form");

            var data = new Date();
            data = "ViewIndex" + data.getHours() + data.getMinutes() + data.getSeconds() + data.getMilliseconds();

            form.method = "post";
            form.action = '/Detalhamento/RelatorioUsuarioDetalhado',
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
    }
    if (url_atual.match("CentroCusto/Index")) {

        $('.tree').treegrid();
    }


});

//https://www.jqueryscript.net/loading/loading-indicator-view.html

//$(document).loadingView('.box');

//// or
//$(function () {
//    $('.box').loadingView({
//    'state': true
//    });
//});

//$(function () {
//    $('.box').loadingView({
//    'image': "loadingImage.gif",
//    'imageClassName': "loadingImage"
//    });
//});

//$(function () {
//    $('.box').loadingView({
//        'imageStyle': "", position: absolute, top: 50 %, left: 50 %; transform: translate(-50 %, -50 %); ""
//  });
//});

//$(function () {
//    $('.box').loadingView({
//    'state': false
//    });
//});




function GetOperadora() {

    $.ajax({
        url: '/Operadora/GetOperadora',
        success: function (data) {
            $.each(data, function (i, item) {
                $('#operadora').append($('<option>', {
                    value: item.value,
                    text: item.text,
                    //selected: true
                }));
            });
        }
    });
}

function GetConta() {

    $.ajax({
        url: '/Conta/GetConta',
        success: function (data) {
            $.each(data, function (i, item) {
                $('#conta').append($('<option>', {
                    value: item.value,
                    text: item.text,
                    //selected: true
                }));
            });
        }
    });
}

function GetCentroCusto(id) {

    $.ajax({
        url: '/NumeroLinha/GetCentroCusto',
        success: function (data) {
            $.each(data, function (i, item) {
                var selected = false;
                if (item.value == id) {
                    selected = true
                };
                $('#FkCentroCusto').append($('<option>', {
                    value: item.value,
                    text: item.text,
                    selected: selected
                }));
            });
        }
    });
}

function ExportToTable() {

    var operadora = $('#operadora').val();
    var mes = $('#mes').val();
    var conta = $('#conta').val();

    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.csv)$/;
    //Checks whether the file is a valid csv file
    if (regex.test($("#csvfile").val().toLowerCase())) {
        //Checks whether the browser supports HTML5
        if (typeof (FileReader) != "undefined") {

            var fatura = [];

            var reader = new FileReader();
            reader.onload = function (e) {
                //var table = $("#csvtable > tbody");
                //Splitting of Rows in the csv file
                var csvrows = e.target.result.split("\n");
                for (var i = 1; i < csvrows.length; i++) {
                    if (csvrows[i] != "") {



                        var obj = {

                            operadora: "", tipoServico: "", numNf: "", numOrigem: "", numDestino: "", mes: "", data: "", codigo: "", grupo: "", codigo2: "",
                            servico: "", duracao: "", min: "", dados: "", horario: "", cidade: "", regiao: "", estado: "", pais: "", valor: "", valorCobrado: "",
                            degrau: "", ufOrigem: ""
                        };

                        var linha = csvrows[i].split(";");

                        obj.operadora = linha[0];
                        obj.tipoServico = linha[1];
                        obj.numNf = linha[2];
                        obj.numOrigem = linha[3];
                        obj.numDestino = linha[4];
                        obj.mes = linha[5];
                        obj.data = linha[6];
                        obj.codigo = linha[7];
                        obj.grupo = linha[8];
                        obj.codigo2 = linha[9];
                        obj.servico = linha[10];
                        obj.duracao = linha[11];
                        obj.min = linha[12];
                        obj.dados = linha[13];
                        obj.horario = linha[14];
                        obj.cidade = linha[15];
                        obj.regiao = linha[16];
                        obj.estado = linha[17];
                        obj.pais = linha[18];
                        obj.valor = linha[19];
                        obj.valorCobrado = linha[20];
                        obj.degrau = linha[21];
                        obj.ufOrigem = linha[22];

                        var myjson = JSON.stringify(obj);

                        fatura.push(myjson);




                        //ajax
                        //$.ajax({
                        //    traditional: true,
                        //    url: "/Fatura/UploadFatura",
                        //    type: "POST",
                        //    content: "application/json; charset=utf-8",
                        //    dataType: "json",
                        //    data: { myjson: myjson },
                        //    success: function (response) {
                        //        console.log(response);
                        //    }
                        //});



                    }
                }

                //$('#csvtable').show();

                //ajax
                $.ajax({
                    traditional: true,
                    url: "/Fatura/UploadFatura",
                    type: "POST",
                    content: "application/json; charset=utf-8",
                    dataType: "json",

                    data: { list: fatura, operadora: operadora, mes: mes, conta: conta },
                    success: function (response) {
                        console.log(response);
                        alert('Pronto!');
                    }
                });

            }
            reader.readAsText($("#csvfile")[0].files[0]);
        } else {
            alert("Sorry! Your browser does not support HTML5!");
        }
    } else {
        alert("Please upload a valid CSV file!");
    }



};




$(function () {
    $("#usuario").DataTable({
        responsive: true,
        "sEmptyTable": "Nenhum registro encontrado",
        "Info": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
        "sInfoFiltered": "(Filtrados de _MAX_ registros)",
        "sInfoPostFix": "",
        "sInfoThousands": ".",
        "sLengthMenu": "_MENU_ resultados por página",
        "sLoadingRecords": "Carregando...",
        "sProcessing": "Processando...",
        "sZeroRecords": "Nenhum registro encontrado",
        "sSearch": "Pesquisar",
        "oPaginate": {
            "sNext": "Próximo",
            "sPrevious": "Anterior",
            "sFirst": "Primeiro",
            "sLast": "Último"
        },
        "oAria": {
            "sSortAscending": ": Ordenar colunas de forma ascendente",
            "sSortDescending": ": Ordenar colunas de forma descendente"
        },
        "select": {
            "rows": {
                "_": "Selecionado %d linhas",
                "0": "Nenhuma linha selecionada",
                "1": "Selecionado 1 linha"
            }
        }
    });

    $('#example2').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
    });

    $('#datatable').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
    });
});

