function createPieChart(canvasId, chartConfig) {
    var ctx = document.getElementById(canvasId).getContext('2d');
    new Chart(ctx, chartConfig);
}