namespace EduhubAPI.Dtos
{
  public class UserEnrollmentInfoDto
  {
    public int UserId { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public decimal? CompletionPercentage { get; set; }
  }
}