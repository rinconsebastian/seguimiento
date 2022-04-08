function BarrasHijos(numero, data, categoria) {

    $("#barrasHijos-" + numero).dxChart({
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
                overlappingBehavior: "rotate",
                rotationAngle: 270

            }
        },
        "export": {
            enabled: true
        },


        tooltip: {
            enabled: true,
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
            tagField: 'tag',
            name: categoria,
            type: "bar",
            color: '#ffaa66',
            //   label: {
            //       visible: true,
            //   }
        },

        customizeLabel: function () {
            if (this.value < 1) {
                return {
                    visible: true,
                    backgroundColor: "#ffaa66",
                    customizeText: function () {
                        return this.valueText;
                    }
                };
            }
        },
        onPointClick: function (info) {
            //alert(info.target.tag);
            var clickedPoint = info.target;
            window.location.href = clickedPoint.tag;

        },
        onArgumentAxisClick: function (info) {
            var series = info.component.getSeriesByName(categoria);
            var point = series.getPointsByArg(info.argument);
            var myTag = point[0].tag;
            // alert(myTag);

            window.location.href = myTag;
        }
    });
}
