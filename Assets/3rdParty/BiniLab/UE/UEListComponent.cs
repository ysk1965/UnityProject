/*********************************************
 * UGUI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UEListComponent : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public

    public RectTransform RectTransform
    {
        get
        {
            if (this.rectTransform == null)
                this.rectTransform = this.GetComponent<RectTransform>();
            return this.rectTransform;
        }
    }

    public int Index { get { return this.index; } set { this.index = value; } }
    public int Group { get { return this.group; } set { this.group = value; } }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // private

    private RectTransform rectTransform;

    private int group = -1;
    private int index = -1;
}
