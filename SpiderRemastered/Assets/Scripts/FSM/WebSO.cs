using UnityEngine;
namespace SFRemastered
{
    [CreateAssetMenu(fileName = "New Web", menuName = "ScriptableObjects/Web")]
    public class WebSO : ScriptableObject
    {
        [Header("Web Properties")]
        public float maxWebLength = 30f;
        public float webWidth = 0.1f;
        public Material webMaterial;

        [Header("Shooting Properties")]
        public float shootCooldown = 0.5f;
        public float shootForce = 50f;

        [Header("Pulling Properties")]
        public float minRopeLength = 5f;
        public float maxPullForce = 30f;
        public float pullDuration = 0.5f;

        [Header("Swinging Properties")]
        public float tensionForce = 50f;
        public float centripetalForce = 50f;
        public float swingForce = 30f;
        public float maxSwingSpeed = 100;

        [Header("Release Properties")]
        public float releaseBoost = 100f;
    }
}
