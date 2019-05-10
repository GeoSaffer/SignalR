using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatDemo.Broadcasters;
using ChatDemo.Models;
using Microsoft.AspNet.SignalR;

namespace ChatDemo.Hubs
{

   public class ChatHub : Hub
   {
       private Broadcaster _broadcaster;

       public ChatHub()
        : this (Broadcaster.Instance)
       {

       }

       public ChatHub(Broadcaster broadcaster)
       {
           _broadcaster = broadcaster;
       }

       public void UpdateModel(ChatModel clientModel)
       {
           clientModel.LastUpdatedBy = Context.ConnectionId;

           //update the chat within our broadcaster
           _broadcaster.UpdateChat(clientModel);
       }

       public void Send(string name, string message)
       {
           var clientModel = new ChatModel();
           clientModel.Name = name;
           clientModel.Message = message;
           clientModel.LastUpdatedBy = Context.ConnectionId;

           //update the chat within our broadcaster
           _broadcaster.UpdateChat(clientModel);
       }

    }

}