using JobTracker.Application.DTOs;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using JobTracker.Domain.Interfaces;

namespace JobTracker.Application.Services;

public class  JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _repository;
    public JobApplicationService(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ApplicationDto>> GetAllAsync(
        string userId,
        ApplicationStatus? status = null)
    {
        var applications = await _repository.GetAllByUserAsync(userId);
        return applications
            .Where(a => status == null || a.Status == status)
            .OrderByDescending(a => a.AppliedAt)
            .Select(ToDto);
    }

    public async Task<ApplicationDto?> GetByIdAsync(int id, string userId)
    {
        var application = await _repository.GetByIdAsync(id, userId);
        return application == null ? null : ToDto(application);
    }

    public async Task<ApplicationDto> CreateAsync(
        CreateApplicationDto dto,
        string userId)
    {
        if(string.IsNullOrWhiteSpace(dto.Company))
            throw new ArgumentException("Company is required", nameof(dto.Company));
        if(string.IsNullOrWhiteSpace(dto.Role))
            throw new ArgumentException("Role is required", nameof(dto.Role));

        var entity = new JobApplication
        {
            UserId = userId,
            Company = dto.Company.Trim(),
            Role = dto.Role.Trim(),
            JobUrl = dto.JobUrl?.Trim(),
            Notes = dto.Notes?.Trim(),
            Status = ApplicationStatus.Applied,
            InterviewAt = dto.InterveiwAt,
            AppliedAt = DateTime.UtcNow
        };

        var saved = await _repository.AddAsync(entity);
        return ToDto(saved);
    }

    public async Task<ApplicationDto?> UpdateAsync(
        int id,
        UpdateApplicationDto dto,
        string userId)
    {
        var application = await _repository.GetByIdAsync(id, userId);
        if(application == null)
            return null;

        if (dto.Status != null) application.Status = dto.Status.Value;
        if (dto.Notes != null) application.Notes = dto.Notes;
        if (dto.InterveiwAt != null) application.InterviewAt = dto.InterveiwAt;
        application.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(application);
        return ToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        return await _repository.DeleteAsync(id, userId);
        
    }

    public async Task<StatsDto> GetStatsAsync(string userId)
    {
        var applications = (await _repository.GetAllByUserAsync(userId)).ToList();
        return new StatsDto
        {
            Total = applications.Count,
            Applied = applications.Count(a => a.Status == ApplicationStatus.Applied),
            Interviewing = applications.Count(a => a.Status == ApplicationStatus.Interviewing),
            Offered = applications.Count(a => a.Status == ApplicationStatus.Offered),
            Rejected = applications.Count(a => a.Status == ApplicationStatus.Rejected),
            Withdrawn = applications.Count(a => a.Status == ApplicationStatus.Withdrawn)
        };
    }
    public static ApplicationDto ToDto(JobApplication a) => new()
    {
        Id = a.Id,
        Company = a.Company,
        Role = a.Role,
        JobUrl = a.JobUrl,
        Notes = a.Notes,
        Status = a.Status,
        AppliedAt = a.AppliedAt,
        InterviewAt = a.InterviewAt,
        UpdatedAt = a.UpdatedAt
    };
}