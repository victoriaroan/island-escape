using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using IslandEscape.Entities;
using IslandEscape.Entities.Modules;
using IslandEscape.Resources;

namespace IslandEscape.UI
{
    public enum UICompKey
    {
        Tooltip,
        ResourceTooltip,
        EntityUICanvas,
        Window,
    }

    [Serializable]
    public struct ComponentPrefab
    {
        public UICompKey key;
        public GameObject prefab;
    }

    public class UIManager : MonoBehaviour
    {
        public static UIManager instance = null;

        // these probably don't need to be separate, but it feels cleaner...
        // TODO: make these auto-loaded registries instead?
        public ComponentPrefab[] components;
        public Dictionary<UICompKey, GameObject> componentMap;
        public WindowPrefab[] windows;
        public Dictionary<WindowType, GameObject> windowMap;

        // existing UI components
        public TextMeshProUGUI clock;
        public TextMeshProUGUI timer;
        public GameObject tooltipBar;
        public GameObject windowPanel;

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
            componentMap = new Dictionary<UICompKey, GameObject>();
            foreach (ComponentPrefab prefab in components)
            {
                componentMap.Add(prefab.key, prefab.prefab);
            }

            windowMap = new Dictionary<WindowType, GameObject>();
            foreach (WindowPrefab prefab in windows)
            {
                windowMap.Add(prefab.type, prefab.prefab);
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


        public GameObject OpenWindow(WindowType type, GameObject source)
        {
            GameObject newWindow = (GameObject)Instantiate(windowMap[type]);
            newWindow.transform.SetParent(windowPanel.transform, false);
            newWindow.GetComponent<Window>().source = source;
            newWindow.SetActive(true);
            return newWindow;
        }

        public void CloseWindow(GameObject window)
        {
            Destroy(window);
        }


        /// <summary>
        /// Base tooltip initialization.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private GameObject TooltipInit(string text)
        {
            GameObject tooltip = (GameObject)Instantiate(componentMap[UICompKey.Tooltip]);
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
        /// Adds a resource tooltip at the given location
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public GameObject ResourceToolTipAddAtPointer(ResourceStack stack, Vector2 position)
        {
            // TODO: find a better place for this?
            GameObject tooltip = (GameObject)Instantiate(componentMap[UICompKey.ResourceTooltip]);

            tooltip.transform.Find("Image").GetComponent<Image>().sprite = stack.blueprint.sprite;
            tooltip.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = stack.blueprint.DisplayName;
            tooltip.transform.Find("Slots").GetComponent<TextMeshProUGUI>().text = String.Join(
                ", ", ((PartBlueprint)stack.blueprint).slots.Select(x => x.category.ToString())
            );
            tooltip.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = stack.blueprint.description;

            tooltip.transform.SetParent(transform, false);
            // TODO: have tooltip follow mouse when it moves around resource display
            tooltip.transform.position = position;
            tooltip.SetActive(true);
            return tooltip;
        }

        /// <summary>
        /// Destroy the given tooltip.
        /// </summary>
        /// <param name="tooltip"></param>
        public void ToolTipRemove(GameObject tooltip)
        {
            Destroy(tooltip);
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
            if (waterMgr.WaterRising)
                timer.text = "Water Rising!";
            else
                timer.text = "Water Rises " + waterMgr.SizeOfNextRise + " tiles in " + waterMgr.TimeToNextRise + "s";
        }
    }
}
