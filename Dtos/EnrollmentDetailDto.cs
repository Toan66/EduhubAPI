using EduhubAPI.Models;

namespace EduhubAPI.Dtos
{
  public class EnrollmentDetailDto
  {
    public int CourseId { get; set; }
    public int EnrollmentId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public decimal? CompletionPercentage { get; set; }
    public Course Course { get; set; }
  }
}