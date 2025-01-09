using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu_02camerazoom : MonoBehaviour
{
    [SerializeField] private Collider CauldronCollider; // ゴミ箱コライダー

    private TextManager textManager; // TextManagerへの参照
    private string currentKeyword; // 現在のコライダーに対応するキーワード
    private bool isTextboxActive; // Textboxが現在アクティブかどうか
    private bool isPlayerInCollider; // プレイヤーがコライダー内にいるかどうか

    // Start is called before the first frame update
    void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
    }

    private void Update()
    {
        // Belt1moveフラグがtrueの場合、このスクリプトとコライダーを無効化
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_02clear))
        {
            DisableTu_02collider();
            return;
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
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Tu_02clear, true);
                DisableTu_02collider(); // スクリプトとコライダーを無効化
            }
        }

        // 指定のフラグが全てtrueの場合、Textboxを呼び出す
        if (isPlayerInCollider &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_02clear) &&  // Tu_02clear が false
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_01clear) &&   // Tu_01clear が true
            FlagManager.Instance.GetFlag(FlagManager.FlagType.NabeCamera))    // NabeCamera が true
        {
            // Textboxを呼び出す
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                currentKeyword = "Tu_02";  // 現在のキーワードを設定
                OnClickTu_02This();        // Textbox表示処理を実行
            }
        }

        // Fire2入力がある場合にフラグとキーワードをチェック
        if (Input.GetButtonDown("Fire2") && currentKeyword != null && FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            textManager.DisplayCurrentLine();
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

    public void OnClickTu_02This()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }

    // 1秒後に処理を実行するコルーチン
    private IEnumerator DisableTu_02colliderWithDelay()
    {
        yield return new WaitForSeconds(1f); // 1秒待つ
        this.enabled = false; // このスクリプトを無効化
    }

    private void DisableTu_02collider()
    {
        // コルーチンを開始して1秒後に処理を実行
        StartCoroutine(DisableTu_02colliderWithDelay());
    }
}
