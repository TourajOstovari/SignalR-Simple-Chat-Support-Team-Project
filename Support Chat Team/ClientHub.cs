using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Support_Chat_Team
{
    public class ClientHub:Hub
    {
        static System.Collections.Generic.Dictionary<string, Guid> conID = new Dictionary<string, Guid>();
        static System.Collections.Generic.Dictionary<string, Guid> admins = new Dictionary<string, Guid>();
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Welcome", "Welcome dear ");
            conID.Add(Context.ConnectionId, Guid.NewGuid());
            return base.OnConnectedAsync();
        }
        public void sendMSG(string name,string text)
        {
            Clients.AllExcept(conID.Keys).SendAsync("AdminMsg",name,text);
        }
        public void Checkadmin()
        {
            Clients.Caller.SendAsync("OnlineAdmins", admins.Count);
        }
        public void remove()
        {
            admins.Add(Context.ConnectionId,conID[Context.ConnectionId]);
            conID.Remove(Context.ConnectionId);
            
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (admins.Keys.Contains(Context.ConnectionId)) admins.Remove(Context.ConnectionId);
            if (conID.Keys.Contains(Context.ConnectionId)) conID.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
