function loadInicial(idPadre) {
    $('#resumen').append("<div class='centrar'><img  class='logoAjax' src='../images/ajax-loader.gif'/></div>");
    $('#detalle').append("<div class='centrar'><img  class='logoAjax' src='../images/ajax-loader.gif'/></div>");
    $.get("../Categorias/resumen", { 'ID': idPadre })
        .done(function (datos) {
            $('#resumen').slideUp(0);
            $('#resumen').empty();
            $('#resumen').append(datos);
            $('#resumen').slideDown("slow");
        }).fail(function () {
            alert("Imposible recuperar resumen de ejecución intente nuevamente");
        });

    //carga indicadores relacionados
    $.get("../Indicadors/Listado", { 'itemId': idPadre })
        .done(function (datos) {
            $('#detalle').slideUp(0);
            $('#detalle').empty();
            $('#detalle').append(datos);
            $('#detalle').slideDown("slow");

        });


}


