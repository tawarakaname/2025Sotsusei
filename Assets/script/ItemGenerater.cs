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
        CheckSingleton();
        // シングルトンパターンを適用
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 既に存在している場合は破棄する
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

    private void CheckSingleton()
    {
        var target = GameObject.FindGameObjectWithTag(gameObject.tag);
        var checkResult = target != null && target != gameObject;

        if (checkResult)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

}
