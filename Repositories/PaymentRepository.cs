using EduhubAPI.Models;
using System.Threading.Tasks;
using EduhubAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace EduhubAPI.Repositories
{
    public class PaymentRepository
    {
        private readonly EDUHUBContext _context;
        public PaymentRepository(EDUHUBContext context)
        {
            _context = context;
        }

        public Payment? GetPaymentByID(int id)
        {
            return _context.Payments.FirstOrDefault(u => u.PaymentId == id);
        }

        public Order? GetOrderByID(string id)
        {
            return _context.Orders.FirstOrDefault(u => u.OrderId == id);
        }


        public Order AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        public Payment AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
            return payment;
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderStatusAsync(string orderId, string status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
            return order;
        }

        public IEnumerable<Order> GetOrdersByUserId(int id)
        {
            return _context.Orders.Where(c => c.UserId == id).ToList();
        }

        public Order UpdateOrder(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
            return order;
        }

        public IEnumerable<Order> GetCompletedOrdersByTeacherId(int teacherId)
        {
            var courseIds = _context.Courses
                .Where(c => c.TeacherId == teacherId)
                .Select(c => c.CourseId)
                .ToList();

            return _context.Orders
                .Where(o => courseIds.Contains(o.CourseId) && o.Status == "Paid")
                .ToList();
        }

        public IEnumerable<Order> GetPaidOrders()
        {
            return _context.Orders.Where(o => o.Status == "Paid").ToList();
        }

    }
}