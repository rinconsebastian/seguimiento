//*********************************** VARIABLES ******************************************
var currenthost;
var chart;
var idIndicador = 0;
var nombreIndicador = "";
//*********************************** FUNCTIONS ******************************************

var funcChart = {
    loadShowModal: function () {
        $('.button-open-indicador-chart').click(function () {
            idIndicador = $(this).attr("data-id");
            nombreIndicador = $(this).attr("data-title");
            funcChart.updateDataset();
            chart.option("title.text", nombreIndicador);
            $('#myModalChart').modal('show');
        });
    },
    updateDataset: function () {
        $('#modalChartContent').html('<div class="loading"><img src="' + currenthost + 'images/loadingcircle.gif" ></div>');
        var tipo = $('#inputChart').val();
        $.get(currenthost +"Indicadors/ChartPop", { 'id': idIndicador, 'tipo': tipo })
            .done(function (data) {
                chart.option("dataSource", data);
                $('#modalChartContent').html('');
            })
            .fail(function () {
                $('#modalChartContent').html('<h3 class="text-danger text-center">Error al cargar los datos.</h3>');
            });
    },
    loadChart: function() {
        chart = $("#chart").dxChart({
            palette: "Carmine",
            //dataSource: dataSource,
            commonSeriesSettings: {
                argumentField: "periodo",
                type: "stackedline"
            },
            size: {
                height: 300,
                width: 750
            },
            margin: {
                bottom: 20
            },
            argumentAxis: {
                valueMarginsEnabled: false,
                discreteAxisDivisionMode: "crossLabels",
                grid: {
                    visible: true
                }
            },
            series: [
                { valueField: "planeado", name: "Planeado" },
                { valueField: "ejecutado", name: "Ejecutado" },
            ],
            legend: {
                verticalAlignment: "bottom",
                horizontalAlignment: "center",
                itemTextPosition: "bottom"
            },
            title: {
                text: "Gráfica",
                subtitle: {
                    text: "(Periodos)"
                }
            },
            "export": {
                enabled: true
            },
            tooltip: {
                enabled: true,
                zIndex : 3000
            },
            valueAxis: {
                visualRange: [0, 100],
                visualRangeUpdateMode: "keep"
            }
        }).dxChart("instance");
    },
    loadUpdateChart: function () {
        $('#inputChart').on('change', function () {
            var text = $("#inputChart option:selected").text();
            chart.option("title.subtitle", "("+text+")");
            funcChart.updateDataset();
        });
    },
    init: function () {

        currenthost = location.protocol + '//' + location.host + '/';

        funcChart.loadChart();

        funcChart.loadShowModal();
        funcChart.loadUpdateChart();
    }
    
};

//************************************** ON READY **********************************************
$(function () {
    funcChart.init();
});
