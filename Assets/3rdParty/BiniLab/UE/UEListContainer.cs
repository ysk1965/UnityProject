/*********************************************
 * UGUI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UEListContainer : MonoBehaviour
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public

    //delegate for update list item
    public delegate void OnUpdateItem(int index, UEListComponent comp);

    public void InitListData(GameObject itemPrefab, int count, int countPerLine, float bottomMargin, ScrollRect scrollRect, OnUpdateItem onUpdateItemDelegate)
    {
        this.itemPrefab = itemPrefab;
        this.itemPrefab.SetActive(false);
        this.cellwidth = this.itemPrefab.GetComponent<RectTransform>().sizeDelta.x;
        this.cellheight = this.itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
        this.dataCount = count;
		this.countPerLine = countPerLine;
        this.fitParent = countPerLine == 0;
		this.bottomMargin = bottomMargin;
		this.scrollRect = scrollRect;

        this.onUpdateItem = onUpdateItemDelegate;
        this.InitData();
    }

    public void ClearList()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
        this.transform.DetachChildren();
        this.transform.localPosition = Vector3.zero;
    }

    public void RefreshAll()
    {
        if (this.items == null) return;

        for (int i = 0; i < this.items.Length; i++)
        {
            int dataIndex = this.items[i].Index;
            if (dataIndex >= 0)
            {
                this.onUpdateItem(dataIndex, this.items[i]);
            }
        }
    }

    public void RefreshAll(int count)
    {
        this.dataCount = count;
        this.InitData(true);
    }

    public void MoveToTop()
    {
        this.StartCoroutine(this.MoveToTopCoroutine());
    }

    public int Count
    {
        get { return this.dataCount; }
    }

    public int CountPerLine
    {
        get { return this.countPerLine; }
    }

    public ScrollRect ScrollRect
    {
        get { return this.scrollRect; }
    }

    public float CellWidth
    {
        get { return this.cellwidth; }
    }

    public float CelllHeight
    {
        get { return this.cellheight; }
    }

    public float BottomMargin
    {
        get { return this.bottomMargin; }
    }

    public float ScrollLimitCount
    {
        get { return this.itemCount - this.countPerLine; }
    }

    public float Alpha
    {
        set
        {
            this.alpha = value;
            if (this.alphaCanvas != null)
                this.alphaCanvas.alpha = this.alpha;
        }
    }

    public void CenterItem(int index)
    {

        if (this.scrollRect.vertical)
        {
            float offSet = (float)index * this.CelllHeight;
            float scrollCenter = this.scrollRectValue.height / 2f;
            float itemCenter = this.CelllHeight / 2f;
            float max = this.rectTransform.rect.height - this.scrollRectValue.height;
            this.rectTransform.anchoredPosition = new Vector2(0f, -(Mathf.Clamp((offSet - scrollCenter + itemCenter), 0, max)));
        }
        else
        {
            float offSet = (float)index * this.CellWidth;
            float scrollCenter = this.scrollRectValue.width / 2f;
            float itemCenter = this.CellWidth / 2f;
            float max = this.rectTransform.rect.width - this.scrollRectValue.width;
            this.rectTransform.anchoredPosition = new Vector2(-(Mathf.Clamp((offSet - scrollCenter + itemCenter), 0, max)), 0f);
        }
        this.UpdateItemPosition();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected

    protected void Awake()
    {
        if (this.scrollRect == null)
            this.scrollRect = GetComponentInParent<ScrollRect>();

        this.scrollRect.onValueChanged.AddListener(this.OnScrollChange);
        this.rectTransform = this.GetComponent<RectTransform>();
        this.alphaCanvas = this.GetComponent<CanvasGroup>();
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // private

    private float cellwidth;
    private float cellheight;
    private int dataCount;
    private int countPerLine = 1;
    private bool fitParent = false;
    private float bottomMargin;
    private GameObject itemPrefab;
    private ScrollRect scrollRect;
    private RectTransform rectTransform;
    private Rect scrollRectValue;

    private OnUpdateItem onUpdateItem;

    private int itemCount;
    private UEListComponent[] items;

    private float alpha;
    private CanvasGroup alphaCanvas;

    private void InitData(bool keepPostion = false)
    {
        this.ClearAllChild();
        this.rectTransform = this.GetComponent<RectTransform>();
        this.scrollRectValue = this.scrollRect.GetComponent<RectTransform>().rect;
        this.countPerLine = this.fitParent ? Mathf.Min(this.countPerLine, Mathf.FloorToInt(this.scrollRectValue.width / this.cellwidth)) : this.countPerLine;
        int rowCount = Mathf.CeilToInt((float)this.dataCount / (float)this.countPerLine);

        if (this.scrollRect.vertical)
        {
            float width = this.fitParent ? this.countPerLine * this.cellwidth : this.rectTransform.sizeDelta.x;
            this.rectTransform.sizeDelta = new Vector2(width, this.bottomMargin + Mathf.Max(this.scrollRect.GetComponent<RectTransform>().sizeDelta.y, this.cellheight * (float)rowCount));
            this.rectTransform.anchorMax = new Vector2(0.5f, 1f);
            this.rectTransform.anchorMin = new Vector2(0.5f, 1f);
            this.rectTransform.pivot = new Vector2(0.5f, 1f);
            this.itemCount = (int)(Mathf.Abs(Mathf.Ceil(this.scrollRectValue.height / this.cellheight)) + 1) * this.countPerLine;
            this.scrollRect.enabled = (this.scrollRectValue.height - this.bottomMargin) < (rowCount * this.cellheight);
        }
        else
        {
            this.rectTransform.sizeDelta = new Vector2(Mathf.Max(this.scrollRect.GetComponent<RectTransform>().sizeDelta.x, this.cellwidth * (float)rowCount), this.rectTransform.sizeDelta.y);
            this.rectTransform.anchorMax = new Vector2(0f, 0.5f);
            this.rectTransform.anchorMin = new Vector2(0f, 0.5f);
            this.rectTransform.pivot = new Vector2(0f, 0.5f);
            this.itemCount = (int)(Mathf.Abs(Mathf.Ceil(this.scrollRectValue.width / this.cellwidth)) + 1) * this.countPerLine;
        }

        if (!keepPostion)
            this.rectTransform.anchoredPosition = Vector2.zero;

        this.items = new UEListComponent[this.itemCount];

        for (int i = 0; i < this.itemCount; i++)
        {
            GameObject go = Instantiate(this.itemPrefab) as GameObject;
            go.SetActive(true);
            this.items[i] = go.GetComponent<UEListComponent>();
            this.items[i].RectTransform.SetParent(this.transform);
            this.items[i].RectTransform.localScale = Vector3.one;
            this.items[i].RectTransform.localPosition = Vector3.zero;
            this.items[i].RectTransform.anchorMax = new Vector2(0, 1);
            this.items[i].RectTransform.anchorMin = new Vector2(0, 1);
            this.items[i].RectTransform.pivot = new Vector2(0, 1);
            this.items[i].RectTransform.sizeDelta = new Vector2(this.cellwidth, this.cellheight);
        }

        this.UpdateItemPosition();
    }

    private void UpdateItemPosition()
    {
        if (this.scrollRect.vertical)
        {
            //Remove Spring area when list Dragging on each end of side.
            float min = 0f;
            float max = this.rectTransform.rect.height - this.scrollRectValue.height;
            float curPosY = Mathf.Clamp(this.rectTransform.anchoredPosition.y, min, max);

            //Start Y Position for item's position in first line
            float startPosY = Mathf.Floor(curPosY / this.cellheight) * this.cellheight;
            //Start index for first item in list
            int startIndex = Mathf.FloorToInt(curPosY / this.cellheight) * this.countPerLine;

            for (int i = 0; i < this.itemCount; i++)
            {
                int dataIndex = startIndex + i;

                if (!this.items[i]) continue;

                if (dataIndex < 0 || dataIndex >= this.dataCount)
                {
                    this.items[i].gameObject.SetActive(false);
                }
                else
                {
                    bool forceUpdate = false;
                    if (!this.items[i].gameObject.activeSelf)
                    {
                        this.items[i].gameObject.SetActive(true);
                        forceUpdate = true;
                    }

                    float posX = this.cellwidth * (i % this.countPerLine);
                    float posY = -(startPosY + (int)((i / this.countPerLine) * this.cellheight));

                    this.items[i].RectTransform.anchoredPosition = new Vector2(posX, posY);
                    if (forceUpdate || dataIndex != this.items[i].Index)
                    {
                        this.items[i].Index = dataIndex;
                        this.onUpdateItem(dataIndex, this.items[i]);
                    }
                }
            }
        }
        else
        {
            //Remove Spring area when list Dragging on each end of side.
            float min = -this.rectTransform.rect.width - this.scrollRectValue.width;
            float max = 0f;
            float curPosX = Mathf.Abs(Mathf.Clamp(this.rectTransform.anchoredPosition.x, min, max));

            //Start X Position for item's position in first line
            float startPosX = Mathf.Floor(curPosX / this.cellwidth) * this.cellwidth;
            //Start index for first item in list
            int startIndex = Mathf.FloorToInt(curPosX / this.cellwidth) * this.countPerLine;

            for (int i = 0; i < this.itemCount; i++)
            {
                int dataIndex = startIndex + i;

                if (!this.items[i]) continue;

                if (dataIndex < 0 || dataIndex >= this.dataCount)
                {
                    this.items[i].gameObject.SetActive(false);
                }
                else
                {
                    bool forceUpdate = false;
                    if (!this.items[i].gameObject.activeSelf)
                    {
                        this.items[i].gameObject.SetActive(true);
                        forceUpdate = true;
                    }

                    float posX = startPosX + (int)((i / this.countPerLine) * this.cellwidth);
                    float posY = -(this.cellheight * (i % this.countPerLine));

                    this.items[i].RectTransform.anchoredPosition = new Vector2(posX, posY);
                    if (forceUpdate || dataIndex != this.items[i].Index)
                    {
                        this.items[i].Index = dataIndex;
                        this.onUpdateItem(dataIndex, this.items[i]);
                    }
                }
            }
        }
    }

    private void OnScrollChange(Vector2 value)
    {
        if (this.items != null)
            this.UpdateItemPosition();
    }

    private void ClearAllChild()
    {
        int childCount = this.transform.childCount;
        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
            this.transform.DetachChildren();
        }
    }

    private IEnumerator MoveToTopCoroutine()
    {
        float verticalNormalizePos = this.scrollRect.verticalNormalizedPosition;
        while (verticalNormalizePos < 1f)
        {
            verticalNormalizePos += 0.05f;
            if (verticalNormalizePos > 1f)
                verticalNormalizePos = 1f;
            this.scrollRect.verticalNormalizedPosition = verticalNormalizePos;
            yield return null;
        }
        yield break;
    }

}
