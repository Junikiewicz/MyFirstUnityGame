using UnityEngine;

namespace MyRPGGame.Events
{
    public class OnPlayerTeleportation:IGameEvent
    {
        public Vector3 destination;
        public OnPlayerTeleportation(Vector3 _destination)
        {
            destination = _destination;
        }
    }
}
