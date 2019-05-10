using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientViewer.Broadcasters;
using ClientViewer.Models;
using Microsoft.AspNet.SignalR;

namespace ClientViewer.Hubs
{
    public class ClientViewerHub : Hub
    {
        private Broadcaster _broadcaster;

        public ClientViewerHub()
            : this(Broadcaster.Instance)
        {

        }

        public ClientViewerHub(Broadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public void UpdateView(ClientViewerModel clientModel)
        {
            clientModel.LastUpdatedBy = Context.ConnectionId;

            //update the chat within our broadcaster
            _broadcaster.UpdateView(clientModel);
        }
    }
}