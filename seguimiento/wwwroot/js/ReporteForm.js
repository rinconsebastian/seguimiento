$(document).ready(function () {
    $('.form-reporte').change(function () {
        //$(this).blur();
        var element = $(this);
        $.post("../ejecucions/EditFormReporte", $(this).serialize())
            .done(function (data) {
                //alert(data);
                $('#notas').empty();
                if (data == true) {
                    //alert(element.attr('class'));
                    element.children("div").children("input").addClass("reporte-input-success");
                    element.children("div").children("input").removeClass("reporte-input-error");
                    element.children("div").children("input").removeClass("reporte-input-warning");
                    

                    var alertaPlaneado = !$.isNumeric(element.find("input[id*='planeado']").val());
                    var alertaEjecutado = !$.isNumeric(element.find("input[id*='ejecutado']").val());

                    if (alertaPlaneado == true) {
                        element.find("input[id*='planeado']").removeClass("reporte-input-error");
                        element.find("input[id*='planeado']").removeClass("reporte-input-success");
                        element.find("input[id*='planeado']").addClass("reporte-input-warning");
                    }
                    if (alertaEjecutado == true) {
                        element.find("input[id*='ejecutado']").removeClass("reporte-input-error");
                        element.find("input[id*='ejecutado']").removeClass("reporte-input-success");
                        element.find("input[id*='ejecutado']").addClass("reporte-input-warning");
                    }


                }
                else {
                    alert("erro");
                    loadEjecucion(idEjecucion, "error", "No se pudo actualizar la Ejecución");
                    element.children("div").children("input").removeClass("reporte-input-success");
                    element.children("div").children("input").removeClass("reporte-input-warning");
                    element.children("div").children("input").addClass("reporte-input-error");
                }

            })
                .fail(function () {
                    element.children("div").children("input").removeClass("reporte-input-success");
                    element.children("div").children("input").removeClass("reporte-input-warning");
                    element.children("div").children("input").addClass("reporte-input-error");

                });


    })

    $('input').on('paste', function (e) {
        $(this).next("input[data-tipo^='ejecutado-Jun-17']").val("clican");
        var pastedData = e.originalEvent.clipboardData.getData('text');
        
        var inputx = $(this);

        arr = pastedData.split('\n');
        var i = 0;

        while (i + 1 < arr.length && inputx != undefined) {
            inputx.val(arr[i]);
            inputx.change();
            inputx = (inputx.closest('.fila-categoria').next('.fila-categoria').find("input[data-tipo^='" + inputx.attr('data-tipo') + "']"));
            i = i + 1;
        }
        e.preventDefault();   
        })

    
});