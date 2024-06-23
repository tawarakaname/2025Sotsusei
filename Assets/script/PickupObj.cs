using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObj : MonoBehaviour
{
    [SerializeField] Item.Type itemType;
    [SerializeField] FlagManager.FlagType flagToSet;
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

        Itembox.instance.SetItem(item);
        gameObject.SetActive(false);

        // フラグを設定する
        FlagManager.Instance.SetFlag(flagToSet, true);
        Debug.Log("FlagON");
    }


}
