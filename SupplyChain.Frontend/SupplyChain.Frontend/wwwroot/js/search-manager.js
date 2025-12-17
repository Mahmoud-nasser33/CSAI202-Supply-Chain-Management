// Global Search Manager - Handles search across all entities
class SearchManager {
    constructor() {
        this.searchInput = null;
        this.init();
    }

    init() {
        document.addEventListener('DOMContentLoaded', () => {
            this.searchInput = document.getElementById('globalSearch');
            if (this.searchInput) {
                this.setupSearchListener();
            }
        });
    }

    setupSearchListener() {
        let searchTimeout;
        this.searchInput.addEventListener('input', (e) => {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(() => {
                this.performSearch(e.target.value);
            }, 300); // Debounce 300ms
        });

        // Handle Enter key
        this.searchInput.addEventListener('keypress', (e) => {
            if (e.key === 'Enter') {
                this.performSearch(e.target.value);
            }
        });
    }

    performSearch(query) {
        if (!query || query.length < 2) {
            this.hideResults();
            return;
        }

        const results = this.searchAll(query);
        this.displayResults(results, query);
    }

    searchAll(query) {
        const lowerQuery = query.toLowerCase();
        const allResults = [];

        // Search Orders
        const orders = mockDataService.getOrders();
        orders.forEach(order => {
            if (order.id.toLowerCase().includes(lowerQuery) ||
                order.customerName.toLowerCase().includes(lowerQuery) ||
                order.status.toLowerCase().includes(lowerQuery)) {
                allResults.push({
                    type: 'Order',
                    icon: 'cart-check',
                    title: `Order ${order.id}`,
                    subtitle: `${order.customerName} - $${order.totalAmount}`,
                    link: `/Orders?id=${order.id}`,
                    data: order
                });
            }
        });

        // Search Products
        const products = mockDataService.getProducts();
        products.forEach(product => {
            if (product.name.toLowerCase().includes(lowerQuery) ||
                product.sku.toLowerCase().includes(lowerQuery) ||
                product.category.toLowerCase().includes(lowerQuery)) {
                allResults.push({
                    type: 'Product',
                    icon: 'box',
                    title: product.name,
                    subtitle: `${product.sku} - $${product.price}`,
                    link: `/Products?id=${product.id}`,
                    data: product
                });
            }
        });

        // Search Customers
        const customers = mockDataService.getCustomers();
        customers.forEach(customer => {
            if (customer.name.toLowerCase().includes(lowerQuery) ||
                customer.email.toLowerCase().includes(lowerQuery)) {
                allResults.push({
                    type: 'Customer',
                    icon: 'person',
                    title: customer.name,
                    subtitle: customer.email,
                    link: `/Customers?id=${customer.id}`,
                    data: customer
                });
            }
        });

        return allResults.slice(0, 10); // Limit to 10 results
    }

    displayResults(results, query) {
        // Remove existing results dropdown
        const existing = document.getElementById('searchResults');
        if (existing) existing.remove();

        if (results.length === 0) {
            this.showNoResults(query);
            return;
        }

        // Create results dropdown
        const dropdown = document.createElement('div');
        dropdown.id = 'searchResults';
        dropdown.className = 'position-absolute bg-white shadow-lg rounded border mt-1';
        dropdown.style.cssText = 'width: 400px; max-height: 400px; overflow-y: auto; z-index: 1050; top: 100%;';

        let html = `
            <div class="p-2 border-bottom bg-light">
                <small class="text-muted">Found ${results.length} result${results.length !== 1 ? 's' : ''} for "${query}"</small>
            </div>
        `;

        results.forEach(result => {
            html += `
                <a href="${result.link}" class="d-block text-decoration-none text-dark p-3 border-bottom search-result-item" style="transition: background 0.2s;">
                    <div class="d-flex align-items-center gap-2">
                        <div class="rounded-circle bg-primary bg-opacity-10 p-2" style="width: 35px; height: 35px;">
                            <i class="bi bi-${result.icon} text-primary"></i>
                        </div>
                        <div class="flex-grow-1">
                            <div class="fw-bold small">${result.title}</div>
                            <div class="text-muted" style="font-size: 0.75rem;">${result.subtitle}</div>
                        </div>
                        <span class="badge bg-secondary">${result.type}</span>
                    </div>
                </a>
            `;
        });

        dropdown.innerHTML = html;

        // Position relative to search input
        const inputParent = this.searchInput.parentElement;
        inputParent.style.position = 'relative';
        inputParent.appendChild(dropdown);

        // Add hover effect
        const items = dropdown.querySelectorAll('.search-result-item');
        items.forEach(item => {
            item.addEventListener('mouseenter', () => {
                item.style.background = '#f8f9fa';
            });
            item.addEventListener('mouseleave', () => {
                item.style.background = '';
            });
        });

        // Close on click outside
        setTimeout(() => {
            document.addEventListener('click', this.closeResultsOnClickOutside.bind(this), { once: true });
        }, 100);
    }

    showNoResults(query) {
        const dropdown = document.createElement('div');
        dropdown.id = 'searchResults';
        dropdown.className = 'position-absolute bg-white shadow-lg rounded border mt-1 p-4 text-center';
        dropdown.style.cssText = 'width: 400px; z-index: 1050; top: 100%;';
        dropdown.innerHTML = `
            <i class="bi bi-search text-muted" style="font-size: 2rem;"></i>
            <p class="text-muted mb-0 mt-2">No results found for "${query}"</p>
        `;

        const inputParent = this.searchInput.parentElement;
        inputParent.style.position = 'relative';
        inputParent.appendChild(dropdown);

        setTimeout(() => {
            document.addEventListener('click', this.closeResultsOnClickOutside.bind(this), { once: true });
        }, 100);
    }

    closeResultsOnClickOutside(e) {
        const results = document.getElementById('searchResults');
        if (results && !results.contains(e.target) && e.target !== this.searchInput) {
            this.hideResults();
        }
    }

    hideResults() {
        const results = document.getElementById('searchResults');
        if (results) results.remove();
    }
}

// Initialize search manager
const searchManager = new SearchManager();

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = SearchManager;
}
