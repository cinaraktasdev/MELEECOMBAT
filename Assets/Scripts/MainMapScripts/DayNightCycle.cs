using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

namespace DNcycle
{
    [RequireComponent(typeof(Light2D))]

    public class DayNightCycle : MonoBehaviour
    {
        public float duration;
        [SerializeField] private Gradient gradient;
        private Light2D light2D;
        private float startTime;

        public GameObject sunset;

        private void Awake()
        {
            light2D = GetComponent<Light2D>();
            startTime = Time.time;
        }

        void Update()
        {
            var timeElapsed = Time.time - startTime;
            var percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;
            percentage = Mathf.Clamp01(percentage);
            light2D.color = gradient.Evaluate(percentage);

            if( percentage > 0.35 && percentage < 0.49)
            {
                sunset.SetActive(true);
            }
            else
            {
                sunset.SetActive(false); 
            }
        }
    }
}
