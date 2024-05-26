using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObj : MonoBehaviour
{
    [SerializeField] Item.Type itemType;
    Item item;

    private void Start()
    {
        // itemタイプに応じてitemを作成する
        item = ItemGenerater.instance.Spawn(itemType);
    }
    // クリックしたらログを出す
    // クリックしたらobjを消す

    public void OnClickObj()
    {
        Debug.Log("拾う");

        Itembox.instance.SetItem(item);
        gameObject.SetActive(false);
    }


}
