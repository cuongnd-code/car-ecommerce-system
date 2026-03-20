

function updateUrlAndReload(page) {

	const url = new URL(window.location);

	url.searchParams.set("_pageNumber", page);

	window.history.pushState({}, "", url);

	const categoryId = url.searchParams.get("categoryId");

	CarShop(page, categoryId);
}

document.addEventListener('DOMContentLoaded', function () {
	console.log("shop"); // nên thấy dòng này nếu không có lỗi JS

	loadRecomandCar()

	CarShop(1,null); // Gọi hàm CarShop với trang đầu tiên




});

const IMAGE_BASE_URL = "/images/"; // Quản lý  ở đây

// khai bao table list

const template = document.querySelector('#car-template')

const table = document.querySelector('#car-list-table')


// khai bao table category
const categoryTemplate = document.querySelector('#car-category')
const categoryTab = document.querySelector('#tab-car-list-category ul')


function CarShop(page, categoryId) {

	let url = `/shop/car-list?_pageNumber=${page}&_pageSize=8`;

	if (categoryId) {
		url += `&categoryId=${categoryId}`;
	}
	fetch(url)
		.then(response => response.json())
		.then(data => {

			categoryTab.innerHTML = '';

			data.categories.forEach(category => {
				const categoryUnit = categoryTemplate.content.cloneNode(true);
				const button = categoryUnit.querySelector('button');

				button.textContent = category.name;
				button.value = category.id;

				button.addEventListener('click', function () {
					CarShop(1, this.value);
					console.log()
				});

				categoryTab.appendChild(categoryUnit);
			});

			table.innerHTML = '';

			data.cars.forEach(car => {
				const unit = template.content.cloneNode(true);

				unit.querySelector('.car-name').textContent = car.name;
				unit.querySelector('.car-type').textContent = car.type;
				unit.querySelector('.car-price').textContent = '$ ' + car.price + ' USD';
				unit.querySelector('.btnBuyCar').value = car.id;

				table.appendChild(unit);
			});

			document.querySelectorAll(".btnBuyCar").forEach(button => {
				button.addEventListener("click", function () {
					buyCar(this.value);
				});
			});


			createPagination2({
				containerSelector: '#pagination',
				infoSelector: '#pagination-info',
				currentPage: data.currentPage,
				pageSize: 8,
				totalItems: data.totalItem,
				onPageChange: (page) => {
					updateUrlAndReload(page);
				}
			});


		})
		.catch(error => console.error('Error fetching car data:', error));
}



function loadCarsByCategory(categoryId) {

	console.log('categore_tses')

	fetch('/shop/car-list-category', {
		method: 'post',
		headers: {
			'Content-Type': 'application/json'
		},
		body: JSON.stringify(categoryId)
	}).then(response => response.json())
		.then(data => {

			console.log(data);
			table.innerHTML = '';
			data.cars.forEach(car => {
				const unit = template.content.cloneNode(true);
				unit.querySelector('.car-name').textContent = car.name;
				unit.querySelector('.car-type').textContent = car.type;
				unit.querySelector('.car-price').textContent = '$ ' + car.price + ' USD';
				unit.querySelector('.btnBuyCar').value = car.id;
				table.appendChild(unit);

			});

		})
		.catch(error => console.error('Error fetching car data:', error));


}


//khai bao template recomanded
const recomandedTemplate = document.querySelector('#car-item-recomanded-template')

const recomandedtable = document.querySelector('#car-recomanded-table')

function loadRecomandCar() {

	fetch('/shop/car-recommend', {
		method: 'post',
		headers: {
			'Content-Type': 'application/json'
		}
	}).then(response => response.json())
		.then(data => {

			console.log(data);
			recomandedtable.innerHTML = '';
			if (data && Array.isArray(data.cars)) {
				data.cars.forEach((car,i) => {
					const unit = recomandedTemplate.content.cloneNode(true);
					const cardElement = unit.querySelector('.car-recomand-item'); 

					unit.querySelector('.car-img').src = IMAGE_BASE_URL + car.image;
					unit.querySelector('.car-year').textContent = car.year;

					unit.querySelector('.car-category').textContent = car.type;
					unit.querySelector('.car-type').textContent = car.type;

					unit.querySelector('.car-description').textContent = car.description;
					unit.querySelector('.car-name').textContent = car.name;
					unit.querySelector('.car-price').textContent = '$ ' + car.price + ' USD';
					unit.querySelector('.btnBuyCar').value = car.id;

					cardElement.dataset.index = i

					if (i == 0) {
						cardElement.classList.add('active');
					}

					recomandedtable.appendChild(unit);

				});

				addFunctionRecomand()



			} else {
				recomandedtable.innerHTML = '<p>No cars available at the moment.</p>';
			}

		})
		.catch(error => console.error('Error fetching car data:', error));


}



function addFunctionRecomand() {
	// thêm chức năng sang trái sang phải  cho đề xuất




	const carRecomandCard = document.querySelectorAll('.car-recomand-item')


	carRecomandCard[0].style.marginLeft = "80%";

	console.log(carRecomandCard)

	let indexCarRecomandCard = 0
	let nowCarPre
	let distance = 0
	carRecomandCard.forEach(card => {

		let nowIndex = Number(card.dataset.index);

		if (nowIndex == indexCarRecomandCard) {
			nowCarPre = card
		}

		card.addEventListener("click", function () {

			console.log(indexCarRecomandCard)
			console.log(nowIndex)
			if (nowIndex > indexCarRecomandCard) {
				distance = distance - 365
				card.classList.toggle('active');
				carRecomandCard.forEach(card => {
					card.style.transform = `translateX(${distance}px)`;

				});

				//
				nowCarPre.classList.toggle('active');
				indexCarRecomandCard = nowIndex
				nowCarPre = card
			}
			else if (nowIndex < indexCarRecomandCard) {
				distance = distance + 365
				card.classList.toggle('active');
				carRecomandCard.forEach(card => {
					card.style.transform = `translateX(${distance}px)`;

				});
				//
				nowCarPre.classList.toggle('active');
				indexCarRecomandCard = nowIndex
				nowCarPre = card
			}

		});
	});
}






function buyCar(carId) {

	window.location.href = `/car/Detail/${carId}`; // Chuyển hướng đến trang deposit với ID xe

}





	// Hiện đề xuất






	