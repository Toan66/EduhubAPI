using EduhubAPI.Models;
using System.Threading.Tasks;
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

        public async Task<Order> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
            return order;
        }
    }
}