function BarrasPeriodos(numero, data, categoria) {
    console.log("ino");
    console.log(data);
    console.log("fin");
    $("#BarrasPeriodos-" + numero).dxChart({



        dataSource: data,
        valueAxis: { // or valueAxis-argumentAxis
            min: 0,
            max: 100,
            valueMarginsEnabled: false
        },
        tooltip: {
            enabled: true
        },
        loadingIndicator: {
            show: true
        },
        valueAxis: {

            valueType: 'Number',
            title: { text: "Avance" },
            label: {
                precision: 2,
                customizeText: function () {
                    return this.value + ' %';
                }
            },
            grid: { visible: true }


        },
        argumentAxis: {

            valueMarginsEnabled: true,
            discreteAxisDivisionMode: "crossLabels",
            grid: {
                visible: true
            },
            label: {
                overlappingBehavior: "rotate"
            }
        },
        "export": {
            enabled: true
        },


        tooltip: {
            enabled: true,
           
            shared: false,
            format: {
                precision: 2
            },
            customizeTooltip: function (arg) {
                return {
                    text: arg.valueText + ' %'
                };
            }
        },

        series: {
            argumentField: categoria,
            valueField: "Cumplimiento",
            tagField: 'color',
            name: categoria,
            type: "bar",
            color: '#dddddd',
            label: {
                visible: true,
            }
        },
        customizeLabel: function () {
            if (this.value < 1) {
                return {
                    visible: true,
                    backgroundColor: "#dddddd",
                    customizeText: function () {
                        return this.valueText;
                    }
                };
            }
        },
        customizePoint: function () {
            console.log(this);
            return { color: this.tag, hoverStyle: { color: this.tag } };

        },





    });






    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var chart = $("#BarrasPeriodos-" + numero).dxChart('instance');
        chart.render();
    });
}
