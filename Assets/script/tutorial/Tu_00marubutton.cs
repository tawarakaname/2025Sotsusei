using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu_00marubutton : MonoBehaviour
{
    [SerializeField] private Collider firstcollider; // 初期コライダー
    [SerializeField] private Animator UIAnimator; // アニメーション用アニメーター

    private TextManager textManager; // TextManagerへの参照
    private string currentKeyword; // 現在のコライダーに対応するキーワード
    private bool isTextboxActive; // Textboxが現在アクティブかどうか
    private bool isPlayerInCollider; // プレイヤーがコライダー内にいるかどうか

    void Start()
    {
    

        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
        isTextboxActive = false; // 初期状態は非アクティブ
        isPlayerInCollider = false; // プレイヤーがコライダーにいない状態
        firstcollider.enabled = false;
    }

    private void Update()
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Be_Aclear))
        {
            return;
        }
        // Greetingフラグがfalseの場合、処理を終了
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Greeting))
        {
            return;
        }
        // Tu_00clearフラグがtrueの場合、このスクリプトを無効化
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_00clear))
        {
            return;
        }

        // Greetingフラグがtrueの場合、コライダーを有効化
        EnableTu_00Collider();

        // プレイヤーがコライダー内にいる場合、Textboxの状態を確認
        if (isPlayerInCollider)
        {

            // Textboxがアクティブでなく、かつ現在のキーワードが設定されている場合に表示処理
            if (Input.GetButtonDown("Fire2") && currentKeyword != null
                && FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                textManager.DisplayCurrentLine();

            }

            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                isTextboxActive = true; // Textboxがアクティブ
            }
            else if (isTextboxActive)
            {
                // Textboxが終了したタイミングで処理を実行
                isTextboxActive = false;
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Tu_00clear, true); // clearフラグを設定
                DisableTu_00Collider(); // スクリプトとコライダーを無効化
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // "Player"タグを持つオブジェクトのみ処理する
        if (other.CompareTag("Player"))
        {
            isPlayerInCollider = true; // プレイヤーがコライダー内にいる状態

            currentKeyword = "Tu_00";

            // 初回のみテキスト表示を許可
            if (currentKeyword != null && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox)
                && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_00clear))
            {
                OnClickTu_00This();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // プレイヤーがコライダーを出た場合、状態をリセット
        if (other.CompareTag("Player"))
        {
            isPlayerInCollider = false; // プレイヤーがコライダー外に出た状態
        }
    }

    public void OnClickTu_00This()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
        // アニメーションを再生
        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("maru");
        }
    }

    private void EnableTu_00Collider()
    {
        // firstcollider を有効化
        if (firstcollider != null)
        {
            firstcollider.enabled = true;
        }
    }

    private void DisableTu_00Collider()
    {
        // コライダーを無効化
        if (firstcollider != null)
        {
            firstcollider.enabled = false;
        }
        // "marudefault" トリガーを呼び出す
        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("marudefault");
        }

    }
}