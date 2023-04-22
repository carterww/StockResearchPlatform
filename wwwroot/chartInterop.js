let chartInstances = {};

function createPieChart(elementId, config) {
    // If a chart with the same elementId already exists, destroy it.
    if (chartInstances[elementId]) {
        chartInstances[elementId].destroy();
    }

    const canvas = document.getElementById(elementId);
    const ctx = canvas.getContext('2d');

    // Set the canvas size based on its current dimensions.
    canvas.width = canvas.clientWidth;
    canvas.height = canvas.clientHeight;

    const chart = new Chart(ctx, config);

    // Store the new chart instance by its elementId.
    chartInstances[elementId] = chart;
}
