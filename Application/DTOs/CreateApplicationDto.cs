using JobTracker.Domain.Enums;

namespace JobTracker.Application.DTOs;

public class CreateApplicationDto
{
    public required string Company { get; set; }
    public required string Role { get; set; }
    public string? JobUrl { get; set; }
    public string? Notes { get; set; }
    public DateTime? InterveiwAt { get; set; }
}

public class UpdateApplicationDto
{
    public ApplicationStatus? Status { get; set; }
    public string? Notes { get; set; }
    public DateTime? InterveiwAt { get; set; }
}

public class ApplicationDto
{
    public int Id { get; set; }
    public string Company { get; set; } = "";
    public string Role { get; set; } = "";
    public string? JobUrl { get; set; }
    public string? Notes { get; set; }
    public ApplicationStatus Status { get; set; }
    public DateTime AppliedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? InterviewAt { get; set; }
}

public class StatsDto
{
    public int Total { get; set; }
    public int Applied { get; set; }
    public int Interviewing { get; set; }
    public int Offered { get; set; }
    public int Rejected { get; set; }
    public int Withdrawn { get; set; }
}