// Modal Manager - Create and manage modal dialogs
class ModalManager {
    constructor() {
        this.modals = new Map();
        this.init();
    }

    init() {
        // Create modal container if it doesn't exist
        if (!document.getElementById('modalContainer')) {
            const container = document.createElement('div');
            container.id = 'modalContainer';
            document.body.appendChild(container);
        }
    }

    // Create a modal
    createModal(id, title, content, options = {}) {
        const modalHtml = `
            <div class="modal fade" id="${id}" tabindex="-1" aria-labelledby="${id}Label" aria-hidden="true">
                <div class="modal-dialog ${options.size || 'modal-lg'}">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="${id}Label">${title}</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            ${content}
                        </div>
                        ${options.footer !== false ? `
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                            <button type="button" class="btn btn-primary" id="${id}ConfirmBtn">
                                ${options.confirmText || 'Confirm'}
                            </button>
                        </div>
                        ` : ''}
                    </div>
                </div>
            </div>
        `;

        const container = document.getElementById('modalContainer');
        container.insertAdjacentHTML('beforeend', modalHtml);

        const modalElement = document.getElementById(id);
        const modal = new bootstrap.Modal(modalElement);
        this.modals.set(id, modal);

        return modal;
    }

    // Show modal
    show(id) {
        const modal = this.modals.get(id);
        if (modal) modal.show();
    }

    // Hide modal
    hide(id) {
        const modal = this.modals.get(id);
        if (modal) modal.hide();
    }

    // Confirmation dialog
    confirm(title, message, onConfirm) {
        const id = 'confirmModal_' + Date.now();
        const content = `<p>${message}</p>`;

        this.createModal(id, title, content, { size: 'modal-sm' });

        const confirmBtn = document.getElementById(id + 'ConfirmBtn');
        confirmBtn.addEventListener('click', () => {
            if (onConfirm) onConfirm();
            this.hide(id);
            this.destroy(id);
        });

        this.show(id);
    }

    // Alert dialog
    alert(title, message) {
        const id = 'alertModal_' + Date.now();
        const content = `<p>${message}</p>`;

        this.createModal(id, title, content, {
            size: 'modal-sm',
            footer: false
        });

        const modalElement = document.getElementById(id);
        modalElement.querySelector('.modal-body').innerHTML += `
            <div class="text-end mt-3">
                <button type="button" class="btn btn-primary" data-bs-dismiss="modal">OK</button>
            </div>
        `;

        this.show(id);
    }

    // Destroy modal
    destroy(id) {
        const modal = this.modals.get(id);
        if (modal) {
            modal.dispose();
            this.modals.delete(id);
            const element = document.getElementById(id);
            if (element) element.remove();
        }
    }
}

// Initialize modal manager
const modalManager = new ModalManager();

// Export
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ModalManager;
}
