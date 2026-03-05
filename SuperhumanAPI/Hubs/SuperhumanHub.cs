using Microsoft.AspNetCore.SignalR;

namespace SuperhumanAPI.Hubs
{
    public class SuperhumanHub : Hub
    {
        public async Task JoinEntityGroup(string entityType)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, entityType);
        }

        public async Task LeaveEntityGroup(string entityType)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, entityType);
        }
    }
}
