using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtoB_door_open : MonoBehaviour
{
    public GameObject objWithAnimation; // アニメーションがついているオブジェクト
    private Animator animator;
    [SerializeField] private Collider CtoBdoorcollider;

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
        if (other.CompareTag("Player"))
        {
            // プレイヤーがコライダーに入った瞬間にアニメーションを再生
            OnClickCtoBdoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーがコライダーから出た瞬間に"閉じる"アニメーションを再生
            OnCloseCtoBdoor();

            // アニメーションをリセット（もし開くアニメーションが再生中なら閉じる状態に戻す）
            animator.ResetTrigger("BtoA_open"); // "開く"トリガーをリセット
        }
    }

    public void OnClickCtoBdoor()
    {
        animator.SetTrigger("BtoA_open"); // "開く"トリガーを設定してアニメーション再生
    }

    public void OnCloseCtoBdoor()
    {
        animator.SetTrigger("BtoA_close"); // "閉じる"トリガーを設定してアニメーション再生
    }
}