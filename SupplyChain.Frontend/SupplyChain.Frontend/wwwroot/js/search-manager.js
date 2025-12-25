
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
        const debounceTime = (typeof AppConfig !== 'undefined' && AppConfig.ui) ? AppConfig.ui.searchDebounce : 300;
        this.searchInput.addEventListener('input', (e) => {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(() => {
                this.performSearch(e.target.value);
            }, debounceTime);
        });

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

        return allResults;
    }

    displayResults(results, query) {

        const existing = document.getElementById('searchResults');
        if (existing) existing.remove();

        if (results.length === 0) {
            this.showNoResults(query);
            return;
        }

        const dropdown = document.createElement('div');
        dropdown.id = 'searchResults';
        dropdown.className = 'position-absolute bg-white shadow-lg rounded border mt-1';
        dropdown.style.cssText = 'width: 400px; max-height: 400px; overflow-y: auto; z-index: 1050; top: 100%;';

        const header = document.createElement('div');
        header.className = 'p-2 border-bottom bg-light';
        const headerText = document.createElement('small');
        headerText.className = 'text-muted';
        const resultCount = results.length;
        headerText.textContent = `Found ${resultCount} result${resultCount !== 1 ? 's' : ''} for "${query}"`;
        header.appendChild(headerText);
        dropdown.appendChild(header);

        results.forEach(result => {
            const link = document.createElement('a');
            link.href = result.link;
            link.className = 'd-block text-decoration-none text-dark p-3 border-bottom search-result-item';
            link.style.transition = 'background 0.2s';

            const itemDiv = document.createElement('div');
            itemDiv.className = 'd-flex align-items-center gap-2';

            const iconDiv = document.createElement('div');
            iconDiv.className = 'rounded-circle bg-primary bg-opacity-10 p-2 search-result-icon';
            const icon = document.createElement('i');
            icon.className = `bi bi-${result.icon} text-primary`;
            icon.setAttribute('aria-hidden', 'true');
            iconDiv.appendChild(icon);

            const contentDiv = document.createElement('div');
            contentDiv.className = 'flex-grow-1';
            const titleDiv = document.createElement('div');
            titleDiv.className = 'fw-bold small';
            titleDiv.textContent = result.title;
            const subtitleDiv = document.createElement('div');
            subtitleDiv.className = 'text-muted';
            subtitleDiv.style.fontSize = '0.75rem';
            subtitleDiv.textContent = result.subtitle;
            contentDiv.appendChild(titleDiv);
            contentDiv.appendChild(subtitleDiv);

            const badge = document.createElement('span');
            badge.className = 'badge bg-secondary';
            badge.textContent = result.type;

            itemDiv.appendChild(iconDiv);
            itemDiv.appendChild(contentDiv);
            itemDiv.appendChild(badge);
            link.appendChild(itemDiv);
            dropdown.appendChild(link);
        });

        const inputParent = this.searchInput.parentElement;
        inputParent.style.position = 'relative';
        inputParent.appendChild(dropdown);

        const items = dropdown.querySelectorAll('.search-result-item');
        items.forEach(item => {

        });

        setTimeout(() => {
            document.addEventListener('click', this.closeResultsOnClickOutside.bind(this), { once: true });
        }, 100);
    }

    showNoResults(query) {
        const dropdown = document.createElement('div');
        dropdown.id = 'searchResults';
        dropdown.className = 'position-absolute bg-white shadow-lg rounded border mt-1 p-4 text-center';
        dropdown.style.cssText = 'width: 400px; z-index: 1050; top: 100%;';
        const icon = document.createElement('i');
        icon.className = 'bi bi-search text-muted';
        icon.style.fontSize = '2rem';
        icon.setAttribute('aria-hidden', 'true');
        const message = document.createElement('p');
        message.className = 'text-muted mb-0 mt-2';
        message.textContent = `No results found for "${query}"`;
        dropdown.appendChild(icon);
        dropdown.appendChild(message);

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

const searchManager = new SearchManager();

if (typeof module !== 'undefined' && module.exports) {
    module.exports = SearchManager;
}
