
class LoadingManager {
    constructor() {
        this.init();
    }

    init() {

        this.createLoadingOverlay();
    }

    createLoadingOverlay() {
        if (document.getElementById('globalLoadingOverlay')) return;

        const overlay = document.createElement('div');
        overlay.id = 'globalLoadingOverlay';
        overlay.className = 'loading-overlay';
        overlay.setAttribute('role', 'status');
        overlay.setAttribute('aria-live', 'polite');
        overlay.setAttribute('aria-label', 'Loading');

        const spinnerContainer = document.createElement('div');
        spinnerContainer.className = 'loading-spinner-container';

        const spinner = document.createElement('div');
        spinner.className = 'loading-spinner';
        spinner.setAttribute('aria-hidden', 'true');

        const text = document.createElement('p');
        text.className = 'loading-text';
        text.textContent = 'Loading...';

        spinnerContainer.appendChild(spinner);
        spinnerContainer.appendChild(text);
        overlay.appendChild(spinnerContainer);
        overlay.style.display = 'none';
        document.body.appendChild(overlay);
    }

    show(message = 'Loading...') {
        const overlay = document.getElementById('globalLoadingOverlay');
        if (overlay) {
            const text = overlay.querySelector('.loading-text');
            if (text) text.textContent = message;
            overlay.style.display = 'flex';
        }
    }

    hide() {
        const overlay = document.getElementById('globalLoadingOverlay');
        if (overlay) {
            overlay.style.display = 'none';
        }
    }

    showButtonLoading(button, text = 'Loading...') {
        if (!button) return;

        const originalContent = button.cloneNode(true);
        button.dataset.originalContent = button.innerHTML;
        button.disabled = true;

        button.textContent = '';
        const spinner = document.createElement('span');
        spinner.className = 'spinner-border spinner-border-sm me-2';
        spinner.setAttribute('role', 'status');
        spinner.setAttribute('aria-hidden', 'true');
        const textNode = document.createTextNode(text);
        button.appendChild(spinner);
        button.appendChild(textNode);
    }

    hideButtonLoading(button) {
        if (!button) return;

        button.disabled = false;
        if (button.dataset.originalContent) {
            button.innerHTML = button.dataset.originalContent;
            delete button.dataset.originalContent;
        }
    }

    showTableSkeleton(tableBody, rows = 5, cols = 6) {
        if (!tableBody) return;

        tableBody.textContent = '';
        for (let i = 0; i < rows; i++) {
            const tr = document.createElement('tr');
            tr.className = 'skeleton-row';
            for (let j = 0; j < cols; j++) {
                const td = document.createElement('td');
                const loader = document.createElement('div');
                loader.className = 'skeleton-loader';
                loader.setAttribute('aria-hidden', 'true');
                td.appendChild(loader);
                tr.appendChild(td);
            }
            tableBody.appendChild(tr);
        }
    }

    showProgress(container, percent = 0) {
        if (!container) return;

        container.textContent = '';
        const progressDiv = document.createElement('div');
        progressDiv.className = 'progress';
        progressDiv.style.height = '8px';

        const progressBar = document.createElement('div');
        progressBar.className = 'progress-bar progress-bar-striped progress-bar-animated';
        progressBar.setAttribute('role', 'progressbar');
        progressBar.style.width = `${percent}%`;
        progressBar.setAttribute('aria-valuenow', percent.toString());
        progressBar.setAttribute('aria-valuemin', '0');
        progressBar.setAttribute('aria-valuemax', '100');
        progressBar.setAttribute('aria-label', `${percent}% complete`);

        progressDiv.appendChild(progressBar);
        container.appendChild(progressDiv);
    }

    updateProgress(container, percent) {
        if (!container) return;

        const progressBar = container.querySelector('.progress-bar');
        if (progressBar) {
            progressBar.style.width = `${percent}%`;
            progressBar.setAttribute('aria-valuenow', percent);
        }
    }

    async simulateLoading(callback, duration = 1000, message = 'Loading...') {
        this.show(message);
        try {
            await new Promise(resolve => setTimeout(resolve, duration));
            if (callback) await callback();
        } finally {
            this.hide();
        }
    }

    showElementLoading(element) {
        if (!element) return;

        element.classList.add('loading-shimmer');
        element.style.pointerEvents = 'none';
    }

    hideElementLoading(element) {
        if (!element) return;

        element.classList.remove('loading-shimmer');
        element.style.pointerEvents = '';
    }
}

const loadingManager = new LoadingManager();

if (typeof module !== 'undefined' && module.exports) {
    module.exports = LoadingManager;
}
