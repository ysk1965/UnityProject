using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtil
{
    public static Color GetGradeColor(int grade)
    {
        Color color = Color.white;
        switch (grade)
        {
            case 1:
                ColorUtility.TryParseHtmlString("#48affe", out color);
                break;
            case 2:
                ColorUtility.TryParseHtmlString("#cc5fff", out color);
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#FF6060", out color);
                break;
            case 4:
                ColorUtility.TryParseHtmlString("#fdb53e", out color);
                break;
        }
        return color;
    }
}
