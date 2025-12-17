// Data Export Manager - Export data to CSV and JSON
class DataExportManager {
    constructor() {
        this.mockDataService = mockDataService;
    }

    // Export to CSV
    exportToCSV(data, filename) {
        if (!data || data.length === 0) {
            errorHandler.showErrorToast('No data to export');
            return;
        }

        const csv = this.convertToCSV(data);
        this.downloadFile(csv, `${filename}.csv`, 'text/csv');
        notificationManager.showToast(`Exported ${data.length} records to CSV`, 'success');
    }

    // Export to JSON
    exportToJSON(data, filename) {
        if (!data || data.length === 0) {
            errorHandler.showErrorToast('No data to export');
            return;
        }

        const json = JSON.stringify(data, null, 2);
        this.downloadFile(json, `${filename}.json`, 'application/json');
        notificationManager.showToast(`Exported ${data.length} records to JSON`, 'success');
    }

    // Convert array of objects to CSV
    convertToCSV(data) {
        if (!data || data.length === 0) return '';

        // Get headers from first object
        const headers = Object.keys(data[0]);
        const csvHeaders = headers.join(',');

        // Convert each row
        const csvRows = data.map(row => {
            return headers.map(header => {
                let value = row[header];

                // Handle special characters
                if (value === null || value === undefined) value = '';
                value = String(value).replace(/"/g, '""'); // Escape quotes

                // Wrap in quotes if contains comma, newline, or quote
                if (value.includes(',') || value.includes('\n') || value.includes('"')) {
                    value = `"${value}"`;
                }

                return value;
            }).join(',');
        });

        return [csvHeaders, ...csvRows].join('\n');
    }

    // Download file
    downloadFile(content, filename, mimeType) {
        const blob = new Blob([content], { type: mimeType });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    }

    // Export all orders
    exportOrders(format = 'csv') {
        const orders = this.mockDataService.getOrders();
        if (format === 'csv') {
            this.exportToCSV(orders, 'orders');
        } else {
            this.exportToJSON(orders, 'orders');
        }
    }

    // Export all products
    exportProducts(format = 'csv') {
        const products = this.mockDataService.getProducts();
        if (format === 'csv') {
            this.exportToCSV(products, 'products');
        } else {
            this.exportToJSON(products, 'products');
        }
    }

    // Export all customers
    exportCustomers(format = 'csv') {
        const customers = this.mockDataService.getCustomers();
        if (format === 'csv') {
            this.exportToCSV(customers, 'customers');
        } else {
            this.exportToJSON(customers, 'customers');
        }
    }

    // Export all suppliers
    exportSuppliers(format = 'csv') {
        const suppliers = this.mockDataService.getSuppliers();
        if (format === 'csv') {
            this.exportToCSV(suppliers, 'suppliers');
        } else {
            this.exportToJSON(suppliers, 'suppliers');
        }
    }

    // Export all data
    exportAllData() {
        const allData = this.mockDataService.getAllData();
        this.exportToJSON(allData, 'silsila-all-data');
    }

    // Export with loading state
    async exportWithLoading(exportFunction, buttonElement) {
        if (buttonElement) {
            loadingManager.showButtonLoading(buttonElement, 'Exporting...');
        } else {
            loadingManager.show('Preparing export...');
        }

        try {
            // Simulate processing time
            await new Promise(resolve => setTimeout(resolve, 500));
            exportFunction();
        } catch (error) {
            errorHandler.handleError(error, 'Export failed');
        } finally {
            if (buttonElement) {
                loadingManager.hideButtonLoading(buttonElement);
            } else {
                loadingManager.hide();
            }
        }
    }
}

// Initialize export manager
const dataExportManager = new DataExportManager();

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = DataExportManager;
}
