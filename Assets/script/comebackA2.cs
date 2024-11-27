using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comebackA2 : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;   // 位置を移動させたいオブジェクト
    [SerializeField] private Vector3 targetPosition;    // 移動先の位置
    [SerializeField] private Collider triggerCollider;
  

    private bool isDoorOpened = false; // ドアが開いたかどうかのフラグ

 
    // Update is called once per frame
    void Update()
    {
        // BTBフラグがfalseの場合、一切動作しない
        if (!FlagManager.Instance.GetFlagByType(Item.Type.BTB))
        {
            return; // Updateメソッドを早期終了
        }

        if (triggerCollider != null)
        {
            triggerCollider.enabled = true;
        }
        // comebackAフラグがtrueになった場合にオブジェクトを移動
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA) && !isDoorOpened)
        {
            MoveTargetObject();
           
        }
    }

    // 指定した位置にオブジェクトを移動させるメソッド
    private void MoveTargetObject()
    {
        if (targetObject != null)
        {
            targetObject.transform.position = targetPosition;
            isDoorOpened = true; // ドアが開いたことを記録
            Debug.Log("2かいめ");
        }
    }
}
