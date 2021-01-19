
	// TODO: add your code here
	
	function gaugaje(rango1,rango2,rango3,numero,data){
        
        var gauge = $("#gauge-"+numero).dxCircularGauge({
        scale: {
            startValue: 0,
            endValue: 100,
            tickInterval: 10,
            label: {
                customizeText: function (arg) {
                    return arg.valueText + " %";
                },
                useRangeColors: false,
                indentFromTick: 50
            }
        },
        rangeContainer: {
            ranges: [
                { startValue: 0, endValue: rango1, color: "#C22E22" },
                { startValue: rango1, endValue: rango2, color: "#F68C36" },
                { startValue: rango2, endValue: rango3, color: "#F3C959" },
                { startValue: rango3, endValue: 100, color: "#298A7F" }
            ], width: 50
        },
        tooltip: { enabled: true },
        //title: {
        //   text: "Temperature in the Greenhouse",
        //    font: { size: 28 }
        //},
        value: data[0].mean,
        valueIndicator: {
            type: "rectangleNeedle",
            color: "#888888",
            width: 3,
            offset:5
        },
        subvalues: [data[0].min, data[0].max]
    }).dxCircularGauge("instance");

    
    
}
