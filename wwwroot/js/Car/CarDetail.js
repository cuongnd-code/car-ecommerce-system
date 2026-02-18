
document.addEventListener('DOMContentLoaded', function () {
	console.log("order detail"); // nên thấy dòng này nếu không có lỗi JS
	CreateOrder()
});





function CreateOrder() {
	document.getElementById('customer-order-form').addEventListener('submit', function (e) {
		e.preventDefault()

		const formData = new FormData(this);



		Swal.fire({
			title: "Do you want to Buy this Car?",
			showCancelButton: true,
			confirmButtonText: "Yes",
		}).then((result) => {
			if (result.isConfirmed) {

				fetch('/order/create', {
					method: 'post',
					body: formData

				}).then(response => response.json())
					.then(a => {

						Swal.fire({
							title: "Good job!",
							text: a.message,
							icon: "success"
						});

					})
					.catch(error => {
						console.error('Error fetching user details:', error);
					});

			} 
		});





		
	})


	
}



