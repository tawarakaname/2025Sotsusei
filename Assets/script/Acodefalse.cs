using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acodefalse : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] scripts; // 無効化したいスクリプトをアタッチ
    [SerializeField] private Collider targetCollider; // 無効化したいコライダーをアタッチ

    // Update is called once per frame
    void Update()
    {
        // Adooropen フラグが true になったらスクリプトとコライダーを無効化
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen))
        {
            // scripts または targetCollider が設定されていない場合は処理を中断
            if (scripts == null || scripts.Length == 0 || targetCollider == null) return;

            DisableAttachedScripts();
            DisableCollider();
        }
    }

    // アタッチされているスクリプトをすべて無効化するメソッド
    private void DisableAttachedScripts()
    {
        foreach (MonoBehaviour script in scripts)
        {
            if (script != null)
            {
                script.enabled = false;
            }
        }
    }

    // 指定されたコライダーを無効化するメソッド
    private void DisableCollider()
    {
        if (targetCollider != null)
        {
            targetCollider.enabled = false;
        }
    }
}
