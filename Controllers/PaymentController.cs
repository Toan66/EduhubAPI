using EduhubAPI.Models;
using EduhubAPI.Models.Orders;
using EduhubAPI.Repositories;
using EduhubAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly IMomoService _momoService;

        public PaymentController(PaymentRepository paymentRepository, IMomoService momoService)
        {
            _paymentRepository = paymentRepository;
            _momoService = momoService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] OrderInfoModel model)
        {
            var paymentResponse = await _momoService.CreatePaymentAsync(model);
            if (paymentResponse != null && !string.IsNullOrEmpty(paymentResponse.PayUrl))
            {
                // Chuyển hướng người dùng đến trang thanh toán của Momo
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
                // Xử lý kết quả thanh toán, ví dụ: cập nhật trạng thái đơn hàng
                return Ok("Thanh toán thành công.");
            }
            return BadRequest("Thanh toán không thành công.");
        }


        [HttpPost("momoNotify")]
        public async Task<IActionResult> MomoNotify([FromBody] object request)
        {
            // Xử lý thông báo từ MoMo ở đây
            // Ví dụ: cập nhật trạng thái đơn hàng, lưu thông tin giao dịch, v.v.
            return Ok();
        }

        [HttpGet("PaymentCallBack")]
        public IActionResult PaymentCallBack([FromQuery] string resultCode, [FromQuery] string orderId)
        {
            // Xử lý kết quả trả về từ MoMo ở đây
            // Ví dụ: hiển thị thông báo thành công hoặc thất bại cho người dùng
            return RedirectToAction("Index", "Home"); // Chuyển hướng người dùng về trang chủ hoặc trang phù hợp
        }

    }
}
