using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using IslandEscape.Entities;
using IslandEscape.Entities.Modules;

namespace IslandEscape.UI
{
    public enum UICompKey
    {
        Tooltip,
        EntityUICanvas,
    }

    [Serializable]
    public struct KeyedPrefab
    {
        public UICompKey key;
        public GameObject prefab;
    }

    public class UIManager : MonoBehaviour
    {
        public static UIManager instance = null;

        public KeyedPrefab[] prefabs;
        public Dictionary<UICompKey, GameObject> prefabMap;

        // existing UI components
        public TextMeshProUGUI clock;
        public TextMeshProUGUI timer;
        public GameObject tooltipBar;

        public void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            prefabMap = new Dictionary<UICompKey, GameObject>();
            foreach (KeyedPrefab prefab in prefabs)
            {
                prefabMap.Add(prefab.key, prefab.prefab);
            }
        }

        public void Update()
        {
            if (!GameManager.instance.gamePaused)
            {
                UpdateClock();
                UpdateTimer();
            }
        }

        /// <summary>
        /// Base tooltip initialization.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private GameObject TooltipInit(string text)
        {
            GameObject tooltip = (GameObject)Instantiate(prefabMap[UICompKey.Tooltip]);
            tooltip.GetComponentInChildren<TextMeshProUGUI>().text = text;
            return tooltip;
        }

        /// <summary>
        /// Adds a tooltip to the tooltip bar at the bottom of the screen.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public GameObject ToolTipAdd(string text)
        {
            GameObject tooltip = TooltipInit(text);
            return tooltip;
        }

        /// <summary>
        /// Adds a tooltip above the target entity's position.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public GameObject ToolTipAdd(string text, Entity target)
        {
            GameObject tooltip = TooltipInit(text);
            target.GetComponent<RenderModule>().CanvasSet(tooltip);
            return tooltip;
        }

        /// <summary>
        /// Destroy the given tooltip.
        /// </summary>
        /// <param name="tooltip"></param>
        public void ToolTipRemove(GameObject tooltip)
        {

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
