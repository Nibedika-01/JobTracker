using JobTracker.Application.DTOs;
using JobTracker.Domain.Enums;

namespace JobTracker.Application.Interfaces;

public interface IJobApplicationService
{
    Task<IEnumerable<ApplicationDto>> GetAllAsync(string userId, ApplicationStatus? status = null); //returns all with no filters all status
    Task<ApplicationDto?> GetByIdAsync(int id, string userId);
    Task<ApplicationDto> CreateAsync(CreateApplicationDto dto, string userId);
    Task<ApplicationDto?> UpdateAsync(int id, UpdateApplicationDto dto, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<StatsDto> GetStatsAsync(string userId);
}