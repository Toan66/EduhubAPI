namespace EduhubAPI.Dtos
{
  public class CreatePaymentDto
  {
    public string OrderId { get; set; }
    public string TransactionId { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Status { get; set; }
  }
}