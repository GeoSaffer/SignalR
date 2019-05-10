using Microsoft.AspNet.SignalR;
using MoveShapeDemo.Broadcasters;
using MoveShapeDemo.Models;

namespace MoveShapeDemo.Hubs
{
    public class MoveShapeHub : Hub
    {
        // Is set via the constructor on each creation
        private Broadcaster _broadcaster;
        public MoveShapeHub()
            : this(Broadcaster.Instance)
        {

        }

        public MoveShapeHub(Broadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public void UpdateModel(ShapeModel clientModel)
        {
            clientModel.LastUpdatedBy = Context.ConnectionId;

            // Update the shape model within our broadcaster
            _broadcaster.UpdateShape(clientModel);
        }

    }
}
