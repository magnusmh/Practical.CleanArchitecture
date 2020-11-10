using ClassifiedAds.Blazor.Modules.Core.Services;
using ClassifiedAds.Blazor.Modules.Files.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClassifiedAds.Blazor.Modules.Files.Services
{
    public class FileService : HttpService
    {
        public FileService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) 
            : base(httpClient, httpContextAccessor)
        {
        }

        public async Task<List<FileEntryModel>> GetFiles()
        {
            var files = await GetAsync<List<FileEntryModel>>("api/files");
            return files;
        }

        public async Task<FileEntryModel> GetFileById(Guid id)
        {
            var file = await GetAsync<FileEntryModel>($"api/files/{id}");
            return file;
        }

        public async Task<FileEntryModel> CreateFile(FileEntryModel product)
        {
            var createdFile = await PostAsync<FileEntryModel>("api/files", product);
            return createdFile;
        }

        public async Task<FileEntryModel> UpdateFile(Guid id, FileEntryModel product)
        {
            var updatedFile = await PutAsync<FileEntryModel>($"api/files/{id}", product);
            return updatedFile;
        }

        public async Task DeleteFile(Guid id)
        {
            await DeleteAsync($"api/files/{id}");
        }

        public async Task<List<FileEntryAuditLogModel>> GetAuditLogs(Guid id)
        {
            var auditLogs = await GetAsync<List<FileEntryAuditLogModel>>($"api/files/{id}/auditlogs");
            return auditLogs;
        }
    }
}
