using UnityEngine;

namespace SFRemastered
{
    public class TargetIcon : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (Camera.main != null)
            {
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0, 180, 0); // Ensure the sprite faces the camera
            }
        }
    }
}