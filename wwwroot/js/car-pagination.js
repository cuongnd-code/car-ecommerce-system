function createPagination({
    containerSelector,
    infoSelector,
    currentPage,
    pageSize,
    totalItems,
    onPageChange
}) {
    const container = document.querySelector(containerSelector);
    const info = document.querySelector(infoSelector);

    if (!container) return;

    const totalPage = Math.ceil(totalItems / pageSize);

    /* ===== Showing 1–9 of 78 ===== */
    const start = (currentPage - 1) * pageSize + 1;
    const end = Math.min(currentPage * pageSize, totalItems);
    info.textContent = `Showing ${start}–${end} of ${totalItems}`;

    /* ===== Clear old page numbers ===== */
    container.querySelectorAll('.page-number').forEach(e => e.remove());

    /* ===== Page numbers ===== */
    for (let i = 1; i <= totalPage; i++) {
        const li = document.createElement('li');
        li.className = `page-item page-number ${i === currentPage ? 'active' : ''}`;
        li.innerHTML = `<a class="page-link" href="#">${i}</a>`;

        li.onclick = e => {
            e.preventDefault();
            onPageChange(i);
        };

        container.insertBefore(li, container.querySelector('.next').parentElement);
    }

    /* ===== Prev / Next ===== */
    container.querySelector('.prev').onclick = e => {
        e.preventDefault();
        if (currentPage > 1) onPageChange(currentPage - 1);
    };

    container.querySelector('.next').onclick = e => {
        e.preventDefault();
        if (currentPage < totalPage) onPageChange(currentPage + 1);
    };
}



function createPagination2({
    containerSelector,
    infoSelector,
    currentPage,
    pageSize,
    totalItems,
    onPageChange
}) {
    const container = document.querySelector(containerSelector);
    const info = document.querySelector(infoSelector);

    if (!container) return;

    const totalPage = Math.ceil(totalItems / pageSize);

    const start = (currentPage - 1) * pageSize + 1;
    const end = Math.min(currentPage * pageSize, totalItems);
    info.textContent = `Showing ${start}–${end} of ${totalItems}`;

    /* ===== Clear old page numbers ===== */
    container.querySelectorAll('.page-number').forEach(e => e.remove());

    for (let i = 1; i <= totalPage; i++) {
        const li = document.createElement('li');
        li.className = `page-item page-number ${i === currentPage ? 'active' : ''}`;

        li.innerHTML = `<a class="page-link" href="#">${i}</a>`;

        li.onclick = e => {
            e.preventDefault();
            onPageChange(i);
        };

        container.insertBefore(
            li,
            container.querySelector('.next').parentElement
        );
    }

    container.querySelector('.prev').addEventListener("click", e => {
        e.preventDefault();
        if (currentPage > 1) onPageChange(currentPage - 1);
    });

    container.querySelector('.next').addEventListener("click", e => {
        e.preventDefault();
        if (currentPage < totalPage) onPageChange(currentPage + 1);
    });
}
