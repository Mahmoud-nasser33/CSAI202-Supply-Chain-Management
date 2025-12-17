// Table Utilities - Sorting, filtering, pagination
class TableUtilities {
    constructor() {
        this.sortDirection = {};
    }

    // Make table sortable
    makeSortable(tableId) {
        const table = document.getElementById(tableId);
        if (!table) return;

        const headers = table.querySelectorAll('thead th');
        headers.forEach((header, index) => {
            if (header.classList.contains('no-sort')) return;

            header.style.cursor = 'pointer';
            header.innerHTML += ' <i class="bi bi-arrow-down-up sort-icon"></i>';

            header.addEventListener('click', () => {
                this.sortTable(table, index);
            });
        });
    }

    // Sort table
    sortTable(table, columnIndex) {
        const tbody = table.querySelector('tbody');
        const rows = Array.from(tbody.querySelectorAll('tr'));

        const key = `${table.id}_${columnIndex}`;
        const ascending = !this.sortDirection[key];
        this.sortDirection[key] = ascending;

        rows.sort((a, b) => {
            const aValue = a.cells[columnIndex]?.textContent.trim() || '';
            const bValue = b.cells[columnIndex]?.textContent.trim() || '';

            // Try numeric comparison
            const aNum = parseFloat(aValue.replace(/[^0-9.-]/g, ''));
            const bNum = parseFloat(bValue.replace(/[^0-9.-]/g, ''));

            if (!isNaN(aNum) && !isNaN(bNum)) {
                return ascending ? aNum - bNum : bNum - aNum;
            }

            // String comparison
            return ascending ?
                aValue.localeCompare(bValue) :
                bValue.localeCompare(aValue);
        });

        // Update table
        rows.forEach(row => tbody.appendChild(row));

        // Update sort icons
        const headers = table.querySelectorAll('thead th');
        headers.forEach((h, i) => {
            const icon = h.querySelector('.sort-icon');
            if (icon) {
                if (i === columnIndex) {
                    icon.className = ascending ? 'bi bi-arrow-up' : 'bi bi-arrow-down';
                } else {
                    icon.className = 'bi bi-arrow-down-up sort-icon';
                }
            }
        });
    }

    // Add pagination
    paginate(tableId, rowsPerPage = 10) {
        const table = document.getElementById(tableId);
        if (!table) return;

        const tbody = table.querySelector('tbody');
        const rows = Array.from(tbody.querySelectorAll('tr'));
        const totalPages = Math.ceil(rows.length / rowsPerPage);
        let currentPage = 1;

        const showPage = (page) => {
            const start = (page - 1) * rowsPerPage;
            const end = start + rowsPerPage;

            rows.forEach((row, index) => {
                row.style.display = (index >= start && index < end) ? '' : 'none';
            });

            updatePaginationControls();
        };

        const updatePaginationControls = () => {
            let controls = table.parentElement.querySelector('.pagination-controls');
            if (!controls) {
                controls = document.createElement('div');
                controls.className = 'pagination-controls d-flex justify-content-between align-items-center mt-3';
                table.parentElement.appendChild(controls);
            }

            controls.innerHTML = `
                <div class="text-muted">
                    Showing ${((currentPage - 1) * rowsPerPage) + 1} to ${Math.min(currentPage * rowsPerPage, rows.length)} of ${rows.length} entries
                </div>
                <nav>
                    <ul class="pagination mb-0">
                        <li class="page-item ${currentPage === 1 ? 'disabled' : ''}">
                            <a class="page-link" href="#" data-page="prev">Previous</a>
                        </li>
                        ${Array.from({ length: totalPages }, (_, i) => i + 1).map(page => `
                            <li class="page-item ${page === currentPage ? 'active' : ''}">
                                <a class="page-link" href="#" data-page="${page}">${page}</a>
                            </li>
                        `).join('')}
                        <li class="page-item ${currentPage === totalPages ? 'disabled' : ''}">
                            <a class="page-link" href="#" data-page="next">Next</a>
                        </li>
                    </ul>
                </nav>
            `;

            // Add click handlers
            controls.querySelectorAll('.page-link').forEach(link => {
                link.addEventListener('click', (e) => {
                    e.preventDefault();
                    const page = e.target.dataset.page;

                    if (page === 'prev' && currentPage > 1) {
                        currentPage--;
                    } else if (page === 'next' && currentPage < totalPages) {
                        currentPage++;
                    } else if (!isNaN(page)) {
                        currentPage = parseInt(page);
                    }

                    showPage(currentPage);
                });
            });
        };

        showPage(1);
    }

    // Add search/filter
    addFilter(tableId, searchInputId) {
        const table = document.getElementById(tableId);
        const searchInput = document.getElementById(searchInputId);

        if (!table || !searchInput) return;

        searchInput.addEventListener('input', (e) => {
            const filter = e.target.value.toLowerCase();
            const rows = table.querySelectorAll('tbody tr');

            rows.forEach(row => {
                const text = row.textContent.toLowerCase();
                row.style.display = text.includes(filter) ? '' : 'none';
            });
        });
    }
}

// Initialize table utilities
const tableUtils = new TableUtilities();

// Export
if (typeof module !== 'undefined' && module.exports) {
    module.exports = TableUtilities;
}
