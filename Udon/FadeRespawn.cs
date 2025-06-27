using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;

namespace Narazaka.VRChat.FadeTeleport
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class FadeRespawn : UdonSharpBehaviour
    {
        [SerializeField] FadeTeleporter fadeTeleport;

        public override void Interact()
        {
            ReserveRespawn();
        }

        [PublicAPI]
        public void ReserveRespawn()
        {
            fadeTeleport.ReserveRespawn();
        }
    }
}
