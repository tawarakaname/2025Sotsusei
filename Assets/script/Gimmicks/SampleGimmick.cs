using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleGimmick : MonoBehaviour
{
    //やりたいこと
    //アイテムcubeを持っている状態で、クリックすると消える
    //　・クリック判定
    //　アイテム持ってますよ判定

    //ここでどのアイテムが適用できるか外部から指定できるコード
    [SerializeField] Item.Type clearItem;

    public void OnClickObj()
    {
        Debug.Log("クリックしたよ！");
        //アイテムCubeを持っているかどうか
        //clearでオブジェクトを消す
        bool clear = Itembox.instance.TryUseItem(clearItem);
        if(clear == true)
        {
            Debug.Log("ギミック解除");
            gameObject.SetActive(false);
        }

    }
}
