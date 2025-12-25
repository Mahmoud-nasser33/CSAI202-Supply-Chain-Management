
class NotificationManager {
    constructor() {
        this.notifications = [];
        this.init();
    }

    init() {

        const storageKey = (typeof AppConfig !== 'undefined' && AppConfig.notifications) ? AppConfig.notifications.storageKey : 'silsila_notifications';
        const saved = localStorage.getItem(storageKey);
        if (saved) {
            this.notifications = JSON.parse(saved);
        }

        if (!document.getElementById('toastContainer')) {
            const container = document.createElement('div');
            container.id = 'toastContainer';
            container.className = 'position-fixed top-0 end-0 p-3';
            container.style.zIndex = '9999';
            document.body.appendChild(container);
        }

        this.updateNotificationCount();
    }

    showToast(message, type = 'info', duration = null) {
        if (duration === null) {
            duration = (typeof AppConfig !== 'undefined' && AppConfig.ui) ? AppConfig.ui.toastDuration : 5000;
        }
        const toastId = 'toast_' + Date.now();
        const iconMap = {
            'success': 'check-circle-fill',
            'error': 'x-circle-fill',
            'warning': 'exclamation-triangle-fill',
            'info': 'info-circle-fill'
        };

        const colorMap = {
            'success': 'success',
            'error': 'danger',
            'warning': 'warning',
            'info': 'primary'
        };

        const icon = iconMap[type] || iconMap['info'];
        const color = colorMap[type] || colorMap['info'];

        const toastEl = document.createElement('div');
        toastEl.className = `toast align-items-center text-bg-${color} border-0`;
        toastEl.setAttribute('role', 'alert');
        toastEl.setAttribute('aria-live', 'assertive');
        toastEl.setAttribute('aria-atomic', 'true');
        toastEl.id = toastId;

        const toastInner = document.createElement('div');
        toastInner.className = 'd-flex';

        const toastBody = document.createElement('div');
        toastBody.className = 'toast-body d-flex align-items-center gap-2';

        const iconEl = document.createElement('i');
        iconEl.className = `bi bi-${icon}`;
        iconEl.setAttribute('aria-hidden', 'true');

        const messageEl = document.createElement('span');
        messageEl.textContent = message;

        const closeBtn = document.createElement('button');
        closeBtn.type = 'button';
        closeBtn.className = 'btn-close btn-close-white me-2 m-auto';
        closeBtn.setAttribute('data-bs-dismiss', 'toast');
        closeBtn.setAttribute('aria-label', 'Close');

        toastBody.appendChild(iconEl);
        toastBody.appendChild(messageEl);
        toastInner.appendChild(toastBody);
        toastInner.appendChild(closeBtn);
        toastEl.appendChild(toastInner);

        const container = document.getElementById('toastContainer');
        container.appendChild(toastEl);

        const toastElement = document.getElementById(toastId);
        const toast = new bootstrap.Toast(toastElement, { delay: duration });
        toast.show();

        toastElement.addEventListener('hidden.bs.toast', () => {
            toastElement.remove();
        });
    }

    addNotification(notification) {
        const newNotification = {
            id: Date.now(),
            title: notification.title,
            message: notification.message,
            type: notification.type || 'info',
            timestamp: new Date().toISOString(),
            read: false,
            ...notification
        };

        this.notifications.unshift(newNotification);
        this.saveNotifications();
        this.updateNotificationCount();

        this.showToast(notification.title, notification.type);

        return newNotification;
    }

    markAsRead(notificationId) {
        const notification = this.notifications.find(n => n.id === notificationId);
        if (notification) {
            notification.read = true;
            this.saveNotifications();
            this.updateNotificationCount();
        }
    }

    markAllAsRead() {
        this.notifications.forEach(n => n.read = true);
        this.saveNotifications();
        this.updateNotificationCount();
    }

    getUnreadCount() {
        return this.notifications.filter(n => !n.read).length;
    }

    updateNotificationCount() {
        const badge = document.getElementById('notificationCount');
        if (badge) {
            const count = this.getUnreadCount();
            badge.textContent = count;
            badge.style.display = count > 0 ? 'block' : 'none';
        }
    }

    saveNotifications() {
        const storageKey = (typeof AppConfig !== 'undefined' && AppConfig.notifications) ? AppConfig.notifications.storageKey : 'silsila_notifications';
        localStorage.setItem(storageKey, JSON.stringify(this.notifications));
    }

    getNotifications() {
        return this.notifications;
    }

    clearAll() {
        this.notifications = [];
        this.saveNotifications();
        this.updateNotificationCount();
    }

    generateDemoNotification() {
        const demoNotifications = [
            { title: 'New Order Received', message: 'Order #' + Math.floor(Math.random() * 10000) + ' from Customer', type: 'success' },
            { title: 'Low Stock Alert', message: 'Product running low on stock', type: 'warning' },
            { title: 'Shipment Delivered', message: 'Order delivered successfully', type: 'success' },
            { title: 'Payment Received', message: 'Payment of $' + Math.floor(Math.random() * 1000) + ' received', type: 'info' },
            { title: 'System Update', message: 'New features available', type: 'info' }
        ];

        const randomNotification = demoNotifications[Math.floor(Math.random() * demoNotifications.length)];
        this.addNotification(randomNotification);
    }
}

const notificationManager = new NotificationManager();

if (sessionManager.isLoggedIn() && typeof AppConfig !== 'undefined' && AppConfig.demo && AppConfig.demo.enabled) {
    const interval = AppConfig.notifications ? AppConfig.notifications.demoInterval : 30000;
    const probability = AppConfig.notifications ? (1 - AppConfig.notifications.demoProbability) : 0.3;
    setInterval(() => {
        if (Math.random() > probability) {
            notificationManager.generateDemoNotification();
        }
    }, interval);
}

if (typeof module !== 'undefined' && module.exports) {
    module.exports = NotificationManager;
}
