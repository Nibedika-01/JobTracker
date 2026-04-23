using JobTracker.Domain.Entities;

namespace JobTracker.Domain.Interfaces;

public interface IJobApplicationRepository
{
    Task<IEnumerable<JobApplication>> GetAllByUserAsync(string userId);
    Task<JobApplication?> GetByIdAsync(int id, string userId);
    Task<JobApplication> AddAsync(JobApplication application);
    Task<JobApplication> UpdateAsync(JobApplication application);
    Task<bool> DeleteAsync(int id, string userId);
}