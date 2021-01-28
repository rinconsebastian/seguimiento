

//BLOQUE DE LOS CAMPOS CATEGORIA E INDICADOR EN LA CREACION Y EDICION DE EVALUACIONES

$(document).ready(function () {
    if ($('#Contexto').length > 0) {
        lockContexto();
        $('#Contexto').change(function () {
            lockContexto();
        });
        
    }
});

function lockContexto() {
    var contexto = $('#Contexto').val();
    if (contexto === "Global") {
        lockIndicador();
        lockCategoria();
    } else if (contexto === "Categoria") {
        lockIndicador();
        $('#Categoria').prop('disabled', false);
        $('#IdCategoria').prop('disabled', false);
    } else if (contexto === "Indicador") {
        lockCategoria();
        $('#Indicador').prop('disabled', false);
        $('#IdIndicador').prop('disabled', false);
    }
}

function lockIndicador(){
    $('#IdIndicador').prop('disabled', true);
    $('#IdIndicador').val(0);
    $('#Indicador').prop('disabled', true);
    $('#Indicador').val('');
}

function lockCategoria() {
    $('#IdCategoria').prop('disabled', true);
    $('#IdCategoria').val(0);
    $('#Categoria').prop('disabled', true);
    $('#Categoria').val('');
}