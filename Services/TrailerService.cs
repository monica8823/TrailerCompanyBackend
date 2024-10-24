using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrailerCompanyBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace TrailerCompanyBackend.Services
{
    public class TrailerService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<TrailerService> _logger;

        public TrailerService(TrailerCompanyDbContext context, ILogger<TrailerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Method to create trailers based on inputted model names
        public async Task<bool> CreateTrailersAsync(List<string> modelNames, int storeId, string? customFields = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var modelName in modelNames)
                {
                    var trailer = new Trailer
                    {
                        ModelName = modelName,
                        StoreId = storeId,
                        Vin = null, // Fields are initially empty
                        Size = null,
                        RatedCapacity = 0.0, // Use a default value for non-nullable field
                        CurrentStatus = "Not Stocked", // Default status
                        CustomFields = customFields // Add custom fields if provided
                    };

                    _context.Trailers.Add(trailer);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();  // Commit transaction
                _logger.LogInformation("Trailers created successfully for store ID: {StoreId}", storeId);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();  // Rollback transaction in case of error
                _logger.LogError(ex, "Error occurred while creating trailers for store ID: {StoreId}", storeId);
                return false;
            }
        }   



        // Method to get all trailers by store and name
        public async Task<List<Trailer>> GetAllTrailersAsync(int storeId, string? modelName = null)
        {
            var query = _context.Trailers.Where(t => t.StoreId == storeId);
            
            if (!string.IsNullOrEmpty(modelName))
            {
                query = query.Where(t => t.ModelName == modelName);
            }

            return await query.ToListAsync();
        }


        // Method to edit specific details of a trailer with VIN uniqueness check
       public async Task<bool> EditTrailerDetailsAsync(int trailerId, string? modelName, string? vin, string? size, double? ratedCapacity, string? currentStatus, string? customFields)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var trailer = await _context.Trailers.FindAsync(trailerId);
                if (trailer == null)
                {
                    _logger.LogWarning("Trailer with ID: {TrailerId} not found.", trailerId);
                    return false;
                }

                // If VIN is being updated, check its uniqueness
                if (!string.IsNullOrEmpty(vin) && vin != trailer.Vin)
                {
                    var existingTrailer = await _context.Trailers.FirstOrDefaultAsync(t => t.Vin == vin);
                    if (existingTrailer != null)
                    {
                        _logger.LogWarning("A trailer with VIN {VIN} already exists. Update aborted.", vin);
                        return false;
                    }
                }

                // Update each field only if the new value is provided
                trailer.ModelName = modelName ?? trailer.ModelName;
                trailer.Vin = vin ?? trailer.Vin;
                trailer.Size = size ?? trailer.Size;
                trailer.RatedCapacity = ratedCapacity ?? trailer.RatedCapacity;
                trailer.CurrentStatus = currentStatus ?? trailer.CurrentStatus;
                trailer.CustomFields = customFields ?? trailer.CustomFields; // Update custom fields if provided

                _context.Trailers.Update(trailer);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();  // Commit transaction
                _logger.LogInformation("Trailer details with ID: {TrailerId} updated successfully.", trailerId);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();  // Rollback transaction in case of error
                _logger.LogError(ex, "Error occurred while updating trailer details with ID: {TrailerId}", trailerId);
                return false;
            }
        }



        // Method to delete a trailer
        public async Task<bool> DeleteTrailerAsync(int trailerId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var trailer = await _context.Trailers.FindAsync(trailerId);
                if (trailer == null)
                {
                    _logger.LogWarning("Trailer with ID: {TrailerId} not found.", trailerId);
                    return false;
                }

                _context.Trailers.Remove(trailer);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();  // Commit transaction
                _logger.LogInformation("Trailer with ID: {TrailerId} deleted successfully.", trailerId);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();  // Rollback transaction in case of error
                _logger.LogError(ex, "Error occurred while deleting trailer with ID: {TrailerId}", trailerId);
                return false;
            }
        }



        // Batch method to delete multiple trailers with transaction management
        public async Task<bool> BatchDeleteTrailersAsync(List<int> trailerIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var trailerId in trailerIds)
                {
                    var trailer = await _context.Trailers.FindAsync(trailerId);
                    if (trailer == null)
                    {
                        _logger.LogWarning("Trailer with ID: {TrailerId} not found.", trailerId);
                        return false;  // Return false or handle it based on your requirements
                    }

                    _context.Trailers.Remove(trailer);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();  // Commit the transaction after all deletes
                _logger.LogInformation("Batch deletion of trailers successful.");
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();  // Rollback the transaction in case of an error
                _logger.LogError(ex, "Error occurred during batch deletion of trailers.");
                return false;
            }
        }


        // Method to get logs for a specific trailer
        public async Task<List<OperationLog>> GetTrailerLogsAsync(int trailerId)
        {
            try
            {
                var logs = await _context.OperationLogs
                                        .Where(log => log.EntityId == trailerId && log.EntityType == "Trailer")
                                        .ToListAsync();
                _logger.LogInformation("Logs for trailer ID: {TrailerId} retrieved successfully.", trailerId);
                return logs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving logs for trailer with ID: {TrailerId}", trailerId);
                throw;
            }
        }


        // Method to get a trailer by its ID
        public async Task<Trailer?> GetTrailerByIdAsync(int trailerId)
        {
            try
            {
                var trailer = await _context.Trailers.FindAsync(trailerId);
                if (trailer == null)
                {
                    _logger.LogWarning("Trailer with ID: {TrailerId} not found.", trailerId);
                    return null;
                }

                _logger.LogInformation("Trailer with ID: {TrailerId} retrieved successfully.", trailerId);
                return trailer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving trailer with ID: {TrailerId}", trailerId);
                throw;
            }
        }

        // Method to search trailers globally across stores by VIN or InvoiceNumber
     
        public async Task<List<Trailer>> GlobalSearchTrailersAsync(string? vin, string? invNumber)
        {
            try
            {
                IQueryable<Trailer> query = _context.Trailers.AsQueryable();

                if (!string.IsNullOrEmpty(vin))
                {
                    query = query.Where(t => t.Vin == vin);
                }

                if (!string.IsNullOrEmpty(invNumber))
                {
                    query = query.Where(t => t.SalesRecords.Any(sr => sr.InvNumber == invNumber));
                }

                var trailers = await query.ToListAsync();

                if (trailers.Count == 0)
                {
                    _logger.LogWarning("No trailers found for VIN: {VIN} or InvNumber: {InvNumber}", vin, invNumber);
                }
                else
                {
                    _logger.LogInformation("Trailers found for VIN: {VIN} or InvNumber: {InvNumber}", vin, invNumber);
                }

                return trailers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during global trailer search for VIN: {VIN} or InvNumber: {InvNumber}", vin, invNumber);
                throw;
            }
        }



    }
}

// CreateTrailersAsync：创建单个或多个拖车记录。
// GetAllTrailersAsync：获取所有拖车。
// EditTrailerDetailsAsync：编辑拖车的部分或所有信息。
// DeleteTrailerAsync：删除单个拖车。
// BatchDeleteTrailersAsync：批量删除拖车。
// GetTrailerLogsAsync：获取指定拖车的操作日志。
// GetTrailerById 方法
// GlobalSearchTrailersAsync 全局搜索。