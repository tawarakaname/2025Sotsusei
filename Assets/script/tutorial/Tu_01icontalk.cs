using UnityEngine;
using System.Collections;

public class Tu_01icontalk : MonoBehaviour
{
    [SerializeField] private Animator UIAnimator; // アニメーション用アニメーター

    [SerializeField] private Collider CauldronCollider; // ゴミ箱コライダー
    [SerializeField] private Collider Tu_01iconCollider; // コライダーAを指定する
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
        CauldronCollider.enabled = false;
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
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Tu_01clear, true);
                DisableTu_01collider(); // スクリプトとコライダーを無効化
                                        // アニメーションを再生
              
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

            currentKeyword = "Tu_01";

            if (currentKeyword != null && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox)
                 && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_01clear))
            {
                OnClickTu_01This();
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

    public void OnClickTu_01This()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("maru");
        }
    }

    // 1秒後に処理を実行するコルーチン
    private IEnumerator DisableTu_01colliderWithDelay()
    {
        yield return new WaitForSeconds(1f); // 1秒待つ

        // Tu_01iconCollider を無効化
        if (Tu_01iconCollider != null)
        {
            Tu_01iconCollider.enabled = false; // コライダーを無効化
        }

        // CauldronCollider を有効化
        if (CauldronCollider != null)
        {
            CauldronCollider.enabled = true; // ゴミ箱コライダーを有効化
        }
        targetImage.SetActive(false);

        this.enabled = false; // このスクリプトを無効化
    }

    private void DisableTu_01collider()
    {
        // コルーチンを開始して1秒後に処理を実行
        StartCoroutine(DisableTu_01colliderWithDelay());

        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("marudefault");
        }
       
    }
}
