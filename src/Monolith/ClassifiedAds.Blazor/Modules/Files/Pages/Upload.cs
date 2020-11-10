using ClassifiedAds.Blazor.Modules.Files.Models;
using ClassifiedAds.Blazor.Modules.Files.Services;
using Microsoft.AspNetCore.Components;

namespace ClassifiedAds.Blazor.Modules.Files.Pages
{
    public partial class Upload
    {
        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        public FileService FileService { get; set; }

        public FileEntryModel File { get; set; } = new FileEntryModel();
    }
}
