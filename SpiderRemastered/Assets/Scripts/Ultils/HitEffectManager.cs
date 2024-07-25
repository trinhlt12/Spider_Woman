using UnityEngine;

namespace SFRemastered.Ultils
{
    public class HitEffectManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem hitParticlePrefab;
        [SerializeField] private AudioClip hitSoundClip;
        [SerializeField] private float cameraShakeIntensity = 0.1f;
        [SerializeField] private float cameraShakeDuration = 0.1f;

        private AudioSource audioSource;
        private Camera mainCamera;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            mainCamera = Camera.main;
        }

        public void PlayHitEffect(Vector3 hitPoint, Vector3 hitNormal)
        {
            // Spawn particle effect
            if (hitParticlePrefab != null)
            {
                ParticleSystem hitParticle = Instantiate(hitParticlePrefab, hitPoint, Quaternion.LookRotation(hitNormal));
                Destroy(hitParticle.gameObject, hitParticle.main.duration);
            }

            // Play hit sound
            if (hitSoundClip != null)
            {
                audioSource.PlayOneShot(hitSoundClip);
            }

            // Camera shake
            StartCoroutine(CameraShake());
        }

        private System.Collections.IEnumerator CameraShake()
        {
            Vector3 originalPosition = mainCamera.transform.localPosition;
            float elapsed = 0f;

            while (elapsed < cameraShakeDuration)
            {
                float x = Random.Range(-1f, 1f) * cameraShakeIntensity;
                float y = Random.Range(-1f, 1f) * cameraShakeIntensity;

                mainCamera.transform.localPosition = new Vector3(x, y, originalPosition.z);

                elapsed += Time.deltaTime;
                yield return null;
            }

            mainCamera.transform.localPosition = originalPosition;
        }
    }
}