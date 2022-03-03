
$(document).ready(function () {

    var url_atual = window.location.href;

    if (url_atual.match(Fatura)) {
        GetOperadora();
    }
    

});



function GetOperadora() {

    $.ajax({
        url: '/Operadora/GetOperadora',
        success: function (data) {
            $(".Agencia").empty();

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