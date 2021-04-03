using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeAreaPanel : MonoBehaviour
{
    public bool IsSafeArea { get => this.isSafeArea; }
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        RefreshPanel(Screen.safeArea);
    }

    private void OnEnable()
    {
        SafeAreaDetection.OnSafeAreaChanged += RefreshPanel;
        RefreshPanel(Screen.safeArea);
        isSafeArea = true;
    }

    private void OnDisable()
    {
        SafeAreaDetection.OnSafeAreaChanged -= RefreshPanel;
        RefreshPanel(Screen.safeArea);
        isSafeArea = false;
    }

    private void RefreshPanel(Rect safeArea)
    {
        Debug.LogColor("Rect " + safeArea);
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        //Debug.LogColor("anchorMin " + anchorMin);
        //Debug.LogColor("anchorMax " + anchorMax);

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        if (rectTransform != null)
        {
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }

    private bool isSafeArea = false;
}
