using System;
using System.Threading;
using Microsoft.AspNet.SignalR;
using MoveShapeDemo.Hubs;
using MoveShapeDemo.Models;

namespace MoveShapeDemo.Broadcasters
{
    public class Broadcaster
    {
        private static readonly Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());
        
        // broadcast to all clients a maximum of 25 times per second
        private readonly TimeSpan _broadcastInterval = TimeSpan.FromMilliseconds(40);
        private readonly IHubContext _hubContext;
        private ShapeModel _model;
        private bool _modelUpdated;

        public Broadcaster()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<MoveShapeHub>();
            _model = new ShapeModel();
            _modelUpdated = false;
            var broadcastLoop = new Timer(
                BroadcastShape,
                null,
                _broadcastInterval,
                _broadcastInterval
            );
        }

        public void BroadcastShape(object state)
        {
            // only send changes when model updated
            if (_modelUpdated)
            {
                // This is how we can access the Clients property 
                // in a static hub method or outside of the hub entirely
                _hubContext.Clients.AllExcept(_model.LastUpdatedBy).updateShape(_model);
                _modelUpdated = false;
            }
        }

        public void UpdateShape(ShapeModel clientModel)
        {
            _model = clientModel;
            _modelUpdated = true;
        }

        public static Broadcaster Instance
        {
            get
            {
                return _instance.Value;
            }
        }
    }
}