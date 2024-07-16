using System.Collections;
using UnityEngine;

namespace SFRemastered.Ultils
{
    public class CoroutineRunner : MonoBehaviour
    {
        // Singleton instance
        public static CoroutineRunner Instance { get; private set; }

        private void Awake() 
        {
            // If the instance is already set, destroy this object
            if (Instance != null) 
            {
                Destroy(this);
                return;
            }

            // Set the instance
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }

        public void StartRoutine(IEnumerator routine) 
        {
            StartCoroutine(routine);
        }
    
        public void StopRoutine(IEnumerator routine) 
        {
            StopCoroutine(routine);
        }
    }
}