using EduhubAPI.Models;
using EduhubAPI.Models.Orders;
using EduhubAPI.Repositories;
using EduhubAPI.Services;
using Microsoft.AspNetCore.Http;
using EduhubAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using EduhubAPI.Dtos;
using System;

namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly IMomoService _momoService;
        private readonly JwtService _jwtService;

        public PaymentController(PaymentRepository paymentRepository, IMomoService momoService, JwtService jwtService)
        {
            _paymentRepository = paymentRepository;
            _momoService = momoService;
            _jwtService = jwtService;
        }

        [HttpPost("addOrder")]
        public IActionResult AddOrder([FromBody] CreateOrderDto orderDto)
        {
            Random random = new Random();
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized();
                }
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);


                var order = new Order
                {
                    OrderId = DateTime.UtcNow.Ticks.ToString(),
                    UserId = userId,
                    CourseId = orderDto.CourseId,
                    Amount = orderDto.Amount,
                    OrderDate = DateTime.Now,
                    Status = orderDto.Status,
                };

                return Ok(_paymentRepository.AddOrder(order));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("addPayment")]
        public IActionResult AddPayment([FromBody] CreatePaymentDto paymentDto)
        {
            var payment = new Payment
            {
                OrderId = paymentDto.OrderId,
                TransactionId = paymentDto.TransactionId,
                PaymentDate = DateTime.Now,
                Amount = paymentDto.Amount,
                Status = paymentDto.Status,
            };

            _paymentRepository.AddPayment(payment);
            return Ok(payment);
        }

        [HttpGet("ByUser")]
        public IActionResult GetOrdersByUser()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized("Don't have token.");
                }

                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);

                var orders = _paymentRepository.GetOrdersByUserId(userId);
                if (orders == null || !orders.Any())
                {
                    return NotFound("Don't have any order.");
                }

                return Ok(orders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("order/{id}")]
        public IActionResult GetOrderByID(string id)
        {
            var order = _paymentRepository.GetOrderByID(id);
            return Ok(order);
        }



        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] OrderInfoModel model)
        {
            var paymentResponse = await _momoService.CreatePaymentAsync(model);
            if (paymentResponse != null && !string.IsNullOrEmpty(paymentResponse.PayUrl))
            {
                return Ok(new { url = paymentResponse.PayUrl });
            }
            return BadRequest("Không thể tạo thanh toán.");
        }

        [HttpPost("callback")]
        public IActionResult PaymentCallback([FromQuery] IQueryCollection query)
        {
            var paymentResult = _momoService.PaymentExecuteAsync(query);
            if (paymentResult != null)
            {
                var order = _paymentRepository.GetOrderByID(paymentResult.OrderId);
                order.Status = "Paid";
                _paymentRepository.UpdateOrderStatusAsync(order.OrderId, order.Status);
                return Ok("Thanh toán thành công.");
            }
            return BadRequest("Thanh toán không thành công.");
        }


        [HttpPost("order/{id}/paid")]
        public IActionResult UpdateOrderPaid(string id)
        {
            var order = _paymentRepository.GetOrderByID(id);
            if (order != null)
            {
                order.Status = "Paid";
                _paymentRepository.UpdateOrder(order);
                return Ok();
            }
            else return BadRequest();
        }

        [HttpGet("PaymentCallBack")]
        public IActionResult PaymentCallBack([FromQuery] string resultCode, [FromQuery] string orderId)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("completedOrdersByTeacherCourses")]
        public IActionResult GetCompletedOrdersByTeacherCourses()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized("Don't have token.");
                }

                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);

                var completedOrders = _paymentRepository.GetCompletedOrdersByTeacherId(userId);

                if (completedOrders == null || !completedOrders.Any())
                {
                    return NotFound("No completed orders found.");
                }

                return Ok(completedOrders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("paidOrders")]
        public IActionResult GetPaidOrders()
        {
            try
            {
                var paidOrders = _paymentRepository.GetPaidOrders();

                if (paidOrders == null || !paidOrders.Any())
                {
                    return NotFound("No paid orders found.");
                }

                return Ok(paidOrders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
