using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu_04itemget : MonoBehaviour
{
    [SerializeField] private Collider CauldronCollider; // ゴミ箱コライダー
    [SerializeField] private Animator UIAnimator; // アニメーション用アニメーター

    private TextManager textManager; // TextManagerへの参照
    private string currentKeyword; // 現在のコライダーに対応するキーワード
    private bool isTextboxActive; // Textboxが現在アクティブかどうか
    private bool isPlayerInCollider; // プレイヤーがコライダー内にいるかどうか
    private bool hasCauldronColliderBeenEnabled = false; // コライダーが一度有効化されたかどうか


    // Start is called before the first frame update
    void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
    }
    // Update is called once per frame
    void Update()
    {
        // Fire2入力がある場合にフラグとキーワードをチェック
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.havecaudlonkey) && !hasCauldronColliderBeenEnabled)
        {
            if (CauldronCollider != null)
            {
                CauldronCollider.enabled = true; // コライダーを有効化
                hasCauldronColliderBeenEnabled = true; // 一度だけ実行されるようにフラグを立てる
            }
        }

        // 指定のフラグが全てtrueの場合、Textboxを呼び出す
        if (isPlayerInCollider &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_04clear) &&  
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_03clear) &&  
            FlagManager.Instance.GetFlag(FlagManager.FlagType.havecaudlonkey))   
        {
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                currentKeyword = "Tu_04";  // 現在のキーワードを設定
                OnClickTu_04This();        // Textbox表示処理を実行
            }
        }

        // Fire2入力がある場合にフラグとキーワードをチェック
        if (Input.GetButtonDown("Fire2") && currentKeyword != null && FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox)
             && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_04clear) && isPlayerInCollider)
        {
            textManager.DisplayCurrentLine();
        }

        // プレイヤーがコライダー内にいる場合、Textboxの状態を確認
        if (isPlayerInCollider)
        {
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                isTextboxActive = true; // Textboxがアクティブ
            }
            else if (isTextboxActive)
            {
                // Textboxが終了したタイミングで処理を実行
                isTextboxActive = false;
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Tu_04clear, true);
                DisableTu_04collider(); // スクリプトとコライダーを無効化
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // "Player"タグを持つオブジェクトかつ Tu_03clear が true の場合のみ処理する
        if (other.CompareTag("Player") && FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_03clear))
        {
            isPlayerInCollider = true; // プレイヤーがコライダー内にいる状態
        }
    }


    public void OnClickTu_04This()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
        // アニメーションを再生
        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("batu");
        }
    }

    // 1秒後に処理を実行するコルーチン
    private IEnumerator DisableTu_04colliderWithDelay()
    {
        yield return new WaitForSeconds(1f); // 1秒待つ
                                             // アニメーションを再生
        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("batudefault");
        }
        this.enabled = false; // このスクリプトを無効化
    }


    private void DisableTu_04collider()
    {
        // コルーチンを開始して1秒後に処理を実行
        StartCoroutine(DisableTu_04colliderWithDelay());

    }
}
