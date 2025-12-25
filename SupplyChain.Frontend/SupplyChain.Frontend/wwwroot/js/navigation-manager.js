
class NavigationManager {
    constructor() {
        this.init();
    }

    init() {

        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', () => this.setupNavigation());
        } else {
            this.setupNavigation();
        }
    }

    setupNavigation() {
        const user = sessionManager.getCurrentUser();
        if (!user) return;

        this.user = user;

        this.updateUserProfile();

        this.filterMenuItems(user.role);

        this.updateSidebarText(user.role);

        this.addRoleBadge(user.role);

        this.highlightActivePage();
    }

    updateSidebarText(role) {
        const ordersLink = document.querySelector('[data-page="orders"]');
        const productsLink = document.querySelector('[data-page="products"]');
        const homeLink = document.querySelector('[data-page="home"]');

        const updateLink = (link, iconClass, text) => {
            if (!link) return;
            link.textContent = '';
            const icon = document.createElement('i');
            icon.className = `bi ${iconClass}`;
            icon.setAttribute('aria-hidden', 'true');
            link.appendChild(icon);
            link.appendChild(document.createTextNode(` ${text}`));
        };

        if (role === 'customer') {
            updateLink(ordersLink, 'bi-bag', 'My Orders');
            updateLink(productsLink, 'bi-shop', 'Browse Shop');
            updateLink(homeLink, 'bi-house-heart', 'My Home');
        } else if (role === 'user') {
            updateLink(ordersLink, 'bi-clipboard-check', 'Orders Management');
            updateLink(productsLink, 'bi-boxes', 'Master Inventory');
            updateLink(homeLink, 'bi-speedometer2', 'Operations Center');
        } else if (role === 'supplier') {
            updateLink(ordersLink, 'bi-file-earmark-text', 'Supply Orders');
            updateLink(homeLink, 'bi-factory', 'Production Hub');
        }
    }

    updateUserProfile() {
        if (!this.user) return;

        const userNameElements = document.querySelectorAll('.user-name, [data-user-name]');
        userNameElements.forEach(el => {
            el.textContent = this.user.name;
        });

        const userEmailElements = document.querySelectorAll('.user-email, [data-user-email]');
        userEmailElements.forEach(el => {
            el.textContent = this.user.email;
        });

        const avatarElements = document.querySelectorAll('.user-avatar, [data-user-avatar]');
        avatarElements.forEach(el => {
            if (el.tagName === 'IMG') {
                const avatarConfig = (typeof AppConfig !== 'undefined' && AppConfig.avatar) ? AppConfig.avatar : { baseUrl: 'https://ui-avatars.com/api/', defaultBackground: '10b981', defaultColor: 'fff' };
                el.src = `${avatarConfig.baseUrl}?name=${encodeURIComponent(this.user.name)}&background=${avatarConfig.defaultBackground}&color=${avatarConfig.defaultColor}`;
                el.alt = this.user.name;
            } else {
                el.textContent = this.user.avatar;
            }
        });

        const roleDisplay = sessionManager.getRoleDisplayName(this.user.role);
        const roleElements = document.querySelectorAll('.user-role, [data-user-role]');
        roleElements.forEach(el => {
            el.textContent = roleDisplay;
        });

        const pageTitle = document.querySelector('h1, .page-title');
        if (pageTitle && pageTitle.textContent.includes('Admin User')) {
            pageTitle.textContent = pageTitle.textContent.replace('Admin User', this.user.name);
        }

        const adminUserElements = document.querySelectorAll('*');
        adminUserElements.forEach(el => {
            if (el.childNodes.length === 1 && el.childNodes[0].nodeType === 3) {
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

        const menuItems = document.querySelectorAll('[data-page]');

        menuItems.forEach(item => {
            const page = item.getAttribute('data-page');
            if (allowedPages.includes(page)) {
                item.style.display = '';
            } else {
                item.style.display = 'none';
            }
        });

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

const navigationManager = new NavigationManager();

document.addEventListener('DOMContentLoaded', () => {
    navigationManager.setupLogout();
});
