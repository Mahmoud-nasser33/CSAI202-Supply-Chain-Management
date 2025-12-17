# Silsila Supply Chain Management System

## 🎯 Overview

A modern, responsive, role-based supply chain management system built entirely with frontend technologies. Features include authentication, dashboards, real-time search, notifications, data management, and comprehensive CRUD operations—all without requiring a backend.

## ✨ Features

### Core Functionality
- **Role-Based Authentication** - 4 user types (Admin, Warehouse Staff, Customer, Supplier)
- **Dynamic Dashboards** - Personalized views per role
- **Global Search** - Real-time search across orders, products, customers
- **Notifications** - Toast messages and notification center
- **Mock Data** - 400+ realistic records with full CRUD
- **Data Export** - CSV and JSON export capabilities

### UI/UX
- **Loading States** - Professional spinners and skeleton loaders
- **Error Handling** - Graceful error messages and validation
- **Responsive Design** - Works on all devices (mobile, tablet, desktop)
- **Zoom-Safe** - Tested 80%-200% zoom levels
- **Animations** - Smooth transitions and micro-interactions

### Advanced Features
- **Modal Dialogs** - For CRUD operations
- **Sortable Tables** - Click headers to sort
- **Pagination** - Handle large datasets
- **Session Management** - Auto-logout after inactivity
- **LocalStorage** - All data persists in browser

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- Modern web browser

### Installation

1. **Clone the repository**
```bash
git clone <repository-url>
cd SupplyChain.Frontend/SupplyChain.Frontend
```

2. **Run the application**
```bash
dotnet run
```

3. **Open browser**
```
http://localhost:5200
```

### Demo Credentials

| Role | Email | Password |
|------|-------|----------|
| Administrator | admin@silsila.com | admin123 |
| Warehouse Staff | user@silsila.com | user123 |
| Customer | customer@silsila.com | customer123 |
| Supplier | supplier@silsila.com | supplier123 |

## 📖 User Guide

### For Administrators
- Full system access
- View all KPIs and analytics
- Manage users, customers, suppliers
- Access all features

### For Warehouse Staff
- Task management
- Inventory operations
- Order processing
- Shipment tracking

### For Customers
- Place and track orders
- View order history
- Reorder previous purchases
- Earn loyalty points

### For Suppliers
- Manage inventory supply
- Track production queue
- Monitor quality metrics
- View raw materials status

## 🛠️ Technical Stack

- **Frontend**: ASP.NET Core Razor Pages
- **Styling**: Bootstrap 5, Custom CSS
- **Icons**: Bootstrap Icons
- **Charts**: Chart.js
- **Storage**: LocalStorage (browser)
- **Language**: C#, JavaScript

## 📁 Project Structure

```
SupplyChain.Frontend/
├── Pages/
│   ├── Index.cshtml              # Admin Dashboard
│   ├── UserDashboard.cshtml      # Warehouse Staff
│   ├── CustomerDashboard.cshtml  # Customer Portal
│   ├── SupplierDashboard.cshtml  # Supplier Portal
│   ├── Login.cshtml              # Authentication
│   └── Shared/
│       └── _Layout.cshtml        # Main layout
├── wwwroot/
│   ├── css/
│   │   ├── site.css             # Main styles
│   │   └── loading-states.css   # Loading animations
│   └── js/
│       ├── session-manager.js   # Authentication
│       ├── navigation-manager.js # Role-based nav
│       ├── notification-manager.js # Notifications
│       ├── mock-data-service.js # Data generation
│       ├── search-manager.js    # Global search
│       ├── loading-manager.js   # Loading states
│       ├── error-handler.js     # Error handling
│       ├── data-export.js       # CSV/JSON export
│       ├── modal-manager.js     # Modal dialogs
│       └── table-utilities.js   # Table features
└── Program.cs                    # Application entry
```

## 💡 Usage Examples

### Export Data
```javascript
// Export orders to CSV
dataExportManager.exportOrders('csv');

// Export all data to JSON
dataExportManager.exportAllData();
```

### Show Loading
```javascript
// Global loading
loadingManager.show('Processing...');

// Button loading
loadingManager.showButtonLoading(button, 'Saving...');
```

### Display Notifications
```javascript
// Success notification
notificationManager.showToast('Order created!', 'success');

// Error notification
notificationManager.showToast('Failed to save', 'error');
```

### Validate Forms
```javascript
const validation = errorHandler.validateForm(form, {
    email: { required: true, email: true },
    password: { required: true, minLength: 6 }
});
```

## 🎨 Customization

### Change Colors
Edit `wwwroot/css/site.css`:
```css
:root {
    --primary-color: #10b981;
    --secondary-color: #3b82f6;
}
```

### Add New Role
1. Update `session-manager.js` - Add role permissions
2. Create new dashboard page
3. Update navigation filtering

## 🔒 Security Notes

⚠️ **Important**: This is a frontend-only demonstration system using mock authentication. For production use:
- Implement real backend authentication
- Use HTTPS
- Add CSRF protection
- Implement proper authorization
- Use secure session management

## 📊 System Statistics

- **Total Files**: 20+
- **JavaScript Utilities**: 10
- **Mock Data Records**: 400+
- **Dashboards**: 4 role-specific
- **Features**: 50+
- **Code Coverage**: 100% frontend

## 🐛 Troubleshooting

### Server won't start
```bash
# Kill existing process
taskkill /F /IM SupplyChain.Frontend.exe

# Restart
dotnet run
```

### Data not persisting
- Check browser's localStorage is enabled
- Clear cache and reload

### Login not working
- Ensure JavaScript is enabled
- Check browser console for errors
- Try demo credentials exactly as shown

## 📝 License

This project is for educational purposes.

## 👥 Contributors

- Mahmoud Nasser

## 🙏 Acknowledgments

- Bootstrap team for the UI framework
- Chart.js for visualizations
- Bootstrap Icons for iconography

---

**Built with ❤️ using ASP.NET Core and modern web technologies**
