using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IslandEscape.Entities.Events;
using IslandEscape.UI;

namespace IslandEscape.Entities.Modules
{
    public class WindowModule : EntityModule
    {
        public WindowType type;
        public GameObject window;

        public void OpenWindow(ActionEventArgs args)
        {
            if (window == null)
            {
                window = GameManager.instance.ui.OpenWindow(type, gameObject);
                ((ActionModule)args.publisher).AgentExited.AddListener(CloseWindow);
            }
        }

        public void CloseWindow(ActionEventArgs args)
        {
            if (window != null)
            {
                GameManager.instance.ui.CloseWindow(window);
            }
            ((ActionModule)args.publisher).AgentExited.RemoveListener(CloseWindow);
        }
    }
}

