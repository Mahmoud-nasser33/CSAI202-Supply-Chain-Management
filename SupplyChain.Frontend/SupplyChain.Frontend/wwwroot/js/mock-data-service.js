// Mock Data Service - Generates and manages realistic sample data
class MockDataService {
    constructor() {
        this.STORAGE_KEY = 'silsila_mock_data';
        this.init();
    }

    init() {
        // Check if data exists, if not, generate it
        if (!localStorage.getItem(this.STORAGE_KEY)) {
            this.generateAllData();
        }
    }

    // Generate all mock data
    generateAllData() {
        const data = {
            orders: this.generateOrders(100),
            products: this.generateProducts(50),
            customers: this.generateCustomers(30),
            suppliers: this.generateSuppliers(20),
            inventory: this.generateInventory(50),
            shipments: this.generateShipments(80),
            payments: this.generatePayments(60),
            warehouses: this.generateWarehouses(5)
        };

        localStorage.setItem(this.STORAGE_KEY, JSON.stringify(data));
        return data;
    }

    // Get all data
    getAllData() {
        const data = localStorage.getItem(this.STORAGE_KEY);
        return data ? JSON.parse(data) : this.generateAllData();
    }

    // Get specific entity data
    getOrders() { return this.getAllData().orders; }
    getProducts() { return this.getAllData().products; }
    getCustomers() { return this.getAllData().customers; }
    getSuppliers() { return this.getAllData().suppliers; }
    getInventory() { return this.getAllData().inventory; }
    getShipments() { return this.getAllData().shipments; }
    getPayments() { return this.getAllData().payments; }
    getWarehouses() { return this.getAllData().warehouses; }

    // Generate Orders
    generateOrders(count) {
        const statuses = ['Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled'];
        const orders = [];

        for (let i = 0; i < count; i++) {
            const orderDate = this.randomDate(new Date(2024, 0, 1), new Date());
            orders.push({
                id: `ORD-${1000 + i}`,
                customerId: `CUST-${Math.floor(Math.random() * 30) + 1}`,
                customerName: this.randomName(),
                orderDate: orderDate.toISOString(),
                totalAmount: this.randomNumber(100, 5000),
                status: statuses[Math.floor(Math.random() * statuses.length)],
                items: this.randomNumber(1, 10),
                shippingAddress: this.randomAddress(),
                paymentMethod: this.randomChoice(['Credit Card', 'PayPal', 'Bank Transfer']),
                notes: this.randomChoice(['', '', 'Urgent delivery', 'Gift wrap requested', 'Fragile items'])
            });
        }

        return orders;
    }

    // Generate Products
    generateProducts(count) {
        const categories = ['Electronics', 'Clothing', 'Food', 'Furniture', 'Tools', 'Books', 'Toys'];
        const products = [];

        for (let i = 0; i < count; i++) {
            const category = categories[Math.floor(Math.random() * categories.length)];
            products.push({
                id: `PROD-${1000 + i}`,
                name: `${category} Product ${i + 1}`,
                sku: `SKU-${Math.random().toString(36).substr(2, 9).toUpperCase()}`,
                category: category,
                price: this.randomNumber(10, 1000),
                cost: this.randomNumber(5, 500),
                stock: this.randomNumber(0, 500),
                minStock: this.randomNumber(10, 50),
                supplier: `SUP-${Math.floor(Math.random() * 20) + 1}`,
                description: `High quality ${category.toLowerCase()} product`,
                weight: this.randomNumber(0.1, 50, 2),
                dimensions: `${this.randomNumber(5, 50)}x${this.randomNumber(5, 50)}x${this.randomNumber(5, 50)} cm`
            });
        }

        return products;
    }

    // Generate Customers
    generateCustomers(count) {
        const customers = [];

        for (let i = 0; i < count; i++) {
            const name = this.randomName();
            customers.push({
                id: `CUST-${i + 1}`,
                name: name,
                email: `${name.toLowerCase().replace(' ', '.')}@example.com`,
                phone: this.randomPhone(),
                address: this.randomAddress(),
                city: this.randomCity(),
                country: 'USA',
                totalOrders: this.randomNumber(1, 50),
                totalSpent: this.randomNumber(100, 10000),
                joinDate: this.randomDate(new Date(2020, 0, 1), new Date()).toISOString(),
                status: this.randomChoice(['Active', 'Active', 'Active', 'Inactive'])
            });
        }

        return customers;
    }

    // Generate Suppliers
    generateSuppliers(count) {
        const suppliers = [];

        for (let i = 0; i < count; i++) {
            suppliers.push({
                id: `SUP-${i + 1}`,
                name: `${this.randomChoice(['Global', 'Premium', 'Quality', 'Best', 'Top'])} ${this.randomChoice(['Supplies', 'Products', 'Goods', 'Materials'])} Co.`,
                contactPerson: this.randomName(),
                email: `contact@supplier${i + 1}.com`,
                phone: this.randomPhone(),
                address: this.randomAddress(),
                city: this.randomCity(),
                country: 'USA',
                rating: this.randomNumber(3, 5, 1),
                totalOrders: this.randomNumber(10, 200),
                status: 'Active'
            });
        }

        return suppliers;
    }

    // Generate Inventory
    generateInventory(count) {
        const inventory = [];

        for (let i = 0; i < count; i++) {
            const stock = this.randomNumber(0, 500);
            const minStock = this.randomNumber(10, 50);
            inventory.push({
                id: `INV-${i + 1}`,
                productId: `PROD-${1000 + i}`,
                productName: `Product ${i + 1}`,
                warehouseId: `WH-${Math.floor(Math.random() * 5) + 1}`,
                stock: stock,
                minStock: minStock,
                maxStock: minStock * 10,
                status: stock < minStock ? 'Low Stock' : stock === 0 ? 'Out of Stock' : 'In Stock',
                lastRestocked: this.randomDate(new Date(2024, 0, 1), new Date()).toISOString()
            });
        }

        return inventory;
    }

    // Generate Shipments
    generateShipments(count) {
        const statuses = ['Pending', 'In Transit', 'Delivered', 'Returned'];
        const carriers = ['FedEx', 'UPS', 'DHL', 'USPS'];
        const shipments = [];

        for (let i = 0; i < count; i++) {
            shipments.push({
                id: `SHIP-${1000 + i}`,
                orderId: `ORD-${1000 + Math.floor(Math.random() * 100)}`,
                carrier: carriers[Math.floor(Math.random() * carriers.length)],
                trackingNumber: `TRK${Math.random().toString(36).substr(2, 12).toUpperCase()}`,
                status: statuses[Math.floor(Math.random() * statuses.length)],
                shipDate: this.randomDate(new Date(2024, 0, 1), new Date()).toISOString(),
                estimatedDelivery: this.randomDate(new Date(), new Date(2025, 11, 31)).toISOString(),
                weight: this.randomNumber(1, 50, 2),
                cost: this.randomNumber(10, 200)
            });
        }

        return shipments;
    }

    // Generate Payments
    generatePayments(count) {
        const methods = ['Credit Card', 'PayPal', 'Bank Transfer', 'Cash'];
        const statuses = ['Pending', 'Completed', 'Failed', 'Refunded'];
        const payments = [];

        for (let i = 0; i < count; i++) {
            payments.push({
                id: `PAY-${1000 + i}`,
                orderId: `ORD-${1000 + Math.floor(Math.random() * 100)}`,
                amount: this.randomNumber(100, 5000),
                method: methods[Math.floor(Math.random() * methods.length)],
                status: statuses[Math.floor(Math.random() * statuses.length)],
                date: this.randomDate(new Date(2024, 0, 1), new Date()).toISOString(),
                transactionId: `TXN${Math.random().toString(36).substr(2, 16).toUpperCase()}`
            });
        }

        return payments;
    }

    // Generate Warehouses
    generateWarehouses(count) {
        const warehouses = [];

        for (let i = 0; i < count; i++) {
            warehouses.push({
                id: `WH-${i + 1}`,
                name: `Warehouse ${this.randomChoice(['North', 'South', 'East', 'West', 'Central'])} ${i + 1}`,
                location: this.randomCity(),
                capacity: this.randomNumber(1000, 10000),
                currentStock: this.randomNumber(500, 8000),
                manager: this.randomName(),
                phone: this.randomPhone(),
                status: 'Active'
            });
        }

        return warehouses;
    }

    // Helper: Random number
    randomNumber(min, max, decimals = 0) {
        const num = Math.random() * (max - min) + min;
        return decimals > 0 ? parseFloat(num.toFixed(decimals)) : Math.floor(num);
    }

    // Helper: Random choice from array
    randomChoice(array) {
        return array[Math.floor(Math.random() * array.length)];
    }

    // Helper: Random date
    randomDate(start, end) {
        return new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));
    }

    // Helper: Random name
    randomName() {
        const firstNames = ['John', 'Jane', 'Michael', 'Sarah', 'David', 'Emily', 'Robert', 'Lisa', 'James', 'Mary'];
        const lastNames = ['Smith', 'Johnson', 'Williams', 'Brown', 'Jones', 'Garcia', 'Miller', 'Davis', 'Rodriguez', 'Martinez'];
        return `${this.randomChoice(firstNames)} ${this.randomChoice(lastNames)}`;
    }

    // Helper: Random phone
    randomPhone() {
        return `+1 (${this.randomNumber(200, 999)}) ${this.randomNumber(200, 999)}-${this.randomNumber(1000, 9999)}`;
    }

    // Helper: Random address
    randomAddress() {
        return `${this.randomNumber(100, 9999)} ${this.randomChoice(['Main', 'Oak', 'Maple', 'Cedar', 'Pine'])} ${this.randomChoice(['St', 'Ave', 'Blvd', 'Rd'])}`;
    }

    // Helper: Random city
    randomCity() {
        return this.randomChoice(['New York', 'Los Angeles', 'Chicago', 'Houston', 'Phoenix', 'Philadelphia', 'San Antonio', 'San Diego', 'Dallas', 'San Jose']);
    }

    // CRUD Operations
    create(entity, item) {
        const data = this.getAllData();
        item.id = item.id || `${entity.toUpperCase()}-${Date.now()}`;
        data[entity].push(item);
        localStorage.setItem(this.STORAGE_KEY, JSON.stringify(data));
        return item;
    }

    update(entity, id, updates) {
        const data = this.getAllData();
        const index = data[entity].findIndex(item => item.id === id);
        if (index !== -1) {
            data[entity][index] = { ...data[entity][index], ...updates };
            localStorage.setItem(this.STORAGE_KEY, JSON.stringify(data));
            return data[entity][index];
        }
        return null;
    }

    delete(entity, id) {
        const data = this.getAllData();
        data[entity] = data[entity].filter(item => item.id !== id);
        localStorage.setItem(this.STORAGE_KEY, JSON.stringify(data));
        return true;
    }

    // Reset all data
    reset() {
        localStorage.removeItem(this.STORAGE_KEY);
        return this.generateAllData();
    }
}

// Initialize mock data service
const mockDataService = new MockDataService();

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = MockDataService;
}
