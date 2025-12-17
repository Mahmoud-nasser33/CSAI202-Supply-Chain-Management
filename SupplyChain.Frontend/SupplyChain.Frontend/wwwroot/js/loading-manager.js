// Loading Manager - Centralized loading state management
class LoadingManager {
    constructor() {
        this.init();
    }

    init() {
        // Create global loading overlay
        this.createLoadingOverlay();
    }

    createLoadingOverlay() {
        if (document.getElementById('globalLoadingOverlay')) return;

        const overlay = document.createElement('div');
        overlay.id = 'globalLoadingOverlay';
        overlay.className = 'loading-overlay';
        overlay.innerHTML = `
            <div class="loading-spinner-container">
                <div class="loading-spinner"></div>
                <p class="loading-text">Loading...</p>
            </div>
        `;
        overlay.style.display = 'none';
        document.body.appendChild(overlay);
    }

    // Show global loading overlay
    show(message = 'Loading...') {
        const overlay = document.getElementById('globalLoadingOverlay');
        if (overlay) {
            const text = overlay.querySelector('.loading-text');
            if (text) text.textContent = message;
            overlay.style.display = 'flex';
        }
    }

    // Hide global loading overlay
    hide() {
        const overlay = document.getElementById('globalLoadingOverlay');
        if (overlay) {
            overlay.style.display = 'none';
        }
    }

    // Show loading on specific button
    showButtonLoading(button, text = 'Loading...') {
        if (!button) return;

        // Store original content
        button.dataset.originalContent = button.innerHTML;
        button.disabled = true;

        // Add spinner
        button.innerHTML = `
            <span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
            ${text}
        `;
    }

    // Hide loading on button
    hideButtonLoading(button) {
        if (!button) return;

        button.disabled = false;
        if (button.dataset.originalContent) {
            button.innerHTML = button.dataset.originalContent;
            delete button.dataset.originalContent;
        }
    }

    // Show skeleton loader for table
    showTableSkeleton(tableBody, rows = 5, cols = 6) {
        if (!tableBody) return;

        let html = '';
        for (let i = 0; i < rows; i++) {
            html += '<tr class="skeleton-row">';
            for (let j = 0; j < cols; j++) {
                html += '<td><div class="skeleton-loader"></div></td>';
            }
            html += '</tr>';
        }
        tableBody.innerHTML = html;
    }

    // Show progress bar
    showProgress(container, percent = 0) {
        if (!container) return;

        const progressHtml = `
            <div class="progress" style="height: 8px;">
                <div class="progress-bar progress-bar-striped progress-bar-animated" 
                     role="progressbar" 
                     style="width: ${percent}%" 
                     aria-valuenow="${percent}" 
                     aria-valuemin="0" 
                     aria-valuemax="100">
                </div>
            </div>
        `;
        container.innerHTML = progressHtml;
    }

    // Update progress
    updateProgress(container, percent) {
        if (!container) return;

        const progressBar = container.querySelector('.progress-bar');
        if (progressBar) {
            progressBar.style.width = `${percent}%`;
            progressBar.setAttribute('aria-valuenow', percent);
        }
    }

    // Simulate async operation with loading
    async simulateLoading(callback, duration = 1000, message = 'Loading...') {
        this.show(message);
        try {
            await new Promise(resolve => setTimeout(resolve, duration));
            if (callback) await callback();
        } finally {
            this.hide();
        }
    }

    // Show loading for element
    showElementLoading(element) {
        if (!element) return;

        element.classList.add('loading-shimmer');
        element.style.pointerEvents = 'none';
    }

    // Hide loading for element
    hideElementLoading(element) {
        if (!element) return;

        element.classList.remove('loading-shimmer');
        element.style.pointerEvents = '';
    }
}

// Initialize loading manager
const loadingManager = new LoadingManager();

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = LoadingManager;
}
