using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandEscape.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance = null;

        public TextMeshProUGUI clock;
        public TextMeshProUGUI timer;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (!GameManager.instance.gamePaused)
            {
                UpdateClock();
                UpdateTimer();
            }
        }

        private void UpdateClock()
        {
            // TODO how to actually track elapsed time taking pauses into consideration (may not matter if we don't let player pause after start)
            // TODO: track sped up time
            float totalTime = Time.timeSinceLevelLoad - GameManager.instance.startTime;
            int minute = totalTime > 60 ? (int)Math.Floor(totalTime / 60) : 0;
            int seconds = (int)Math.Round(totalTime % 60);
            if (seconds == 60)
            {
                // TODO: there's almost certainly a better way to do this, but this is quick and easy.
                minute += 1;
                seconds = 0;
            }

            clock.text = minute.ToString() + "m " + seconds.ToString() + "s";
        }

        private void UpdateTimer()
        {
            var waterMgr = GameManager.instance.waterManager;
            timer.text = "Water Rises " + waterMgr.SizeOfNextRise + " tiles in " + waterMgr.TimeToNextRise + "s";
        }
    }
}
