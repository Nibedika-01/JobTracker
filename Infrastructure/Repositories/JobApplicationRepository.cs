using JobTracker.Domain.Entities;
using JobTracker.Domain.Interfaces;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Repositories;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly AppDbContext _db;
    public JobApplicationRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<JobApplication>> GetAllByUserAsync(string userId)
    {
        return await _db.JobApplications.Where(a => a.UserId == userId) //gets all the application of a loggedin user
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync();
    }

    public async Task<JobApplication?> GetByIdAsync(int id, string userId)
    {
        return await _db.JobApplications.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId); //returns application that matches the app id and also logged in user id
    }

    public async Task<JobApplication> AddAsync(JobApplication application)
    {
        _db.JobApplications.Add(application);
        await _db.SaveChangesAsync();
        return application;
    }

    public async Task<JobApplication> UpdateAsync(JobApplication application)
    {
        _db.JobApplications.Update(application);
        await _db.SaveChangesAsync();
        return application;
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var app = await GetByIdAsync(id, userId);
        if(app == null) return false;
        _db.JobApplications.Remove(app);
        await _db.SaveChangesAsync();
        return true;
    }
}