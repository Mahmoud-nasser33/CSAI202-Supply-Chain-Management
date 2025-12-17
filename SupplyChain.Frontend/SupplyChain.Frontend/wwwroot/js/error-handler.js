// Error Handler - Global error handling and display
class ErrorHandler {
    constructor() {
        this.init();
    }

    init() {
        // Setup global error handlers
        window.addEventListener('error', (event) => {
            this.handleError(event.error || event.message);
        });

        window.addEventListener('unhandledrejection', (event) => {
            this.handleError(event.reason);
        });
    }

    // Handle and display error
    handleError(error, context = '') {
        console.error('Error:', error, context);

        const errorMessage = this.getErrorMessage(error);
        this.showErrorToast(errorMessage);

        // Log error for debugging
        this.logError(error, context);
    }

    // Get user-friendly error message
    getErrorMessage(error) {
        if (typeof error === 'string') return error;
        if (error?.message) return error.message;
        return 'An unexpected error occurred. Please try again.';
    }

    // Show error as toast notification
    showErrorToast(message) {
        if (typeof notificationManager !== 'undefined') {
            notificationManager.showToast(message, 'error');
        } else {
            alert(message);
        }
    }

    // Show error in specific container
    showError(container, message, title = 'Error') {
        if (!container) return;

        const errorHtml = `
            <div class="error-state fade-in">
                <div class="d-flex align-items-center">
                    <i class="bi bi-exclamation-circle-fill error-state-icon"></i>
                    <div>
                        <div class="error-state-message">${title}</div>
                        <div class="text-muted small">${message}</div>
                    </div>
                </div>
            </div>
        `;
        container.innerHTML = errorHtml;
    }

    // Show success message
    showSuccess(container, message, title = 'Success') {
        if (!container) return;

        const successHtml = `
            <div class="success-state fade-in">
                <div class="d-flex align-items-center">
                    <i class="bi bi-check-circle-fill success-state-icon"></i>
                    <div>
                        <div class="fw-bold text-success">${title}</div>
                        <div class="text-muted small">${message}</div>
                    </div>
                </div>
            </div>
        `;
        container.innerHTML = successHtml;
    }

    // Validate form field
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

    // Show field error
    showFieldError(field, message) {
        if (!field) return;

        // Remove existing error
        this.clearFieldError(field);

        // Add error class
        field.classList.add('is-invalid');

        // Create error message
        const errorDiv = document.createElement('div');
        errorDiv.className = 'invalid-feedback';
        errorDiv.textContent = message;
        field.parentNode.appendChild(errorDiv);
    }

    // Clear field error
    clearFieldError(field) {
        if (!field) return;

        field.classList.remove('is-invalid');
        const errorDiv = field.parentNode.querySelector('.invalid-feedback');
        if (errorDiv) errorDiv.remove();
    }

    // Validate form
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

    // Email validation
    isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    // Handle network error
    handleNetworkError(error) {
        const message = 'Network error. Please check your connection and try again.';
        this.showErrorToast(message);
    }

    // Handle not found error
    handleNotFoundError(resource = 'Resource') {
        const message = `${resource} not found.`;
        this.showErrorToast(message);
    }

    // Handle permission error
    handlePermissionError() {
        const message = 'You do not have permission to perform this action.';
        this.showErrorToast(message);
    }

    // Log error (could send to server in production)
    logError(error, context) {
        const errorLog = {
            timestamp: new Date().toISOString(),
            error: error?.toString() || 'Unknown error',
            stack: error?.stack,
            context: context,
            userAgent: navigator.userAgent,
            url: window.location.href
        };

        // Store in localStorage for debugging
        const logs = JSON.parse(localStorage.getItem('error_logs') || '[]');
        logs.push(errorLog);
        // Keep only last 50 errors
        if (logs.length > 50) logs.shift();
        localStorage.setItem('error_logs', JSON.stringify(logs));

        // In production, you would send this to a logging service
        console.log('Error logged:', errorLog);
    }

    // Get error logs
    getErrorLogs() {
        return JSON.parse(localStorage.getItem('error_logs') || '[]');
    }

    // Clear error logs
    clearErrorLogs() {
        localStorage.removeItem('error_logs');
    }
}

// Initialize error handler
const errorHandler = new ErrorHandler();

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ErrorHandler;
}
