using ClassifiedAds.Blazor.Modules.Files.Models;
using ClassifiedAds.Blazor.Modules.Files.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace ClassifiedAds.Blazor.Modules.Files.Pages
{
    public partial class Edit
    {
        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        public FileService FileService { get; set; }

        [Parameter]
        public string Id { get; set; }

        public FileEntryModel File { get; set; } = new FileEntryModel();

        protected override async Task OnInitializedAsync()
        {
            File = await FileService.GetFileById(Guid.Parse(Id));
        }
    }
}
