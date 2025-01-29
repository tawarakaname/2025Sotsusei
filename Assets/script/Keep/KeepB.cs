using UnityEngine;

public class KeepB : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // 対象のGameObjectをInspectorで設定

    void Start()
    {
        if (FlagManager.Instance.GetFlagByType(Item.Type.BTB))
        {
            targetObject.SetActive(true);
        }
    }
}
