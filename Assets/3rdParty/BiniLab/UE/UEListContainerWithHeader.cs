/*********************************************
 * UGUI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UEListContainerWithHeader : MonoBehaviour
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // overrides MonoBehaviour 

    void Awake()
    {
        if (this.parentScrollRect == null)
            this.parentScrollRect = GetComponentInParent<ScrollRect>();

        this.parentScrollRect.onValueChanged.AddListener(this.OnScrollChange);
        this.rectTransform = this.GetComponent<RectTransform>();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public

    //delegate for update list item
    public delegate void OnUpdateItem(int group, int index, GameObject comp);
    public delegate void OnUpdateGroup(int group, GameObject comp);

    public void InitListData(int[] groups, OnUpdateItem onUpdateItemDelegate, OnUpdateGroup onUpdateGroupDelegate)
    {
        this.groupCounts = new int[groups.Length];
        for (int i = 0; i < groups.Length; i++)
        {
            int extra = groups[i] % this.countPerLine;
            this.groupCounts[i] = groups[i] + (extra > 0 ? this.countPerLine - extra : 0);
        }

        this.onUpdateItem = onUpdateItemDelegate;
        this.onUpdateGroup = onUpdateGroupDelegate;
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
        for (int i = 0; i < this.items.Length; i++)
        {
            int groupIndex = this.items[i].GetComponent<UEListComponent>().Group;
            int dataIndex = this.items[i].GetComponent<UEListComponent>().Index;

            if (dataIndex >= 0)
            {
                this.onUpdateItem(groupIndex, dataIndex, this.items[i]);
            }
        }
    }

    public void MoveToTop()
    {
        this.StartCoroutine(this.MoveToTopCoroutine());
    }

    public int Count
    {
        get
        {
            int totalCount = 0;
            foreach (int count in this.groupCounts)
                totalCount += count;
            return totalCount;
        }
    }

    public int CountPerLine
    {
        get { return this.countPerLine; }
    }

    public ScrollRect ParentScrollRect
    {
        get { return this.parentScrollRect; }
    }

    public float CellWidth
    {
        get { return this.cellwidth; }
    }

    public float CelllHeight
    {
        get { return this.cellheight; }
    }

    public float GroupWidth
    {
        get { return this.groupWidth; }
    }

    public float GroupHeight
    {
        get { return this.groupHeight; }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // private

    [SerializeField] private ScrollRect parentScrollRect;

    [SerializeField] private float cellwidth;
    [SerializeField] private float cellheight;
    [SerializeField] private float groupWidth;
    [SerializeField] private float groupHeight;
    [SerializeField] private float bottomMargin;

    [SerializeField] private int countPerLine = 1;

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject groupPrefab;

    private RectTransform rectTransform;
    private Rect scrollRectValue;

    private int[] groupCounts;
    private OnUpdateItem onUpdateItem;
    private OnUpdateGroup onUpdateGroup;

    private int itemCount;
    private GameObject[] items;
    private GameObject[] groups;
    private float[] groupsPosList;

    private void InitData(bool keepPostion = false)
    {
        this.ClearAllChild();
        this.rectTransform = this.GetComponent<RectTransform>();
        this.scrollRectValue = this.parentScrollRect.GetComponent<RectTransform>().rect;
        int rowCount = Mathf.CeilToInt((float)Count / (float)this.countPerLine);

        if (this.parentScrollRect.vertical)
        {
            this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, this.bottomMargin + Mathf.Max(this.parentScrollRect.GetComponent<RectTransform>().sizeDelta.y, (this.cellheight * (float)rowCount + this.groupCounts.Length * this.groupHeight)));
            this.rectTransform.anchorMax = new Vector2(0.5f, 1f);
            this.rectTransform.anchorMin = new Vector2(0.5f, 1f);
            this.rectTransform.pivot = new Vector2(0.5f, 1f);
            this.itemCount = (int)(Mathf.Abs(Mathf.Ceil(this.scrollRectValue.height / this.cellheight)) + 1) * this.countPerLine;
        }
        else
        {
            this.rectTransform.sizeDelta = new Vector2(Mathf.Max(this.parentScrollRect.GetComponent<RectTransform>().sizeDelta.x, (this.cellwidth * (float)rowCount + this.groupCounts.Length * this.GroupWidth)), this.rectTransform.sizeDelta.y);
            this.rectTransform.anchorMax = new Vector2(0f, 0.5f);
            this.rectTransform.anchorMin = new Vector2(0f, 0.5f);
            this.rectTransform.pivot = new Vector2(0f, 0.5f);
            this.itemCount = (int)(Mathf.Abs(Mathf.Ceil(this.scrollRectValue.width / this.cellwidth)) + 1) * this.countPerLine;
        }


        if (!keepPostion)
            this.rectTransform.anchoredPosition = Vector2.zero;

        this.items = new GameObject[this.itemCount];

        for (int i = 0; i < this.itemCount; i++)
        {
            this.items[i] = Instantiate(this.itemPrefab) as GameObject;
            RectTransform rTrans = this.items[i].GetComponent<RectTransform>();
            rTrans.SetParent(this.transform);
            rTrans.localScale = Vector3.one;
            rTrans.localPosition = Vector3.zero;
            rTrans.anchorMax = new Vector2(0, 1);
            rTrans.anchorMin = new Vector2(0, 1);
            rTrans.pivot = new Vector2(0, 1);
            rTrans.sizeDelta = new Vector2(this.cellwidth, this.cellheight);
        }

        this.groups = new GameObject[this.groupCounts.Length];
        this.groupsPosList = new float[this.groupCounts.Length];
        for (int i = 0; i < this.groups.Length; i++)
        {
            this.groups[i] = Instantiate(this.groupPrefab) as GameObject;
            RectTransform rTrans = this.groups[i].GetComponent<RectTransform>();
            rTrans.SetParent(this.transform);
            rTrans.localScale = Vector3.one;
            rTrans.localPosition = Vector3.zero;
            rTrans.anchorMax = new Vector2(0, 1);
            rTrans.anchorMin = new Vector2(0, 1);
            rTrans.pivot = new Vector2(0, 1);
            rTrans.sizeDelta = new Vector2(this.groupWidth, this.groupHeight);
        }

        this.SetGoupPosition();
        this.UpdateItemPosition();
    }

    private void SetGoupPosition()
    {
        if (this.parentScrollRect.vertical)
        {

            float lastPosY = 0;
            for (int i = 0; i < this.groups.Length; i++)
            {
                this.groups[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -lastPosY);
                this.groupsPosList[i] = lastPosY;
                lastPosY += this.groupHeight;
                lastPosY += (this.groupCounts[i] / this.countPerLine) * this.cellheight;

                this.onUpdateGroup(i, this.groups[i]);

            }
        }
        else
        {
            float lastPosX = 0;
            for (int i = 0; i < this.groups.Length; i++)
            {
                this.groups[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-lastPosX, 0f);
                this.groupsPosList[i] = lastPosX;
                lastPosX += this.GroupWidth;
                lastPosX += (this.groupCounts[i] / this.countPerLine) * this.cellwidth;

                this.onUpdateGroup(i, this.groups[i]);
            }
        }
    }

    private void UpdateItemPosition()
    {
        if (this.parentScrollRect.vertical)
        {
            //Remove Spring area when list Dragging on each end of side.
            float min = 0f;
            float max = this.rectTransform.rect.height - this.scrollRectValue.height;
            float curPosY = Mathf.Clamp(this.rectTransform.anchoredPosition.y, min, max);


            int groupPosIndex = 0;
            for (int i = 0; i < this.groupCounts.Length; i++)
            {
                if (curPosY > (this.groupsPosList[i] + this.groupHeight))
                    groupPosIndex = i + 1;
            }

            //Start Y Position for item's position in first line
            float startPosY = Mathf.Floor(((curPosY - (groupPosIndex * groupHeight)) / this.cellheight)) * this.cellheight;
            //Start index for first item in list
            int startIndex = Mathf.FloorToInt(((curPosY - (groupPosIndex * groupHeight)) / this.cellheight)) * this.countPerLine;

            for (int i = 0; i < this.itemCount; i++)
            {
                int dataIndex = startIndex + i;

                if (!this.items[i]) continue;

                if (dataIndex < 0 || dataIndex >= this.Count)
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

                    int groupIndex = this.GetGroupIndex(dataIndex);
                    float posX = this.cellwidth * (i % this.countPerLine);
                    float posY = -(startPosY + (int)((i / this.countPerLine) * this.cellheight) + ((groupIndex + 1) * this.groupHeight));

                    this.items[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
                    UEListComponent comp = this.items[i].GetComponent<UEListComponent>();
                    if (forceUpdate || dataIndex != comp.Index)
                    {
                        comp.Group = groupIndex;
                        comp.Index = dataIndex;
                        this.onUpdateItem(groupIndex, this.GetGroupItemIndex(groupIndex, dataIndex), this.items[i]);
                    }
                }
            }
        }
        else
        {
            //Remove Spring area when list Dragging on each end of side.
            //			float min = - this.rectTransform.rect.width - this.scrollRectValue.width;
            //			float max = 0f;
            //			float curPosX = Mathf.Abs(Mathf.Clamp(this.rectTransform.anchoredPosition.x, min, max));
            //			
            //			//Start X Position for item's position in first line
            //			float startPosX = Mathf.Floor (curPosX / this.cellwidth) * this.cellwidth;
            //			//Start index for first item in list
            //			int startIndex = Mathf.FloorToInt(curPosX / this.cellwidth) * this.countPerLine;
            //
            //			for(int i = 0; i < this.itemCount; i++)
            //			{
            //				int dataIndex = startIndex + i;
            //				
            //				if(!this.items[i]) continue;
            //				
            //				if(dataIndex < 0 || dataIndex >= this.Count)
            //				{
            //					this.items[i].gameObject.SetActive(false);
            //				}
            //				else
            //				{
            //					bool forceUpdate = false;
            //					if(!this.items[i].gameObject.activeSelf)
            //					{
            //						this.items[i].gameObject.SetActive(true);
            //						forceUpdate = true;
            //					}
            //					
            //					float posX = startPosX + (int)((i / this.countPerLine) * this.cellwidth);
            //					float posY = -(this.cellheight * (i % this.countPerLine));
            //					
            //					this.items[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
            //					UEListComponent comp = this.items[i].GetComponent<UEListComponent>();
            //					if(forceUpdate || dataIndex != comp.Index)
            //					{
            //						comp.Index = dataIndex;
            //						this.onUpdateItem(dataIndex, this.items[i]);
            //					}
            //				}
            //			}
        }
    }

    private int GetGroupIndex(int itemIndex)
    {
        int groupCount = 0;
        for (int i = 0; i < this.groupCounts.Length; i++)
        {
            groupCount += this.groupCounts[i];
            if (itemIndex < groupCount)
                return i;
        }
        return 0;
    }

    private int GetGroupItemIndex(int groupIndex, int dataIndex)
    {
        for (int i = 0; i < groupIndex; i++)
        {
            dataIndex -= this.groupCounts[i];
        }
        return dataIndex;
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
        float verticalNormalizePos = this.parentScrollRect.verticalNormalizedPosition;
        while (verticalNormalizePos < 1f)
        {
            verticalNormalizePos += 0.05f;
            if (verticalNormalizePos > 1f)
                verticalNormalizePos = 1f;
            this.parentScrollRect.verticalNormalizedPosition = verticalNormalizePos;
            yield return null;
        }
        yield break;
    }
}
