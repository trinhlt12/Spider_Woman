using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    public class SFXManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] footstepClips;
        [SerializeField] private AudioClip[] footlandClips;

        public void PlayFootstep()
        {
            if (footstepClips.Length > 0)
            {
                int randomIndex = Random.Range(0, footstepClips.Length);
                AudioSource.PlayClipAtPoint(footstepClips[randomIndex], transform.position);
            }
        }

        public void PlayFootland()
        {
            if (footlandClips.Length > 0)
            {
                int randomIndex = Random.Range(0, footlandClips.Length);
                AudioSource.PlayClipAtPoint(footlandClips[randomIndex], transform.position);
            }
        }
    }
}
