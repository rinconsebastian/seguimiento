
$(document).ready(function () {

    $('.control_grupo').click(function () {
        //alert("ok");
        if ($(this).attr("data-displayed") === "hide") {

            //alert($(this).attr("data-grupo"));

            $('.control_' + $(this).attr("data-indicador")).attr("data-displayed", "hide");
            $(".indicador_" + $(this).attr("data-indicador")).addClass("periodo");
            $(".grupo_" + $(this).attr("data-indicador") + "_" + $(this).attr("data-grupo")).removeClass("periodo");


            $('.boton_indicador_' + $(this).attr("data-indicador")).css('color', 'red');
            $('.boton_grupo_' + $(this).attr("data-indicador") + '_' + $(this).attr("data-grupo")).css('color', 'grey');

            $(this).attr("data-displayed", "visible");



        }
        else {
            $('.boton_grupo_' + $(this).attr("data-indicador") + '_' + $(this).attr("data-grupo")).css('color', 'red');
            //$(".grupo_" + $(this).attr("data-indicador") + "_" + $(this).attr("data-grupo")).hide("slow");
            $(".grupo_" + $(this).attr("data-indicador") + "_" + $(this).attr("data-grupo")).addClass("periodo");
            $(this).attr("data-displayed", "hide");

        }

    });

});