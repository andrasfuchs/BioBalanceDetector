"use strict";

var interval;

var options = {
    xAxes: [{
        display: false
    }]
};

var data = {
    labels: ["January", "February", "March", "April", "May", "June", "July"],
    datasets: [
        {
            label: "My First dataset",

            // Boolean - if true fill the area under the line
            fill: false,

            // Tension - bezier curve tension of the line. Set to 0 to draw straight lines connecting points
            // Used to be called "tension" but was renamed for consistency. The old option name continues to work for compatibility.
            lineTension: 0.1,

            // String - the color to fill the area under the line with if fill is true
            backgroundColor: "rgba(75,192,192,0.4)",

            // String - Line color
            borderColor: "rgba(75,192,192,1)",

            // String - cap style of the line. See https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/lineCap
            borderCapStyle: 'butt',

            // Array - Length and spacing of dashes. See https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/setLineDash
            borderDash: [],

            // Number - Offset for line dashes. See https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/lineDashOffset
            borderDashOffset: 0.0,

            // String - line join style. See https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/lineJoin
            borderJoinStyle: 'miter',

            // The properties below allow an array to be specified to change the value of the item at the given index

            // String or Array - Point stroke color
            pointBorderColor: "rgba(75,192,192,1)",

            // String or Array - Point fill color
            pointBackgroundColor: "#fff",

            // Number or Array - Stroke width of point border
            pointBorderWidth: 1,

            // Number or Array - Radius of point when hovered
            pointHoverRadius: 5,

            // String or Array - point background color when hovered
            pointHoverBackgroundColor: "rgba(75,192,192,1)",

            // String or Array - Point border color when hovered
            pointHoverBorderColor: "rgba(220,220,220,1)",

            // Number or Array - border width of point when hovered
            pointHoverBorderWidth: 2,

            // Number or Array - the pixel size of the point shape. Can be set to 0 to not render a circle over the point
            // Used to be called "radius" but was renamed for consistency. The old option name continues to work for compatibility.
            pointRadius: 1,

            // Number or Array - the pixel size of the non-displayed point that reacts to mouse hover events
            //
            // Used to be called "hitRadius" but was renamed for consistency. The old option name continues to work for compatibility.
            pointHitRadius: 10,

            // The actual data
            data: [65, 59, 80, 81, 56, 55, 40],

            // String - If specified, binds the dataset to a certain y-axis. If not specified, the first y-axis is used. First id is y-axis-0
            yAxisID: "y-axis-0",
        },
        {
            label: "My Second dataset",
            fill: false,
            backgroundColor: "rgba(255,205,86,0.4)",
            borderColor: "rgba(255,205,86,1)",
            pointBorderColor: "rgba(255,205,86,1)",
            pointBackgroundColor: "#fff",
            pointBorderWidth: 1,
            pointHoverRadius: 5,
            pointHoverBackgroundColor: "rgba(255,205,86,1)",
            pointHoverBorderColor: "rgba(255,205,86,1)",
            pointHoverBorderWidth: 2,
            data: [28, 48, 40, 19, 86, 27, 90]
        }
    ]
};

$(document).ready(function () {    

    // Any of the following formats may be used
    //var ctx = document.getElementById("myChart");
    //var ctx = document.getElementById("myChart").getContext("2d");
    var ctx = $("#myChart");

    var myLineChart = new Chart(ctx, {
        type: 'line',
        data: data,
        options: options
    });

    //$("canvas#myChart").click(function () {
    interval = setInterval(function() {
        $.ajax({
            url: urlListSessionIds,
            type: "GET",
            dataType: 'html',
            data: {},
            success: function (sessionIds) {
                sessionIds = JSON.parse(sessionIds);

                $.ajax({
                    url: urlChannelGetValues.replace("{sessionId}", sessionIds[0]),
                    type: "GET",
                    dataType: 'html',
                    data: {},
                    success: function (data) {
                        var values = JSON.parse(data);

                        myLineChart.data.labels.length = 0;
                        myLineChart.data.datasets[0].data.length = 0;

                        for (var i = 0; i < values.length; i++)
                        {
                            myLineChart.data.labels.push(values[i].Label);
                            myLineChart.data.datasets[0].data.push(values[i].Value);
                        }
                        myLineChart.update();
                    }
                });
            }
        });
    }, 40);
});