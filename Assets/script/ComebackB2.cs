using UnityEngine;

public class ComebackB2 : MonoBehaviour
{
    //やっぱりこっちがcomeback1にしました

    [SerializeField] private GameObject targetObject;   // 位置を移動させたいオブジェクト
    [SerializeField] private Vector3 targetPosition;    // 移動先の位置
    [SerializeField] private Collider triggerCollider;


    private bool isDoorOpened = false; // ドアが開いたかどうかのフラグ


    // Update is called once per frame
    void Update()
    {
     
        // comebackAフラグがtrueになった場合にオブジェクトを移動
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackB) && !isDoorOpened)
        {
            MoveTargetObject();

        }
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }
    }

    // 指定した位置にオブジェクトを移動させるメソッド
    private void MoveTargetObject()
    {
        if (targetObject != null)
        {
            targetObject.transform.position = targetPosition;
            isDoorOpened = true; // ドアが開いたことを記録
        }
    }
}
