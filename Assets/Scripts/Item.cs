using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemData itemData;
    public int itemCount;
    public int apply_count = 0;

    public Item(ItemData itemdata, int count)
    {
        this.itemData = itemdata;
        this.itemCount = count;
    }

    public void usingItem()
    {
        apply_count = itemData.duration;
    }
}
