using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PurchaseProductID
{

}

[System.Serializable]
public class PurchaseProductData
{
    public PurchaseProductData() { }

    public PurchaseProductData(string id, string time, string receipt)
    {
        this.id = id;
        this.time = time;
        this.receipt = receipt;
    }

    public string id;
    public string time;
    public string receipt;
}
