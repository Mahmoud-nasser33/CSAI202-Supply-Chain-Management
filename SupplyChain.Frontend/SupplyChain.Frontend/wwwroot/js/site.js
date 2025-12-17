// Live Interaction Logic

document.addEventListener('DOMContentLoaded', function () {
    
    // 1. Live Revenue Chart (Chart.js)
    const ctx = document.getElementById('revenueChart');
    if (ctx) {
        const revenueChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: ['10:00', '10:05', '10:10', '10:15', '10:20', '10:25'],
                datasets: [{
                    label: 'Revenue',
                    data: [12000, 12150, 12100, 12250, 12300, 12400],
                    borderColor: '#10b981', // Primary Green
                    backgroundColor: 'rgba(16, 185, 129, 0.1)',
                    borderWidth: 2,
                    tension: 0.4, // Smooths the curve
                    fill: true,
                    pointRadius: 0, // Hides points for clean look
                    pointHoverRadius: 4
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        mode: 'index',
                        intersect: false,
                    }
                },
                scales: {
                    x: { display: false }, // Hide X axis
                    y: { display: false, min: 11000 }  // Hide Y axis
                },
                animation: {
                    duration: 1000,
                    easing: 'linear'
                }
            }
        });

        // Simulate Live Data Updates
        setInterval(() => {
            // Remove oldest
            revenueChart.data.labels.shift();
            revenueChart.data.datasets[0].data.shift();

            // Add new random data
            const now = new Date();
            const timeLabel = now.getHours() + ':' + String(now.getMinutes()).padStart(2, '0');
            const lastValue = revenueChart.data.datasets[0].data[revenueChart.data.datasets[0].data.length - 1];
            const volatility = Math.floor(Math.random() * 200) - 80; // Random +/- change
            const newValue = Math.max(11000, lastValue + volatility);

            revenueChart.data.labels.push(timeLabel);
            revenueChart.data.datasets[0].data.push(newValue);
            revenueChart.update();

            // Update Text Display
            const displayVal = (newValue / 10000).toFixed(2) + 'M';
            const displayEl = document.getElementById('liveRevenueDisplay');
            if(displayEl) displayEl.innerText = '$' + displayVal;

        }, 3000); // Update every 3 seconds
    }

    // 2. Random Number Simulator for Other Stats
    const simulateUpdates = () => {
        // Find badges or numbers to "flicker" update
        // We'll just randomly trigger a toast for now to simulate "Activity"
    };

    // 3. Toast Notification System
    const toasts = [
        { icon: 'bi-cart-check', msg: 'New Order #1024 received', color: 'text-success' },
        { icon: 'bi-truck', msg: 'Shipment #773 arrived at Hub A', color: 'text-primary' },
        { icon: 'bi-box-seam', msg: 'Low stock alert: Industrial Steel', color: 'text-warning' },
        { icon: 'bi-person-plus', msg: 'New user "Sarah" joined', color: 'text-info' },
        { icon: 'bi-cash-coin', msg: 'Payment received: $4,500', color: 'text-success' }
    ];

    const showRandomToast = () => {
        const toast = toasts[Math.floor(Math.random() * toasts.length)];
        
        // Create Toast Element
        const toastEl = document.createElement('div');
        toastEl.className = 'position-fixed bottom-0 end-0 p-3';
        toastEl.style.zIndex = '1100';
        toastEl.innerHTML = `
            <div class="toast show align-items-center border-0 shadow-lg" role="alert" aria-live="assertive" aria-atomic="true" style="background: rgba(255,255,255,0.95); backdrop-filter: blur(10px);">
                <div class="d-flex">
                    <div class="toast-body d-flex align-items-center">
                        <i class="bi ${toast.icon} fs-4 me-3 ${toast.color}"></i>
                        <div>
                            <strong class="d-block text-dark">System Update</strong>
                            <small class="text-muted">${toast.msg}</small>
                        </div>
                    </div>
                    <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast" aria-label="Close" onclick="this.closest('.toast').remove()"></button>
                </div>
            </div>
        `;

        document.body.appendChild(toastEl);

        // Auto remove
        setTimeout(() => {
            toastEl.remove();
        }, 5000);
    };

    // Trigger toast occasionally
    setTimeout(showRandomToast, 5000); // First one after 5s
    setInterval(() => {
        if(Math.random() > 0.6) showRandomToast(); // Random chance every 10s
    }, 10000);

});
