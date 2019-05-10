using System;
using System.Threading;
using Microsoft.AspNet.SignalR;

using ChatDemo.Hubs;
using ChatDemo.Models;

namespace ChatDemo.Broadcasters
{
    public class Broadcaster
    {
        private readonly static Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());
       
        // We're going to broadcast to all clients a maximum of 25 times per second
        private readonly TimeSpan _broadcastInterval = TimeSpan.FromMilliseconds(40);
        private readonly IHubContext _hubContext;
        private Timer _broadcastLoop;
        private ChatModel _model;
        private bool _modelUpdated;

        public Broadcaster()
        {
            // Save our hub context so we can easily use it 
            // to send to its connected clients
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            _model = new ChatModel();
            _modelUpdated = false;
            // Start the broadcast loop
            _broadcastLoop = new Timer(
                BroadcastChat,
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

        public void BroadcastChat(object state)
        {
            // No need to send anything if our model hasn't changed
            if (_modelUpdated)
            {
                // This is how we can access the Clients property 
                // in a static hub method or outside of the hub entirely
                _hubContext.Clients.All.updateChat(_model);
                _modelUpdated = false;
            }
        }

        public void UpdateChat(ChatModel clientModel)
        {
            _model = clientModel;
            _modelUpdated = true;
        }


    }
}