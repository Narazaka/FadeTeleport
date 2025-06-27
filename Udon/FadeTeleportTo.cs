using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;

namespace Narazaka.VRChat.FadeTeleport
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class FadeTeleportTo : UdonSharpBehaviour
    {
        [SerializeField] FadeTeleporter fadeTeleport;
        [SerializeField] Transform destination;

        public override void Interact()
        {
            ReserveTeleport();
        }

        [PublicAPI]
        public void ReserveTeleport()
        {
            fadeTeleport.ReserveTeleportTo(destination.position, destination.rotation);
        }
    }
}
