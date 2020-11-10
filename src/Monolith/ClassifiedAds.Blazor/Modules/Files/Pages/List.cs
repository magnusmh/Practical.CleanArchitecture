using ClassifiedAds.Blazor.Modules.Core.Components;
using ClassifiedAds.Blazor.Modules.Files.Components;
using ClassifiedAds.Blazor.Modules.Files.Models;
using ClassifiedAds.Blazor.Modules.Files.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassifiedAds.Blazor.Modules.Files.Pages
{
    public partial class List
    {
        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        public FileService FileService { get; set; }

        public List<FileEntryModel> Files { get; set; } = new List<FileEntryModel>();

        protected AuditLogsDialog AuditLogsDialog { get; set; }

        protected ConfirmDialog DeleteDialog { get; set; }

        public FileEntryModel DeletingFile { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            Files = await FileService.GetFiles();
        }

        protected async Task ViewAuditLogs(FileEntryModel file)
        {
            var logs = await FileService.GetAuditLogs(file.Id);
            AuditLogsDialog.Show(logs);
        }

        protected void DeleteFile(FileEntryModel file)
        {
            DeletingFile = file;
            DeleteDialog.Show();
        }

        public async void ConfirmedDeleteFile()
        {
            await FileService.DeleteFile(DeletingFile.Id);
            DeleteDialog.Close();
            Files = await FileService.GetFiles();
            StateHasChanged();
        }
    }
}
