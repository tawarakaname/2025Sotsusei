using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerater : MonoBehaviour
{
    [SerializeField] ItemListEntity itemListEntity;

    //どこでも実行できるやつ(staticはどこでも引っ張っていける変数）
    public static ItemGenerater instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Item Spawn(Item.Type type)
    {
        foreach (Item item in itemListEntity.itemList)
        {
            if (item.type == type)
            {
                // 必要に応じて他のプロパティを使用
                return new Item(item.type, item.sprite, item.zoomObj, item.zoomsprite);
            }
        }
        return null;
    }



    public Sprite GetZoomItem(Item.Type type)
    {
        foreach(Item item in itemListEntity.itemList)
        {
            if(item.type == type)
            {
                return item.zoomObj;
            }
        }
        return null;
    }
}
  