
class ModalManager {
    constructor() {
        this.modals = new Map();
        this.init();
    }

    init() {

        const setupContainer = () => {
            if (!document.getElementById('modalContainer')) {
                const container = document.createElement('div');
                container.id = 'modalContainer';
                document.body.appendChild(container);
            }
        };

        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', setupContainer);
        } else {
            setupContainer();
        }
    }

    createModal(id, title, content, options = {}) {
        const modalEl = document.createElement('div');
        modalEl.className = 'modal fade';
        modalEl.id = id;
        modalEl.setAttribute('tabindex', '-1');
        modalEl.setAttribute('aria-labelledby', `${id}Label`);
        modalEl.setAttribute('aria-hidden', 'true');
        modalEl.setAttribute('role', 'dialog');

        const dialog = document.createElement('div');
        dialog.className = `modal-dialog ${options.size || 'modal-lg'}`;

        const modalContent = document.createElement('div');
        modalContent.className = 'modal-content';

        const header = document.createElement('div');
        header.className = 'modal-header';
        const titleEl = document.createElement('h5');
        titleEl.className = 'modal-title';
        titleEl.id = `${id}Label`;
        titleEl.textContent = title;
        const closeBtn = document.createElement('button');
        closeBtn.type = 'button';
        closeBtn.className = 'btn-close';
        closeBtn.setAttribute('data-bs-dismiss', 'modal');
        closeBtn.setAttribute('aria-label', 'Close');
        header.appendChild(titleEl);
        header.appendChild(closeBtn);

        const body = document.createElement('div');
        body.className = 'modal-body';

        if (content instanceof Node) {
            body.appendChild(content);
        } else if (typeof content === 'string') {

            const textNode = document.createTextNode(content);
            body.appendChild(textNode);
        }

        modalContent.appendChild(header);
        modalContent.appendChild(body);

        if (options.footer !== false) {
            const footer = document.createElement('div');
            footer.className = 'modal-footer';
            const cancelBtn = document.createElement('button');
            cancelBtn.type = 'button';
            cancelBtn.className = 'btn btn-secondary';
            cancelBtn.setAttribute('data-bs-dismiss', 'modal');
            cancelBtn.textContent = 'Cancel';
            const confirmBtn = document.createElement('button');
            confirmBtn.type = 'button';
            confirmBtn.className = 'btn btn-primary';
            confirmBtn.id = `${id}ConfirmBtn`;
            confirmBtn.textContent = options.confirmText || 'Confirm';
            footer.appendChild(cancelBtn);
            footer.appendChild(confirmBtn);
            modalContent.appendChild(footer);
        }

        dialog.appendChild(modalContent);
        modalEl.appendChild(dialog);

        const container = document.getElementById('modalContainer');
        container.appendChild(modalEl);

        const modalElement = document.getElementById(id);
        const modal = new bootstrap.Modal(modalElement);
        this.modals.set(id, modal);

        return modal;
    }

    show(id) {
        const modal = this.modals.get(id);
        if (modal) modal.show();
    }

    hide(id) {
        const modal = this.modals.get(id);
        if (modal) modal.hide();
    }

    confirm(title, message, onConfirm) {
        const id = 'confirmModal_' + Date.now();
        const messageEl = document.createElement('p');
        messageEl.textContent = message;

        this.createModal(id, title, messageEl, { size: 'modal-sm' });

        const modalElement = document.getElementById(id);
        const confirmBtn = document.getElementById(id + 'ConfirmBtn');
        if (confirmBtn) {
            confirmBtn.addEventListener('click', () => {
                if (onConfirm) onConfirm();
                this.hide(id);
                this.destroy(id);
            });

            if (modalElement) {
                modalElement.addEventListener('shown.bs.modal', () => {
                    const firstInput = modalElement.querySelector('input, textarea, select, button');
                    if (firstInput) firstInput.focus();
                });

                modalElement.addEventListener('hidden.bs.modal', () => {

                    const trigger = document.activeElement;
                    if (trigger && trigger.hasAttribute('data-bs-toggle')) {
                        trigger.focus();
                    }
                });
            }
        }

        this.show(id);
    }

    alert(title, message) {
        const id = 'alertModal_' + Date.now();
        const messageEl = document.createElement('p');
        messageEl.textContent = message;

        this.createModal(id, title, messageEl, {
            size: 'modal-sm',
            footer: false
        });

        const modalElement = document.getElementById(id);
        if (modalElement) {
            const modalBody = modalElement.querySelector('.modal-body');
            if (modalBody) {
                const buttonDiv = document.createElement('div');
                buttonDiv.className = 'text-end mt-3';
                const okButton = document.createElement('button');
                okButton.type = 'button';
                okButton.className = 'btn btn-primary';
                okButton.setAttribute('data-bs-dismiss', 'modal');
                okButton.textContent = 'OK';
                buttonDiv.appendChild(okButton);
                modalBody.appendChild(buttonDiv);
            }

            modalElement.addEventListener('shown.bs.modal', () => {
                const okBtn = modalElement.querySelector('.btn-primary');
                if (okBtn) okBtn.focus();
            });
        }

        this.show(id);
    }

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

const modalManager = new ModalManager();

if (typeof module !== 'undefined' && module.exports) {
    module.exports = ModalManager;
}
