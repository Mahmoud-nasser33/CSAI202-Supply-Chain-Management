

document.addEventListener('DOMContentLoaded', function () {

    const ctx = document.getElementById('revenueChart');
    if (ctx) {
        const revenueChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: ['10:00', '10:05', '10:10', '10:15', '10:20', '10:25'],
                datasets: [{
                    label: 'Revenue',
                    data: [12000, 12150, 12100, 12250, 12300, 12400],
                    borderColor: '#10b981',
                    backgroundColor: 'rgba(16, 185, 129, 0.1)',
                    borderWidth: 2,
                    tension: 0.4,
                    fill: true,
                    pointRadius: 0,
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
                    x: { display: false },
                    y: { display: false, min: 11000 }
                },
                animation: {
                    duration: 1000,
                    easing: 'linear'
                }
            }
        });

        setInterval(() => {

            revenueChart.data.labels.shift();
            revenueChart.data.datasets[0].data.shift();

            const now = new Date();
            const timeLabel = now.getHours() + ':' + String(now.getMinutes()).padStart(2, '0');
            const lastValue = revenueChart.data.datasets[0].data[revenueChart.data.datasets[0].data.length - 1];
            const volatility = Math.floor(Math.random() * 200) - 80;
            const newValue = Math.max(11000, lastValue + volatility);

            revenueChart.data.labels.push(timeLabel);
            revenueChart.data.datasets[0].data.push(newValue);
            revenueChart.update();

            const displayVal = (newValue / 10000).toFixed(2) + 'M';
            const displayEl = document.getElementById('liveRevenueDisplay');
            if(displayEl) displayEl.innerText = '$' + displayVal;

        }, (typeof AppConfig !== 'undefined' && AppConfig.ui) ? AppConfig.ui.chartUpdateInterval : 3000);
    }

    const simulateUpdates = () => {

    };

    const toasts = [
        { icon: 'bi-cart-check', msg: 'New Order #1024 received', color: 'text-success' },
        { icon: 'bi-truck', msg: 'Shipment #773 arrived at Hub A', color: 'text-primary' },
        { icon: 'bi-box-seam', msg: 'Low stock alert: Industrial Steel', color: 'text-warning' },
        { icon: 'bi-person-plus', msg: 'New user "Sarah" joined', color: 'text-info' },
        { icon: 'bi-cash-coin', msg: 'Payment received: $4,500', color: 'text-success' }
    ];

    const showRandomToast = () => {
        const toast = toasts[Math.floor(Math.random() * toasts.length)];

        const toastContainer = document.createElement('div');
        toastContainer.className = 'position-fixed bottom-0 end-0 p-3';
        toastContainer.style.zIndex = '1100';

        const toastEl = document.createElement('div');
        toastEl.className = 'toast show align-items-center border-0 shadow-lg';
        toastEl.setAttribute('role', 'alert');
        toastEl.setAttribute('aria-live', 'assertive');
        toastEl.setAttribute('aria-atomic', 'true');
        toastEl.style.background = 'rgba(255,255,255,0.95)';
        toastEl.style.backdropFilter = 'blur(10px)';

        const toastInner = document.createElement('div');
        toastInner.className = 'd-flex';

        const toastBody = document.createElement('div');
        toastBody.className = 'toast-body d-flex align-items-center';

        const iconEl = document.createElement('i');
        iconEl.className = `bi ${toast.icon} fs-4 me-3 ${toast.color}`;
        iconEl.setAttribute('aria-hidden', 'true');

        const contentDiv = document.createElement('div');
        const titleEl = document.createElement('strong');
        titleEl.className = 'd-block text-dark';
        titleEl.textContent = 'System Update';
        const msgEl = document.createElement('small');
        msgEl.className = 'text-muted';
        msgEl.textContent = toast.msg;

        const closeBtn = document.createElement('button');
        closeBtn.type = 'button';
        closeBtn.className = 'btn-close me-2 m-auto';
        closeBtn.setAttribute('data-bs-dismiss', 'toast');
        closeBtn.setAttribute('aria-label', 'Close');
        closeBtn.addEventListener('click', () => {
            toastEl.remove();
        });

        contentDiv.appendChild(titleEl);
        contentDiv.appendChild(msgEl);
        toastBody.appendChild(iconEl);
        toastBody.appendChild(contentDiv);
        toastInner.appendChild(toastBody);
        toastInner.appendChild(closeBtn);
        toastEl.appendChild(toastInner);
        toastContainer.appendChild(toastEl);
        document.body.appendChild(toastContainer);

        setTimeout(() => {
            toastContainer.remove();
        }, 5000);
    };

    const demoConfig = (typeof AppConfig !== 'undefined' && AppConfig.demo) ? AppConfig.demo : { toastDelay: 5000, toastInterval: 10000, toastProbability: 0.6 };
    setTimeout(showRandomToast, demoConfig.toastDelay);
    setInterval(() => {
        if(Math.random() > (1 - demoConfig.toastProbability)) showRandomToast();
    }, demoConfig.toastInterval);

});
