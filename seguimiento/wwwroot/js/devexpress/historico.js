function historico(numero, data){
    $("#historico-"+numero).dxChart({
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
        series: {
            argumentField: "Periodo",
            valueField: "Ejecucion",
            name: "Periodos",
            type: "bar",
            color: '#ffaa66'
        }
    });
}