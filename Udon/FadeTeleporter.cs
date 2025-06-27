using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Narazaka.VRChat.FadeTeleport
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class FadeTeleporter : UdonSharpBehaviour
    {
        [SerializeField] bool defaultLockPosition = true;
        [SerializeField] float fadeDuration = 1.0f;
        [SerializeField] float stayFadeDuration = 0.5f;
        [SerializeField] Renderer fadeRenderer;

        float fadeSpeed => 1.0f / fadeDuration;

        float rate;
        float remainStayStartTime;
        float remainStayEndTime; // teleport time
        float remainEndTime; // fade to normal time

        Vector3 position;
        Quaternion rotation;
        bool respawn;
        bool lockPosition;
        bool specifyLockPosition;

        bool useLockPosition => specifyLockPosition ? lockPosition : defaultLockPosition;

        [PublicAPI]
        public void ReserveTeleportTo(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
            respawn = false;
            specifyLockPosition = false;
            SetTimes();
            Debug.Log($"[FadeTeleporter](Reserve) teleport to {position} {rotation}");
        }

        [PublicAPI]
        public void ReserveTeleportTo(Vector3 position, Quaternion rotation, bool lockPosition)
        {
            this.position = position;
            this.rotation = rotation;
            respawn = false;
            this.lockPosition = lockPosition;
            specifyLockPosition = true;
            SetTimes();
            Debug.Log($"[FadeTeleporter](Reserve) teleport to {position} {rotation}");
        }

        [PublicAPI]
        public void ReserveRespawn()
        {
            respawn = true;
            specifyLockPosition = false;
            SetTimes();
            Debug.Log($"[FadeTeleporter](Reserve) respawn");
        }

        [PublicAPI]
        public void ReserveRespawn(bool lockPosition)
        {
            respawn = true;
            this.lockPosition = lockPosition;
            specifyLockPosition = true;
            SetTimes();
            Debug.Log($"[FadeTeleporter](Reserve) respawn");
        }

        void SetTimes()
        {
            if (remainEndTime == 0f)
            {
                remainStayStartTime = fadeDuration;
            }
            remainStayEndTime = fadeDuration + stayFadeDuration;
            remainEndTime = fadeDuration + stayFadeDuration + fadeDuration;
        }

        void Update()
        {
            var deltaTime = Time.deltaTime;
            if (remainStayStartTime > 0f)
            {
                remainStayStartTime = Mathf.Max(remainStayStartTime - deltaTime, 0);
                remainStayEndTime = Mathf.Max(remainStayEndTime - deltaTime, 0);
                remainEndTime = Mathf.Max(remainEndTime - deltaTime, 0);
                rate = Mathf.Clamp01(rate + deltaTime * fadeSpeed);
            }
            else if (remainStayEndTime > 0f)
            {
                remainStayEndTime = Mathf.Max(remainStayEndTime - deltaTime, 0);
                remainEndTime = Mathf.Max(remainEndTime - deltaTime, 0);
                rate = Mathf.Clamp01(rate + deltaTime * fadeSpeed);
                if (remainStayEndTime <= 0f)
                {
                    Teleport();
                }
            }
            else if (remainEndTime > 0f)
            {
                remainEndTime = Mathf.Max(remainEndTime - deltaTime, 0);
                rate = Mathf.Clamp01(rate - deltaTime * fadeSpeed);
            }
            else
            {
                rate = 0f;
            }
            if (rate > 0f)
            {
                fadeRenderer.enabled = true;
                fadeRenderer.material.SetFloat("_Alpha", rate);
                if (useLockPosition) Networking.LocalPlayer.Immobilize(true);
            }
            else
            {
                fadeRenderer.enabled = false;
                Networking.LocalPlayer.Immobilize(false);
            }
        }

        void Teleport()
        {
            if (respawn)
            {
                Networking.LocalPlayer.Respawn();
                Debug.Log("[FadeTeleporter](Teleport) Respawn");
            }
            else
            {
                Networking.LocalPlayer.TeleportTo(position, rotation);
                Debug.Log($"[FadeTeleporter](Teleport) Teleport to {position} {rotation}");
            }
        }
    }
}
