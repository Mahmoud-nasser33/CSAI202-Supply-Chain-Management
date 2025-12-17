// Navigation Manager - Handles role-based menu visibility and dynamic UI updates
class NavigationManager {
    constructor() {
        this.init();
    }

    init() {
        // Wait for DOM to be ready
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', () => this.setupNavigation());
        } else {
            this.setupNavigation();
        }
    }

    setupNavigation() {
        const user = sessionManager.getCurrentUser();
        if (!user) return;

        this.user = user; // Store for other methods

        // Update user profile in header
        this.updateUserProfile();

        // Show/hide menu items based on role
        this.filterMenuItems(user.role);

        // Update sidebar text based on role for logical consistency
        this.updateSidebarText(user.role);

        // Add role badge
        this.addRoleBadge(user.role);

        // Highlight active page
        this.highlightActivePage();
    }

    // Update sidebar link text based on role for logical consistency
    updateSidebarText(role) {
        const ordersLink = document.querySelector('[data-page="orders"]');
        const productsLink = document.querySelector('[data-page="products"]');
        const homeLink = document.querySelector('[data-page="home"]');

        if (role === 'customer') {
            if (ordersLink) ordersLink.innerHTML = '<i class="bi bi-bag"></i> My Orders';
            if (productsLink) productsLink.innerHTML = '<i class="bi bi-shop"></i> Browse Shop';
            if (homeLink) homeLink.innerHTML = '<i class="bi bi-house-heart"></i> My Home';
        } else if (role === 'user') { // Warehouse Staff
            if (ordersLink) ordersLink.innerHTML = '<i class="bi bi-clipboard-check"></i> Orders Management';
            if (productsLink) productsLink.innerHTML = '<i class="bi bi-boxes"></i> Master Inventory';
            if (homeLink) homeLink.innerHTML = '<i class="bi bi-speedometer2"></i> Operations Center';
        } else if (role === 'supplier') {
            if (ordersLink) ordersLink.innerHTML = '<i class="bi bi-file-earmark-text"></i> Supply Orders';
            if (homeLink) homeLink.innerHTML = '<i class="bi bi-factory"></i> Production Hub';
        }
    }

    // Update user profile information in the UI
    updateUserProfile() {
        if (!this.user) return;

        // Update user name displays
        const userNameElements = document.querySelectorAll('.user-name, [data-user-name]');
        userNameElements.forEach(el => {
            el.textContent = this.user.name;
        });

        // Update user email displays
        const userEmailElements = document.querySelectorAll('.user-email, [data-user-email]');
        userEmailElements.forEach(el => {
            el.textContent = this.user.email;
        });

        // Update avatar
        const avatarElements = document.querySelectorAll('.user-avatar, [data-user-avatar]');
        avatarElements.forEach(el => {
            if (el.tagName === 'IMG') {
                el.src = `https://ui-avatars.com/api/?name=${encodeURIComponent(this.user.name)}&background=10b981&color=fff`;
                el.alt = this.user.name;
            } else {
                el.textContent = this.user.avatar;
            }
        });

        // Update role display
        const roleDisplay = sessionManager.getRoleDisplayName(this.user.role);
        const roleElements = document.querySelectorAll('.user-role, [data-user-role]');
        roleElements.forEach(el => {
            el.textContent = roleDisplay;
        });

        // Update page title to remove "Admin User" and show actual user
        const pageTitle = document.querySelector('h1, .page-title');
        if (pageTitle && pageTitle.textContent.includes('Admin User')) {
            pageTitle.textContent = pageTitle.textContent.replace('Admin User', this.user.name);
        }

        // Update any hardcoded "Admin User" text
        const adminUserElements = document.querySelectorAll('*');
        adminUserElements.forEach(el => {
            if (el.childNodes.length === 1 && el.childNodes[0].nodeType === 3) { // Text node only
                const text = el.textContent.trim();
                if (text === 'Admin User') {
                    el.textContent = this.user.name;
                }
            }
        });
    }

    filterMenuItems(role) {
        const permissions = sessionManager.getRolePermissions(role);
        const allowedPages = permissions.navigation;

        // Get all menu items
        const menuItems = document.querySelectorAll('[data-page]');

        menuItems.forEach(item => {
            const page = item.getAttribute('data-page');
            if (allowedPages.includes(page)) {
                item.style.display = '';
            } else {
                item.style.display = 'none';
            }
        });

        // Hide entire sections if all items are hidden
        const sections = document.querySelectorAll('.sidebar-label');
        sections.forEach(section => {
            const nextItems = [];
            let next = section.nextElementSibling;

            while (next && !next.classList.contains('sidebar-label')) {
                if (next.hasAttribute('data-page')) {
                    nextItems.push(next);
                }
                next = next.nextElementSibling;
            }

            const allHidden = nextItems.every(item => item.style.display === 'none');
            if (allHidden) {
                section.style.display = 'none';
            }
        });
    }

    addRoleBadge(role) {
        const roleDisplay = sessionManager.getRoleDisplayName(role);
        const roleBadgeColor = sessionManager.getRoleBadgeColor(role);

        // Add badge to sidebar heading
        const sidebarHeading = document.querySelector('.sidebar-heading');
        if (sidebarHeading && !sidebarHeading.querySelector('.role-badge')) {
            const badge = document.createElement('span');
            badge.className = `badge bg-${roleBadgeColor} role-badge ms-2`;
            badge.style.fontSize = '0.65rem';
            badge.textContent = roleDisplay;
            sidebarHeading.appendChild(badge);
        }
    }

    highlightActivePage() {
        const currentPath = window.location.pathname;
        const menuItems = document.querySelectorAll('.list-group-item');

        menuItems.forEach(item => {
            const href = item.getAttribute('href');
            if (href && currentPath.includes(href.replace('/', ''))) {
                item.classList.add('active');
            } else {
                item.classList.remove('active');
            }
        });
    }

    // Add logout functionality
    setupLogout() {
        const logoutButtons = document.querySelectorAll('[data-action="logout"]');
        logoutButtons.forEach(btn => {
            btn.addEventListener('click', (e) => {
                e.preventDefault();
                if (confirm('Are you sure you want to logout?')) {
                    sessionManager.logout();
                }
            });
        });
    }
}

// Initialize navigation manager
const navigationManager = new NavigationManager();

// Setup logout when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    navigationManager.setupLogout();
});
