using Focus.UI.Models;
using Microsoft.AspNetCore.Components;

namespace Focus.UI.Pages
{
    public partial class Index : ComponentBase
    {
        private List<TaskModel> tasks = new List<TaskModel>();
        protected override Task OnInitializedAsync()
        {
            for (int i = 0; i < 20; i++)
                tasks.Add(new TaskModel { Id = Guid.NewGuid(), Name = $"Do task number {i}" });
            return base.OnInitializedAsync();
        }
    }
}