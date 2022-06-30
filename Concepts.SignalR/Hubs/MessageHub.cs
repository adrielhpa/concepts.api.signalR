using Amazon.SQS.Model;
using Concepts.SignalR.Models;
using Microsoft.AspNetCore.SignalR;

namespace Concepts.SignalR.Hubs
{
    public class MessageHub : Hub
    {
        #region Groups
        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public Task ExitGroup(string groupName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        #endregion

        #region User
        public async Task SendCreatedUser(Response res)
        {
            await Clients.Group("Users").SendAsync("CreatedUser", res);
        }
        public async Task SendUpdatedUser(Response res)
        {
            await Clients.Group("Users").SendAsync("UpdatedUser", res);
        }
        public async Task SendDeletedUser(Response res)
        {
            await Clients.Group("Users").SendAsync("DeletedUser", res);
        }
        #endregion

        #region Product

        public async Task SendCreatedProduct(Response res)
        {
            await Clients.Group("Products").SendAsync("CreatedProduct", res);
        }
        public async Task SendUpdatedProduct(Response res)
        {
            await Clients.Group("Products").SendAsync("UpdatedProduct", res);
        }
        public async Task SendDeletedProduct(Response res)
        {
            await Clients.Group("Products").SendAsync("DeletedProduct", res);
        }

        #endregion
    }
}
