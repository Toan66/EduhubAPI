using EduhubAPI.Models.Momo;
using EduhubAPI.Models.Orders;


namespace EduhubAPI.Services
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
