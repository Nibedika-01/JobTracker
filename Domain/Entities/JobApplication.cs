namespace JobTracker.Domain.Entities;

using JobTracker.Domain.Enums;

public class JobApplication
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public required string Company { get; set; }
    public required string Role { get; set; }
    public string? JobUrl { get; set; }
    public string? Notes { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? InterviewAt { get; set; }
}