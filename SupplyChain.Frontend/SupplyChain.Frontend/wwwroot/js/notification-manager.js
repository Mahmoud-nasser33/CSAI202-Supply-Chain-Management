// Notification System - Toast notifications and notification center
class NotificationManager {
    constructor() {
        this.notifications = [];
        this.init();
    }

    init() {
        // Load notifications from localStorage
        const saved = localStorage.getItem('silsila_notifications');
        if (saved) {
            this.notifications = JSON.parse(saved);
        }

        // Create toast container if it doesn't exist
        if (!document.getElementById('toastContainer')) {
            const container = document.createElement('div');
            container.id = 'toastContainer';
            container.className = 'position-fixed top-0 end-0 p-3';
            container.style.zIndex = '9999';
            document.body.appendChild(container);
        }

        // Update notification count
        this.updateNotificationCount();
    }

    // Show toast notification
    showToast(message, type = 'info', duration = 5000) {
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

        const toastHTML = `
            <div class="toast align-items-center text-bg-${color} border-0" role="alert" id="${toastId}">
                <div class="d-flex">
                    <div class="toast-body d-flex align-items-center gap-2">
                        <i class="bi bi-${icon}"></i>
                        <span>${message}</span>
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        `;

        const container = document.getElementById('toastContainer');
        container.insertAdjacentHTML('beforeend', toastHTML);

        const toastElement = document.getElementById(toastId);
        const toast = new bootstrap.Toast(toastElement, { delay: duration });
        toast.show();

        // Remove from DOM after hidden
        toastElement.addEventListener('hidden.bs.toast', () => {
            toastElement.remove();
        });
    }

    // Add notification to center
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

        // Also show as toast
        this.showToast(notification.title, notification.type);

        return newNotification;
    }

    // Mark notification as read
    markAsRead(notificationId) {
        const notification = this.notifications.find(n => n.id === notificationId);
        if (notification) {
            notification.read = true;
            this.saveNotifications();
            this.updateNotificationCount();
        }
    }

    // Mark all as read
    markAllAsRead() {
        this.notifications.forEach(n => n.read = true);
        this.saveNotifications();
        this.updateNotificationCount();
    }

    // Get unread count
    getUnreadCount() {
        return this.notifications.filter(n => !n.read).length;
    }

    // Update notification count badge
    updateNotificationCount() {
        const badge = document.getElementById('notificationCount');
        if (badge) {
            const count = this.getUnreadCount();
            badge.textContent = count;
            badge.style.display = count > 0 ? 'block' : 'none';
        }
    }

    // Save to localStorage
    saveNotifications() {
        localStorage.setItem('silsila_notifications', JSON.stringify(this.notifications));
    }

    // Get all notifications
    getNotifications() {
        return this.notifications;
    }

    // Clear all notifications
    clearAll() {
        this.notifications = [];
        this.saveNotifications();
        this.updateNotificationCount();
    }

    // Demo: Generate random notification
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

// Initialize notification manager
const notificationManager = new NotificationManager();

// Setup demo notification generator (every 30 seconds)
if (sessionManager.isLoggedIn()) {
    setInterval(() => {
        if (Math.random() > 0.7) { // 30% chance every 30 seconds
            notificationManager.generateDemoNotification();
        }
    }, 30000);
}

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = NotificationManager;
}
