using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace GestionOrdinateurs.Client.Pages
{
    public partial class Computers : ComponentBase
    {
        [Inject]
        public IHttpClientFactory Factory { get; set; } = default!;

        protected ComputerItem[]? computers;

        protected override async Task OnInitializedAsync()
        {
            var client = Factory.CreateClient("ComputerApiClient");
            computers = await client.GetFromJsonAsync<ComputerItem[]>("computer");
        }

        public class ComputerItem
        {
            public string Name { get; set; } = "";
            public float Price { get; set; }
            public bool Sold { get; set; }
        }
    }
}
