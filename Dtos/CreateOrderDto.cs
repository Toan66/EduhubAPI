namespace EduhubAPI.Dtos
{
  public class CreateOrderDto
  {
    public int CourseId { get; set; }
    public decimal Amount { get; set; }
    public string? Status { get; set; }
  }
}