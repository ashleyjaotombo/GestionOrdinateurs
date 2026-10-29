using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace GestionOrdinateurs.Client.Pages
{
    public partial class RepairHistory : ComponentBase
    {
        [Inject]
        public IHttpClientFactory Factory { get; set; } = default!;

        protected RepairHistoryItem[]? repairs;

        protected override async Task OnInitializedAsync()
        {
            var client = Factory.CreateClient("ComputerApiClient");
            repairs = await client.GetFromJsonAsync<RepairHistoryItem[]>("repair/history");
        }

        public class RepairHistoryItem
        {
            public int ComputerId { get; set; }
            public string ComputerName { get; set; } = "";
            public string Technician { get; set; } = "";
            public string RepairType { get; set; } = "";
            public DateTime InterventionDate { get; set; }
        }
    }
}
