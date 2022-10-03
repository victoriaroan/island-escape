using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ChildSizeFitter : MonoBehaviour
{
    public RectOffset padding;

    public void Start()
    {
        RebuildFit();
    }

    public void RebuildFit()
    {
        RectTransform children = GetComponentInChildren<RectTransform>();

        // slipped into python naming convention, oops
        float min_x, max_x, min_y, max_y;
        min_x = max_x = transform.position.x;
        min_y = max_y = transform.position.y;

        // Debug.Log($"min_x: {min_x}, max_x: {max_x}, min_y: {min_y}, max_y: {max_y}");

        foreach (RectTransform child in children)
        {
            var pref_width = LayoutUtility.GetPreferredWidth(child);
            var pref_height = LayoutUtility.GetPreferredHeight(child);
            var child_x = Math.Abs(child.anchoredPosition.x);
            var child_y = Math.Abs(child.anchoredPosition.y);

            // Debug.Log($"pref_width: {pref_width}, pref_height: {pref_height}, min_x: {child_x}, min_y: {child_y}", child.gameObject);

            float child_max_x = child_x + pref_width;
            if (child_max_x > max_x)
                max_x = child_max_x;

            float child_max_y = child_y + pref_height;
            if (child_max_y > max_y)
                max_y = child_max_y;
        }

        // TODO add padding
        GetComponent<RectTransform>().sizeDelta = new Vector2(max_x + padding.right, max_y + padding.bottom);
    }
}
