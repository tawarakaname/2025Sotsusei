using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu_05itemuse : MonoBehaviour
{
    [SerializeField] private Collider CauldronCollider; // ゴミ箱コライダー
    [SerializeField] private Animator UIAnimator; // アニメーション用アニメーター

    private TextManager textManager; // TextManagerへの参照
    private string currentKeyword; // 現在のコライダーに対応するキーワード
    private bool isTextboxActive; // Textboxが現在アクティブかどうか
    private bool isPlayerInCollider; // プレイヤーがコライダー内にいるかどうか
    [SerializeField] private SetObj setObj;

    // Start is called before the first frame update
    void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
        setObj.IsFreeInteract = false;
    }

    private void Update()
    {

        // 指定のフラグが全てtrueの場合、Textboxを呼び出す
        if (isPlayerInCollider &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_04clear) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.havecaudlonkey) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.NabeCamera) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_05clear))
        {
            // Textboxが表示されていない場合、キーワードに基づいて表示
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                currentKeyword = "Tu_05";  // 現在のキーワードを設定
                OnClickTu_05This();        // Textbox表示処理を実行
            }
        }

        // Fire2入力がある場合にテキストを表示
        if (Input.GetButtonDown("Fire2") && currentKeyword != null &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_05clear) && isPlayerInCollider)
        {
            textManager.DisplayCurrentLine();
        }

        // Textboxが終了したらフラグを更新
        if (isPlayerInCollider &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_04clear) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.havecaudlonkey) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.NabeCamera) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox) == false &&
            isTextboxActive)
        {
            isTextboxActive = false; // Textboxの終了を検知
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Tu_05clear, true); // Tu_05clearフラグを設定
            setObj.IsFreeInteract = true;
            DisableTu_05collider(); // スクリプトとコライダーを無効化
        }

        // Textboxがアクティブなら状態を記録
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            isTextboxActive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // "Player"タグを持つオブジェクトのみ処理する
        if (other.CompareTag("Player"))
        {
            isPlayerInCollider = true; // プレイヤーがコライダー内にいる状態
        }
    }

    public void OnClickTu_05This()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
        // アニメーションを再生
        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("maru");
        }
    }

    private void DisableTu_05collider()
    {
        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("marudefault");
        }

        this.enabled = false; // このスクリプトを無効化
    }
}
