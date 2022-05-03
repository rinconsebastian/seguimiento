$(document).ready(function () {
    $('.container-listado-responsable').on('click', '.ListadoCategoria', function () {


        var element = $(this);
        //color
        $('.ListadoCategoria').removeClass("highlight");
        element.addClass("highlight");
        //fin color
        var idPadre = $(this).data("id");
        var nivel = $(this).data("nivel");

        /*
        //alert(element.data("childs"));
        if (element.data("childs") === "hide") {
            $(this).data("childs", "show");
            // element.attr("data-childs", "show");
            $('div[data-padre=' + idPadre + ']').slideDown(100);
        }
        else {
            //element.attr("data-childs", "hide");
            $(this).data("childs", "hide");
            for (var x = 1; x < 10; x++) {

                $('div[data-nivel=' + (parseInt(nivel) + x) + ']').next().find("input").attr("data-childs", "hide");
                $('div[data-nivel=' + (parseInt(nivel) + x) + ']').prevAll().find("input").attr("data-childs", "hide");
                $('div[data-nivel=' + (parseInt(nivel) + x) + ']').slideUp(100);
            }

        }*/


        $('#resumen').empty();
        $('#detalle').empty();

        $('#resumen').append("<div class='centrar'><img  class='logoAjax' src='../images/ajax-loader.gif'/></div>");
        $('#detalle').append("<div class='centrar'><img  class='logoAjax' src='../images/ajax-loader.gif'/></div>");


        //carga resumen de la categoria
        //alert("hey");
        $('#resumen').slideUp(0);
        $('#resumen').empty();
        
          //$.get("../Categorias/resumen", { 'ID': idPadre })
         //   .done(function (datos) {
         //       $('#resumen').slideUp(0);
         //       $('#resumen').empty();
         //       $('#resumen').append(datos);
         //       $('#resumen').slideDown("slow");
         //   }).fail(function () {
         //       alert("Imposible recuperar reumen de ejecución intente nuevamente");
         //   });

        //carga indicadores relacionados
        $.get("../Indicadors/ListadoResponsable", { 'itemId': idPadre })
            .done(function (datos) {
                $('#detalle').slideUp(0);
                $('#detalle').empty();
                $('#detalle').append(datos);
                $('#detalle').slideDown("slow");

            });





    });




});