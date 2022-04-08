$(document).ready(function () {


    $('.classCambioPeriodo').change(function () {

        var val = $(this).val();
        var old = window.location.href;
        var ruta = old.split('?')[0];
        var argumentos = old.split('?')[1];
        var id = null;

        if (argumentos !== undefined) {
            if (argumentos.includes('&', 0)) {

                id = (argumentos.split('&')[0]).split('=')[1];

            } else {
                if (argumentos.includes('id', 0)) {
                    id = argumentos.split('=')[1];
                }
            }
        }
        if (id !== null) {
            window.location.href = ruta + "?id=" + id + "&tperiodo=" + val;
        } else {
            window.location.href = ruta + "?tperiodo=" + val;
        }



    });

});