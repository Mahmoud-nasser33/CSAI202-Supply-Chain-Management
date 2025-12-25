
class SessionManager {
    constructor() {
        this.SESSION_KEY = (typeof AppConfig !== 'undefined' && AppConfig.session) ? AppConfig.session.sessionKey : 'silsila_session';
        this.INACTIVITY_TIMEOUT = (typeof AppConfig !== 'undefined' && AppConfig.session) ? AppConfig.session.inactivityTimeout : 30 * 60 * 1000;
        this.inactivityTimer = null;
        this.init();
    }

    init() {

        const session = this.getSession();
        if (session) {
            this.startInactivityTimer();
            this.setupActivityListeners();
        }
    }

    login(userData) {
        const session = {
            user: {
                id: userData.id || this.generateId(),
                name: userData.name,
                email: userData.email,
                role: userData.role,
                avatar: userData.avatar || this.getAvatarInitials(userData.name)
            },
            loginTime: new Date().toISOString(),
            lastActivity: new Date().toISOString()
        };

        localStorage.setItem(this.SESSION_KEY, JSON.stringify(session));
        this.startInactivityTimer();
        this.setupActivityListeners();
        return session;
    }

    logout() {
        localStorage.removeItem(this.SESSION_KEY);
        this.clearInactivityTimer();
        window.location.href = '/Login';
    }

    getSession() {
        const sessionData = localStorage.getItem(this.SESSION_KEY);
        return sessionData ? JSON.parse(sessionData) : null;
    }

    getCurrentUser() {
        const session = this.getSession();
        return session ? session.user : null;
    }

    isLoggedIn() {
        return this.getSession() !== null;
    }

    hasRole(role) {
        const user = this.getCurrentUser();
        return user && user.role === role;
    }

    hasAnyRole(roles) {
        const user = this.getCurrentUser();
        return user && roles.includes(user.role);
    }

    updateActivity() {
        const session = this.getSession();
        if (session) {
            session.lastActivity = new Date().toISOString();
            localStorage.setItem(this.SESSION_KEY, JSON.stringify(session));
        }
    }

    startInactivityTimer() {
        this.clearInactivityTimer();
        this.inactivityTimer = setTimeout(() => {
            this.logout();
            alert('Session expired due to inactivity. Please login again.');
        }, this.INACTIVITY_TIMEOUT);
    }

    clearInactivityTimer() {
        if (this.inactivityTimer) {
            clearTimeout(this.inactivityTimer);
            this.inactivityTimer = null;
        }
    }

    setupActivityListeners() {
        const events = ['mousedown', 'keydown', 'scroll', 'touchstart'];
        events.forEach(event => {
            document.addEventListener(event, () => {
                this.updateActivity();
                this.startInactivityTimer();
            }, { passive: true });
        });
    }

    getDashboardUrl(role) {
        const dashboards = {
            'admin': '/Index',
            'user': '/UserDashboard',
            'customer': '/CustomerDashboard',
            'supplier': '/SupplierDashboard'
        };
        return dashboards[role] || '/Index';
    }

    getRolePermissions(role) {
        const permissions = {
            'admin': {
                canViewAll: true,
                canCreate: true,
                canEdit: true,
                canDelete: true,
                navigation: ['home', 'orders', 'inventory', 'shipments', 'products', 'customers', 'suppliers', 'warehouses', 'payments', 'analytics', 'feedback', 'users', 'profile', 'settings']
            },
            'user': {
                canViewAll: false,
                canCreate: true,
                canEdit: true,
                canDelete: false,
                navigation: ['home', 'orders', 'inventory', 'shipments', 'products', 'profile', 'settings']
            },
            'customer': {
                canViewAll: false,
                canCreate: true,
                canEdit: false,
                canDelete: false,
                navigation: ['home', 'orders', 'notifications', 'profile']
            },
            'supplier': {
                canViewAll: false,
                canCreate: false,
                canEdit: true,
                canDelete: false,
                navigation: ['home', 'orders', 'payments', 'profile']
            }
        };
        return permissions[role] || permissions['customer'];
    }

    generateId() {
        return 'user_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
    }

    getAvatarInitials(name) {
        return name
            .split(' ')
            .map(n => n[0])
            .join('')
            .toUpperCase()
            .substr(0, 2);
    }

    getRoleDisplayName(role) {
        const names = {
            'admin': 'Administrator',
            'user': 'Operations / Warehouse',
            'customer': 'Consumer / Client',
            'supplier': 'Supply Partner'
        };
        return names[role] || role;
    }

    getRoleBadgeColor(role) {
        const colors = {
            'admin': 'danger',
            'user': 'primary',
            'customer': 'success',
            'supplier': 'warning'
        };
        return colors[role] || 'secondary';
    }
}

const sessionManager = new SessionManager();

function requireAuth() {
    if (!sessionManager.isLoggedIn()) {
        window.location.href = '/Login';
        return false;
    }
    return true;
}

function redirectToDashboard() {
    const user = sessionManager.getCurrentUser();
    if (user) {
        window.location.href = sessionManager.getDashboardUrl(user.role);
    }
}

if (typeof module !== 'undefined' && module.exports) {
    module.exports = SessionManager;
}
