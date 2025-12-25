
class TableUtilities {
    constructor() {
        this.sortDirection = {};
    }

    makeSortable(tableId) {
        const table = document.getElementById(tableId);
        if (!table) return;

        const headers = table.querySelectorAll('thead th');
        headers.forEach((header, index) => {
            if (header.classList.contains('no-sort')) return;

            header.style.cursor = 'pointer';
            const sortIcon = document.createElement('i');
            sortIcon.className = 'bi bi-arrow-down-up sort-icon';
            sortIcon.setAttribute('aria-hidden', 'true');
            header.appendChild(document.createTextNode(' '));
            header.appendChild(sortIcon);

            header.addEventListener('click', () => {
                this.sortTable(table, index);
            });
        });
    }

    sortTable(table, columnIndex) {
        const tbody = table.querySelector('tbody');
        const rows = Array.from(tbody.querySelectorAll('tr'));

        const key = `${table.id}_${columnIndex}`;
        const ascending = !this.sortDirection[key];
        this.sortDirection[key] = ascending;

        rows.sort((a, b) => {
            const aValue = a.cells[columnIndex]?.textContent.trim() || '';
            const bValue = b.cells[columnIndex]?.textContent.trim() || '';

            const aNum = parseFloat(aValue.replace(/[^0-9.-]/g, ''));
            const bNum = parseFloat(bValue.replace(/[^0-9.-]/g, ''));

            if (!isNaN(aNum) && !isNaN(bNum)) {
                return ascending ? aNum - bNum : bNum - aNum;
            }

            return ascending ?
                aValue.localeCompare(bValue) :
                bValue.localeCompare(aValue);
        });

        rows.forEach(row => tbody.appendChild(row));

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

    paginate(tableId, rowsPerPage = null) {
        if (rowsPerPage === null) {
            rowsPerPage = (typeof AppConfig !== 'undefined' && AppConfig.table) ? AppConfig.table.defaultRowsPerPage : 10;
        }
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

            controls.textContent = '';

            const infoText = document.createElement('div');
            infoText.className = 'text-muted';
            const start = ((currentPage - 1) * rowsPerPage) + 1;
            const end = Math.min(currentPage * rowsPerPage, rows.length);
            infoText.textContent = `Showing ${start} to ${end} of ${rows.length} entries`;

            const nav = document.createElement('nav');
            const ul = document.createElement('ul');
            ul.className = 'pagination mb-0';

            const prevLi = document.createElement('li');
            prevLi.className = `page-item ${currentPage === 1 ? 'disabled' : ''}`;
            const prevLink = document.createElement('a');
            prevLink.className = 'page-link';
            prevLink.href = '#';
            prevLink.setAttribute('data-page', 'prev');
            prevLink.setAttribute('aria-label', 'Previous page');
            prevLink.textContent = 'Previous';
            prevLi.appendChild(prevLink);
            ul.appendChild(prevLi);

            for (let page = 1; page <= totalPages; page++) {
                const li = document.createElement('li');
                li.className = `page-item ${page === currentPage ? 'active' : ''}`;
                const link = document.createElement('a');
                link.className = 'page-link';
                link.href = '#';
                link.setAttribute('data-page', page.toString());
                link.setAttribute('aria-label', `Page ${page}`);
                link.textContent = page.toString();
                li.appendChild(link);
                ul.appendChild(li);
            }

            const nextLi = document.createElement('li');
            nextLi.className = `page-item ${currentPage === totalPages ? 'disabled' : ''}`;
            const nextLink = document.createElement('a');
            nextLink.className = 'page-link';
            nextLink.href = '#';
            nextLink.setAttribute('data-page', 'next');
            nextLink.setAttribute('aria-label', 'Next page');
            nextLink.textContent = 'Next';
            nextLi.appendChild(nextLink);
            ul.appendChild(nextLi);

            nav.appendChild(ul);
            controls.appendChild(infoText);
            controls.appendChild(nav);

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

const tableUtils = new TableUtilities();

if (typeof module !== 'undefined' && module.exports) {
    module.exports = TableUtilities;
}
