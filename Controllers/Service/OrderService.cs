using ManageCars.Models;
using ManageCars.Models.Request;
using ManageCars.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ManageCars.Controllers.Service
{
	public class OrderService
	{

		private readonly ILogger<OrderController> _logger;

		private readonly AppDbContext _context;



		public OrderService(ILogger<OrderController> logger, AppDbContext dbContext)
		{
			_logger = logger;
			_context = dbContext;
		}



		public bool CheckMeetingStatus(int Id)
		{


			return _context.Meeting.Any(c => c.OrderId == Id);
		}

		public Meeting GetMeeting(int Id)
		{
			return _context.Meeting.FirstOrDefault(c => c.OrderId == Id)!;
		}


		public async Task UpdateOrder(OrderUpdateModel orderModel)
		{



			var order = await _context.Orders.FirstOrDefaultAsync(c => c.Id == orderModel.id);

			Console.WriteLine(order.Id);
			if (orderModel.completionNote != null)
			{
				order.CompletionNote = orderModel.completionNote;
			}
			if (orderModel.assignedTo != null)
			{
				order.AssignedTo = orderModel.assignedTo;
			}

			if (orderModel.depositAmount != null)
			{
				order.DepositAmount = orderModel.depositAmount.Value;
			}

			if (orderModel.finalPrice != null)
			{
				order.FinalPrice = orderModel.finalPrice.Value;
			}

			if (orderModel.paymentMethod != null)
			{
				order.PaymentMethod = orderModel.paymentMethod;
			}

			order.Status = orderModel.status;

			await _context.SaveChangesAsync();

		}





		//public async Task<List<OrderViewModel>> getOrdersPage(PagingRequest pagingRequest)
		//{

		//	return await _context.Orders
		//		.OrderByDescending(o => o.Id) // hoặc o.Id
		//		.Select(o => new OrderViewModel
		//		{
		//			id = o.Id,
		//			customerName = o.CustomerName,
		//			//phoneNumber = o.PhoneNumber,
		//			//emaiil = o.Email,
		//			//message=o.Message,
		//			carId = o.CarId,
		//			quantity = o.quantity,
		//			lastmeeting = o.Meeting.OrderByDescending(m => m.StartTime)
		//									.FirstOrDefault(),
		//			createdDate = o.CreatedDate,
		//			status = o.Status

		//		})

		//  .Skip(pagingRequest._pageSize * (pagingRequest._pageNumber - 1))
		//		.Take(pagingRequest._pageSize)
		//		.ToListAsync();
		//	int totalItems = _context.Orders.Count();
		//	int totalPages = (int)Math.Ceiling((double)totalItems / pagingRequest._pageSize);

		//}

		public async Task<List<OrderViewModel>> getOrdersPage(PagingRequest pagingRequest)
		{
			var query = _context.Orders
				.Include(o => o.Meeting)
				.AsQueryable();

			// ===== FILTER STATUS =====
			if (!string.IsNullOrEmpty(pagingRequest.Status))
			{
				if (int.TryParse(pagingRequest.Status, out int statusInt))
				{
					query = query.Where(o => (int)o.Status == statusInt);
				}
			}

			// ===== SORT DATE =====
			if (pagingRequest.SortDate == "Newest")
			{
				query = query.OrderByDescending(o => o.CreatedDate);
			}
			else if (pagingRequest.SortDate == "Oldest")
			{
				query = query.OrderBy(o => o.CreatedDate);
			}
			else
			{
				query = query.OrderByDescending(o => o.Id);
			}

			return await query
				.Select(o => new OrderViewModel
				{
					id = o.Id,
					customerName = o.CustomerName,
					carId = o.CarId,
					quantity = o.quantity,
					lastmeeting = o.Meeting
						.OrderByDescending(m => m.StartTime)
						.FirstOrDefault(),
					createdDate = o.CreatedDate,
					status = o.Status
				})
				.Skip(pagingRequest._pageSize * (pagingRequest._pageNumber - 1))
				.Take(pagingRequest._pageSize)
				.ToListAsync();
		}


		public Task<int> CountOrder()
		{
			return _context.Orders.CountAsync();
		}





		public OrderViewModel? GetOrderWithCustomerInfor(int orderId)
		{

			return _context.Orders
				.Where(o => o.Id == orderId)
				.Select(o => new OrderViewModel
				{
					customerName = o.CustomerName,
					phoneNumber = o.PhoneNumber,
					email = o.Email,
					message = o.Message

				}).FirstOrDefault();



		}

	}
}
