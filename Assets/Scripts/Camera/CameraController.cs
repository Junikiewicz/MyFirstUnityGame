using MyRPGGame.Events;
using MyRPGGame.Player;
using UnityEngine;

namespace MyRPGGame.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float shakeMagnitude, shakeTime;

        private readonly Vector3 cameraOffset = new Vector3(0, 0, -1);

        private bool shaking;
        private static CameraController instance;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                EventManager.Instance.AddListener<OnPlayerHit>(ShakeIt);
            }
            else
            {
                Destroy(gameObject);//Prevent object duplicates when switching scenes
            }
        }
        private void Update()
        {
            if (PlayerController.Instance != null)
            {
                transform.position = PlayerController.Instance.GetCurrentPlayerPosition() + cameraOffset;
            }
            if (shaking)
            {
                float cameraShakingOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
                float cameraShakingOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
                transform.position = transform.position + Vector3.right * cameraShakingOffsetX + Vector3.up * cameraShakingOffsetY;
            }
        }
        private void ShakeIt(OnPlayerHit eventData)
        {
            shaking = true;
            Invoke(nameof(StopCameraShaking), shakeTime);
        }
        private void StopCameraShaking()
        {
            shaking = false;
        }
        private void OnDestroy()
        {
            if(instance==this)
            {
                EventManager.Instance.RemoveListener<OnPlayerHit>(ShakeIt);
                instance = null;
            }
        }
    }
}