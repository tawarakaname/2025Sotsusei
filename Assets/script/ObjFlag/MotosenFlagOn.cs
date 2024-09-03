using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MotosenFlagOn : MonoBehaviour
{
    [SerializeField] Collider triggerCollider; // 追加したコライダーのフィールド
    private bool playerInsideCollider = false;

    
    void Update()
    {
        if (playerInsideCollider)
        {
            // PS4コントローラーの⚪︎ボタンは「Fire2」として認識されます
            if (Input.GetButtonDown("Fire2"))
            {
                OnClickObj();

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;
        }
    }

    public void OnClickObj()
    {
        // フラグを設定する
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Motosen, true);
    }
}
