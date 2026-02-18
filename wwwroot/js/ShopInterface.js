


document.addEventListener('DOMContentLoaded', function () {
	console.log("shop"); // nên thấy dòng này nếu không có lỗi JS

	loadRecomandCar()

	CarShop(1); // Gọi hàm CarShop với trang đầu tiên




});

const IMAGE_BASE_URL = "/images/"; // Quản lý  ở đây

// khai bao table list

const template = document.querySelector('#car-template')

const table = document.querySelector('#car-list-table')


// khai bao table category
const categoryTemplate = document.querySelector('#car-category')
const categoryTab = document.querySelector('#tab-car-list-category ul')


function CarShop(page) {

	fetch('/shop/car-list', {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json'
		},
		body: JSON.stringify({ _pageNumber: page, _pageSize: 6 })
	})
		.then(response => response.json())
		.then(data => {




			// display car category nav



			categoryTab.innerHTML = '';
			data.categories.forEach(category => {
				const categoryUnit = categoryTemplate.content.cloneNode(true);

				const button = categoryUnit.querySelector('button');

				button.textContent = category.name;
				button.value = category.id;


				// them nut category
				button.addEventListener('click', function () {
					loadCarsByCategory(this.value)
					console.log(this.value)
				})


				categoryTab.appendChild(categoryUnit);
			});

			// display car list all

			table.innerHTML = '';
			data.cars.forEach(car => {
				const unit = template.content.cloneNode(true);
				unit.querySelector('.car-name').textContent = car.name;
				unit.querySelector('.car-type').textContent = car.type;
				unit.querySelector('.car-price').textContent = '$ ' + car.price + ' USD';
				unit.querySelector('.btnBuyCar').value = car.id;

				table.appendChild(unit);

			});



			//	(data.currentPage, data.totalPage);


			document.querySelectorAll(".btnBuyCar").forEach(button => {
				button.addEventListener("click", function () {
					buyCar(this.value)
				})

			})
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

	window.location.href = `/shop/car/${carId}`; // Chuyển hướng đến trang deposit với ID xe

}





	// Hiện đề xuất






	