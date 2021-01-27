//----------------------------- AJAXA POST PARA CARGAR ARCHIVOS -----------------------------
var currenthost;

var funcUpload = {
    //Carga archivos para indicadores
    //Puede usarse junto con la vista parcial _IndicadorUpload pasando como parametro el Model del indicador
    //Regresa el nombre del archivo cargado a un input #Adjunto
    loadFile: function () {
        $('body').on('change', '.fileUploadAdjunto', function () {
            var formulario = $(this).closest('form');
            var key = $(this).data('key');

            // Serialize your form
            var formData = new FormData(formulario[0]);
            var time = funcUpload.getFormatDate(key);
            formData.append("id", time);


            $('#link_'+key).empty();
            $('#info_' + key).html('Cargando <img src="/images/ajax-loader.gif">');

            //alert(formData.stringify());
            $.ajax({
                type: "POST",
                url: formulario.attr('action'),
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    //var respuesta = JSON.parse(response);
                    if (response.loaded == true) {
                        
                        $('#Adjunto_'+key).val(response.nombre);
                        $('#info_' + key).empty();
                        $('#link_' + key).attr("href", currenthost + "/UploadedFiles/" + response.nombre)
                        $('#link_' + key).html(response.nombre);
                    }
                    else {
                        $('#info_' + key).html("Imposible cargar el archivo.");
                    }
                },
                error: function (error) {
                    $('#info_' + key).html("Error al cargar el archivo.");
                }
            });

        });
    },
    getFormatDate: function (key) {
        var dt = new Date();
        var day = dt.getDate();
        var month = dt.getMonth() + 1;
        var hour = dt.getHours();
        var minute = dt.getMinutes();
        var seconds = dt.getSeconds();
        if (day < 10) { day = "0" + day; }
        if (month < 10) { month = "0" + month; }
        if (hour < 10) { hour = "0" + hour; }
        if (minute < 10) { minute = "0" + minute; }
        if (seconds < 10) { seconds = "0" + seconds; }
        return key + "-" + dt.getFullYear() + "_" + month + "_" + day + "-" + hour + "_" + minute + "_" + seconds;
    },
    showSelection: function () {
        $('body').on('change', 'input[type="file"].custom-file-input', function (e) {
            if (e.target.files.length > 0) {
                var fileName = e.target.files[0].name;
                $(this).next('.custom-file-label').html(fileName);
            }
        });
    },
  
    init: function () {
        console.log("Upload Script load");
        currenthost = location.protocol + '//' + location.host + '/';
        funcUpload.loadFile();

        funcUpload.showSelection();
    }
};


//************************************** ON READY **********************************************
$(function () {
    funcUpload.init();
});


