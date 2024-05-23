namespace EduhubAPI.Dtos
{
  public class UpdateUserPasswordDto
  {
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
  }
}