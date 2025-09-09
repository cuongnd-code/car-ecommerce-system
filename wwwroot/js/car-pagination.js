

function pagePagin(currentPage, totalPage) {
	const carPagination = document.querySelector('.pagin');
	console.log("Total Pages:", totalPage);
	carPagination.innerHTML = '';
	for (let i = 1; i <= totalPage; i++) {
		carPagination.innerHTML += `
		<li class="pagecar page-item ${i == currentPage ? 'active' : ''}"><a class="page-link" href="/car/car-list?page=${i}" data-page="${i}"> ${i} </a></li>
							`;
	}





	document.querySelectorAll(".pagecar a").forEach(link => {
		link.addEventListener("click", e => {
			e.preventDefault();
			const page = link.dataset.page;
			console.log("trang hien tai " + page);
			CarList(page);
		})

	});
}