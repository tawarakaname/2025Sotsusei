using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtoA_door_open : MonoBehaviour
{
    public GameObject objWithAnimation; // アニメーションがついているオブジェクト
    private Animator animator;
    [SerializeField] private Collider BtoAdoorcollider;

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
            animator.ResetTrigger("BtoA_open"); // "開く"トリガーをリセット
        }
    }

    public void OnClickBtoAdoor()
    {
        animator.SetTrigger("BtoA_open"); // "開く"トリガーを設定してアニメーション再生
    }

    public void OnCloseBtoAdoor()
    {
        animator.SetTrigger("BtoA_close"); // "閉じる"トリガーを設定してアニメーション再生
    }
}
