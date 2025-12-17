// Session Manager - Handles user authentication and role management
class SessionManager {
    constructor() {
        this.SESSION_KEY = 'silsila_session';
        this.INACTIVITY_TIMEOUT = 30 * 60 * 1000; // 30 minutes
        this.inactivityTimer = null;
        this.init();
    }

    init() {
        // Check for existing session
        const session = this.getSession();
        if (session) {
            this.startInactivityTimer();
            this.setupActivityListeners();
        }
    }

    // Login user with role
    login(userData) {
        const session = {
            user: {
                id: userData.id || this.generateId(),
                name: userData.name,
                email: userData.email,
                role: userData.role, // 'admin', 'user', 'customer', 'supplier'
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

    // Logout user
    logout() {
        localStorage.removeItem(this.SESSION_KEY);
        this.clearInactivityTimer();
        window.location.href = '/Login';
    }

    // Get current session
    getSession() {
        const sessionData = localStorage.getItem(this.SESSION_KEY);
        return sessionData ? JSON.parse(sessionData) : null;
    }

    // Get current user
    getCurrentUser() {
        const session = this.getSession();
        return session ? session.user : null;
    }

    // Check if user is logged in
    isLoggedIn() {
        return this.getSession() !== null;
    }

    // Check if user has specific role
    hasRole(role) {
        const user = this.getCurrentUser();
        return user && user.role === role;
    }

    // Check if user has any of the specified roles
    hasAnyRole(roles) {
        const user = this.getCurrentUser();
        return user && roles.includes(user.role);
    }

    // Update last activity time
    updateActivity() {
        const session = this.getSession();
        if (session) {
            session.lastActivity = new Date().toISOString();
            localStorage.setItem(this.SESSION_KEY, JSON.stringify(session));
        }
    }

    // Start inactivity timer
    startInactivityTimer() {
        this.clearInactivityTimer();
        this.inactivityTimer = setTimeout(() => {
            this.logout();
            alert('Session expired due to inactivity. Please login again.');
        }, this.INACTIVITY_TIMEOUT);
    }

    // Clear inactivity timer
    clearInactivityTimer() {
        if (this.inactivityTimer) {
            clearTimeout(this.inactivityTimer);
            this.inactivityTimer = null;
        }
    }

    // Setup activity listeners
    setupActivityListeners() {
        const events = ['mousedown', 'keydown', 'scroll', 'touchstart'];
        events.forEach(event => {
            document.addEventListener(event, () => {
                this.updateActivity();
                this.startInactivityTimer();
            }, { passive: true });
        });
    }

    // Get role-specific dashboard URL
    getDashboardUrl(role) {
        const dashboards = {
            'admin': '/Index',
            'user': '/UserDashboard',
            'customer': '/CustomerDashboard',
            'supplier': '/SupplierDashboard'
        };
        return dashboards[role] || '/Index';
    }

    // Get role permissions
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

    // Helper: Generate unique ID
    generateId() {
        return 'user_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
    }

    // Helper: Get avatar initials
    getAvatarInitials(name) {
        return name
            .split(' ')
            .map(n => n[0])
            .join('')
            .toUpperCase()
            .substr(0, 2);
    }

    // Helper: Get role display name
    getRoleDisplayName(role) {
        const names = {
            'admin': 'Administrator',
            'user': 'Operations / Warehouse',
            'customer': 'Consumer / Client',
            'supplier': 'Supply Partner'
        };
        return names[role] || role;
    }

    // Helper: Get role badge color
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

// Initialize session manager
const sessionManager = new SessionManager();

// Protect pages that require authentication
function requireAuth() {
    if (!sessionManager.isLoggedIn()) {
        window.location.href = '/Login';
        return false;
    }
    return true;
}

// Redirect to appropriate dashboard based on role
function redirectToDashboard() {
    const user = sessionManager.getCurrentUser();
    if (user) {
        window.location.href = sessionManager.getDashboardUrl(user.role);
    }
}

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = SessionManager;
}
