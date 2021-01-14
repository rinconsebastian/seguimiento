
$(document).ready(function () {

    $('.control_grupo_unido').click(function () {
        //alert("ok");
        if ($(this).attr("data-displayed") === "hide") {

            //alert($(this).attr("data-grupo"));

            $('.control_' + $(this).attr("data-grupo")).attr("data-displayed", "hide");
            $(".grupo_unido").addClass("periodo");
            $(".grupo_unido_" + $(this).attr("data-grupo")).removeClass("periodo");


            $('.boton_grupo_unido').css('color', 'red');
            $('.boton_grupo_' + $(this).attr("data-grupo")).css('color', 'grey');

            $(this).attr("data-displayed", "visible");



        }
        else {
            $('.boton_grupo_unido_' + $(this).attr("data-grupo")).css('color', 'red');
            //$(".grupo_" + $(this).attr("data-indicador") + "_" + $(this).attr("data-grupo")).hide("slow");
            $(".grupo_unido_" + $(this).attr("data-grupo")).addClass("periodo");
            $(this).attr("data-displayed", "hide");

        }

    });

});