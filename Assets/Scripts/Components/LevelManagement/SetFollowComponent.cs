using Cinemachine;
using Scripts.Creatures.Player;
using UnityEngine;

namespace Scripts.Components.LevelManagement
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class SetFollowComponent : MonoBehaviour
    {
        private void Start()
        {
            var vCamera = GetComponent<CinemachineVirtualCamera>();
            vCamera.Follow = FindObjectOfType<Player>().transform;
        }
    }
}