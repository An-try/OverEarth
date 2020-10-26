using System;
using UnityEngine;
using TMPro;

namespace OverEarth
{
    [RequireComponent(typeof(TMP_Text))]
    public class FPSDisplay : MonoBehaviour
    {
        // Attach this to a UI text to make a frames/second indicator.
        //
        // It calculates frames/second over each updateInterval,
        // so the display does not keep changing wildly.
        //
        // It is also fairly accurate at very low FPS counts (<10).
        // We do this not by simply counting frames per interval, but
        // by accumulating FPS for each frame. This way we end up with
        // correct overall FPS even if the interval renders something like
        // 5.5 frames.

        public float updateInterval = 0.5f;

        // FPS accumulated over the interval
        private float accum = 0;

        // Frames drawn over the interval
        private int frames = 0;

        // Left time for current interval
        private float timeleft;

        // Output
        private TMP_Text _UIText;

        private void Start()
        {
            if (!GetComponent<TMP_Text>())
            {
                Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
                enabled = false;
                return;
            }

            timeleft = updateInterval;
            _UIText = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            // Interval ended - update GUI text and start new interval
            if (timeleft <= 0.0)
            {
                // display two fractional digits (f2 format)
                float fps = accum / frames;
                string format = String.Format("{0:F2} FPS", fps);
                _UIText.text = format;

                if (fps < 30)
                {
                    _UIText.color = Color.yellow;
                }
                else if (fps < 10)
                {
                    _UIText.color = Color.red;
                }
                else
                {
                    _UIText.color = Color.green;
                }

                timeleft = updateInterval;
                accum = 0.0f;
                frames = 0;
            }
        }
    }
}
