using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtoB_door_open : MonoBehaviour
{
    public GameObject objWithAnimation; // アニメーションがついているオブジェクト
    private Animator animator;
    [SerializeField] private Collider AtoBdoorcollider;

    void Start()
    {
        // Animatorコンポーネントを取得
        if (objWithAnimation != null)
        {
            animator = objWithAnimation.GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA))
        {
            // プレイヤーがコライダーに入った瞬間にアニメーションを再生（条件が満たされた場合）
            OnClickBtoAdoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーがコライダーから出た瞬間に"閉じる"アニメーションを再生
            OnCloseBtoAdoor();

            // アニメーションをリセット（もし開くアニメーションが再生中なら閉じる状態に戻す）
            animator.ResetTrigger("AtoB_open"); // "開く"トリガーをリセット
        }
    }

    public void OnClickBtoAdoor()
    {
        animator.SetTrigger("AtoB_open"); // "開く"トリガーを設定してアニメーション再生
    }

    public void OnCloseBtoAdoor()
    {
        animator.SetTrigger("AtoB_close"); // "閉じる"トリガーを設定してアニメーション再生
    }
}
