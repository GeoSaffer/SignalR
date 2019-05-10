using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Web;
using ClientViewer.Hubs;
using ClientViewer.Models;
using Microsoft.AspNet.SignalR;

namespace ClientViewer.Broadcasters
{
    public class Broadcaster
    {
        private readonly static Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());

        // We're going to broadcast to all clients a maximum of 25 times per second
        private readonly TimeSpan _broadcastInterval = TimeSpan.FromMilliseconds(40);
        private readonly IHubContext _hubContext;
        private Timer _broadcastLoop;
        private ClientViewerModel _model;
        private bool _modelUpdated;

        public Broadcaster()
        {
            // Save our hub context so we can easily use it 
            // to send to its connected clients
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<ClientViewerHub>();
            _model = new ClientViewerModel();
            _modelUpdated = false;
            // Start the broadcast loop
            _broadcastLoop = new Timer(
                BroadcastView,
                null,
                _broadcastInterval,
                _broadcastInterval
            );
        }

        public static Broadcaster Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void BroadcastView(object state)
        {
            // No need to send anything if our model hasn't changed
            if (_modelUpdated)
            {
                // This is how we can access the Clients property 
                // in a static hub method or outside of the hub entirely
                _hubContext.Clients.AllExcept(_model.LastUpdatedBy).updateView(_model);
                _modelUpdated = false;
            }
        }

        public void UpdateView(ClientViewerModel viewerModel)
        {
            _model = viewerModel;
            _modelUpdated = true;
        }

    }
}