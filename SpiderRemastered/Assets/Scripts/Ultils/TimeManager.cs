using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager instance;

        public float expectedTimeScale = 1f;

        public bool focus;

        public bool pause;

        public float gameTime { get; private set; }
        public float unscaledGameTime { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetTimeScale(float timeScale)
        {
            expectedTimeScale = timeScale;
        }

        private void Update()
        {
            float actualTimeScale = 1f;

            if (pause)
            {
                actualTimeScale = 0f;
            }
            else if (focus)
            {
                actualTimeScale = 0.2f;
            }
            else
            {
                actualTimeScale = expectedTimeScale;
            }

            Time.timeScale = actualTimeScale;

            gameTime += Time.deltaTime;

            unscaledGameTime += Time.unscaledDeltaTime;
        }
    }
}
