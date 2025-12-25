
class ErrorHandler {
    constructor() {
        this.init();
    }

    init() {

        window.addEventListener('error', (event) => {
            this.handleError(event.error || event.message);
        });

        window.addEventListener('unhandledrejection', (event) => {
            this.handleError(event.reason);
        });
    }

    handleError(error, context = '') {
        console.error('Error:', error, context);

        const errorMessage = this.getErrorMessage(error);
        this.showErrorToast(errorMessage);

        this.logError(error, context);
    }

    getErrorMessage(error) {
        if (typeof error === 'string') return error;
        if (error?.message) return error.message;
        return 'An unexpected error occurred. Please try again.';
    }

    showErrorToast(message) {
        if (typeof notificationManager !== 'undefined') {
            notificationManager.showToast(message, 'error');
        } else {
            alert(message);
        }
    }

    showError(container, message, title = 'Error') {
        if (!container) return;

        container.textContent = '';
        const errorDiv = document.createElement('div');
        errorDiv.className = 'error-state fade-in';
        errorDiv.setAttribute('role', 'alert');

        const innerDiv = document.createElement('div');
        innerDiv.className = 'd-flex align-items-center';

        const icon = document.createElement('i');
        icon.className = 'bi bi-exclamation-circle-fill error-state-icon';
        icon.setAttribute('aria-hidden', 'true');

        const contentDiv = document.createElement('div');
        const titleDiv = document.createElement('div');
        titleDiv.className = 'error-state-message';
        titleDiv.textContent = title;
        const messageDiv = document.createElement('div');
        messageDiv.className = 'text-muted small';
        messageDiv.textContent = message;

        contentDiv.appendChild(titleDiv);
        contentDiv.appendChild(messageDiv);
        innerDiv.appendChild(icon);
        innerDiv.appendChild(contentDiv);
        errorDiv.appendChild(innerDiv);
        container.appendChild(errorDiv);
    }

    showSuccess(container, message, title = 'Success') {
        if (!container) return;

        container.textContent = '';
        const successDiv = document.createElement('div');
        successDiv.className = 'success-state fade-in';
        successDiv.setAttribute('role', 'status');
        successDiv.setAttribute('aria-live', 'polite');

        const innerDiv = document.createElement('div');
        innerDiv.className = 'd-flex align-items-center';

        const icon = document.createElement('i');
        icon.className = 'bi bi-check-circle-fill success-state-icon';
        icon.setAttribute('aria-hidden', 'true');

        const contentDiv = document.createElement('div');
        const titleDiv = document.createElement('div');
        titleDiv.className = 'fw-bold text-success';
        titleDiv.textContent = title;
        const messageDiv = document.createElement('div');
        messageDiv.className = 'text-muted small';
        messageDiv.textContent = message;

        contentDiv.appendChild(titleDiv);
        contentDiv.appendChild(messageDiv);
        innerDiv.appendChild(icon);
        innerDiv.appendChild(contentDiv);
        successDiv.appendChild(innerDiv);
        container.appendChild(successDiv);
    }

    validateField(field, rules) {
        const value = field.value.trim();
        const errors = [];

        if (rules.required && !value) {
            errors.push(`${field.name || 'This field'} is required`);
        }

        if (rules.minLength && value.length < rules.minLength) {
            errors.push(`Minimum length is ${rules.minLength} characters`);
        }

        if (rules.maxLength && value.length > rules.maxLength) {
            errors.push(`Maximum length is ${rules.maxLength} characters`);
        }

        if (rules.email && value && !this.isValidEmail(value)) {
            errors.push('Please enter a valid email address');
        }

        if (rules.number && value && isNaN(value)) {
            errors.push('Please enter a valid number');
        }

        if (rules.min && parseFloat(value) < rules.min) {
            errors.push(`Minimum value is ${rules.min}`);
        }

        if (rules.max && parseFloat(value) > rules.max) {
            errors.push(`Maximum value is ${rules.max}`);
        }

        if (rules.pattern && value && !rules.pattern.test(value)) {
            errors.push(rules.patternMessage || 'Invalid format');
        }

        return errors;
    }

    showFieldError(field, message) {
        if (!field) return;

        this.clearFieldError(field);

        field.classList.add('is-invalid');

        const errorDiv = document.createElement('div');
        errorDiv.className = 'invalid-feedback';
        errorDiv.textContent = message;
        field.parentNode.appendChild(errorDiv);
    }

    clearFieldError(field) {
        if (!field) return;

        field.classList.remove('is-invalid');
        const errorDiv = field.parentNode.querySelector('.invalid-feedback');
        if (errorDiv) errorDiv.remove();
    }

    validateForm(form, rules) {
        if (!form) return { valid: false, errors: {} };

        const errors = {};
        let isValid = true;

        Object.keys(rules).forEach(fieldName => {
            const field = form.querySelector(`[name="${fieldName}"]`);
            if (!field) return;

            const fieldErrors = this.validateField(field, rules[fieldName]);
            if (fieldErrors.length > 0) {
                errors[fieldName] = fieldErrors;
                this.showFieldError(field, fieldErrors[0]);
                isValid = false;
            } else {
                this.clearFieldError(field);
            }
        });

        return { valid: isValid, errors };
    }

    isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    handleNetworkError(error) {
        const message = 'Network error. Please check your connection and try again.';
        this.showErrorToast(message);
    }

    handleNotFoundError(resource = 'Resource') {
        const message = `${resource} not found.`;
        this.showErrorToast(message);
    }

    handlePermissionError() {
        const message = 'You do not have permission to perform this action.';
        this.showErrorToast(message);
    }

    logError(error, context) {
        const errorLog = {
            timestamp: new Date().toISOString(),
            error: error?.toString() || 'Unknown error',
            stack: error?.stack,
            context: context,
            userAgent: navigator.userAgent,
            url: window.location.href
        };

        const logs = JSON.parse(localStorage.getItem('error_logs') || '[]');
        logs.push(errorLog);

        const maxLogs = (typeof AppConfig !== 'undefined' && AppConfig.table) ? AppConfig.table.maxErrorLogs : 50;
        if (logs.length > maxLogs) logs.shift();
        localStorage.setItem('error_logs', JSON.stringify(logs));

    }

    getErrorLogs() {
        return JSON.parse(localStorage.getItem('error_logs') || '[]');
    }

    clearErrorLogs() {
        localStorage.removeItem('error_logs');
    }
}

const errorHandler = new ErrorHandler();

if (typeof module !== 'undefined' && module.exports) {
    module.exports = ErrorHandler;
}
