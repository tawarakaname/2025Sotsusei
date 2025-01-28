using System.Collections;
using UnityEngine;

public class Tu_03pickupitem : MonoBehaviour
{
    [SerializeField] private Animator UIAnimator; // アニメーション用アニメーター

    [SerializeField] private Collider itemCollider; // itemコライダー
    [SerializeField] private Collider Tu_03pickupCollider; // コライダーAを指定する
    [SerializeField] GameObject targetImage; // 表示・非表示を制御するImage

    private TextManager textManager; // TextManagerへの参照
    private string currentKeyword; // 現在のコライダーに対応するキーワード
    private bool isTextboxActive; // Textboxが現在アクティブかどうか
    private bool isPlayerInCollider; // プレイヤーがコライダー内にいるかどうか

    // Start is called before the first frame update
    void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
        isTextboxActive = false; // 初期状態は非アクティブ
        isPlayerInCollider = false; // プレイヤーがコライダーにいない状態
        itemCollider.enabled = false;
        if (targetImage == null)
        {
            return;
        }
        else
        {
            targetImage.SetActive(false);
        }
    }

    private void Update()
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_025clear))
        {
            if (Tu_03pickupCollider != null)
            {
                Tu_03pickupCollider.enabled = true; 
            }
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
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Tu_03clear, true);
                DisableTu_03collider(); // スクリプトとコライダーを無効化

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
        if (targetImage != null)
        {
            targetImage.SetActive(true);
        }

        // "Player"タグを持つオブジェクトのみ処理する
        if (other.CompareTag("Player"))
        {
            isPlayerInCollider = true; // プレイヤーがコライダー内にいる状態

            currentKeyword = "Tu_03";

            if (currentKeyword != null && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox)
                 && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_03clear))
            {
                OnClickTu_03This();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetImage != null)
        {
            targetImage.SetActive(false);
        }
        // プレイヤーがコライダーを出た場合、状態をリセット
        if (other.CompareTag("Player"))
        {
            isPlayerInCollider = false; // プレイヤーがコライダー外に出た状態
        }
    }

    public void OnClickTu_03This()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("maru");
        }
    }

    // 1秒後に処理を実行するコルーチン
    private IEnumerator DisableTu_03colliderWithDelay()
    {
        yield return new WaitForSeconds(1f); // 1秒待つ

        // Tu_03pickupCollider を無効化
        if (Tu_03pickupCollider != null)
        {
            Tu_03pickupCollider.enabled = false; // コライダーを無効化
        }

        // itemCollider を有効化
        if (itemCollider != null)
        {
            itemCollider.enabled = true; // ゴミ箱コライダーを有効化
        }
        targetImage.SetActive(false);

        this.enabled = false; // このスクリプトを無効化
    }

    private void DisableTu_03collider()
    {
        // コルーチンを開始して1秒後に処理を実行
        StartCoroutine(DisableTu_03colliderWithDelay());

        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("marudefault");
        }
    }
}
