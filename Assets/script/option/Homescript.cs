using UnityEngine;
using UnityEngine.UI;
using System.Collections; // IEnumerator を使用するために必要


public class HomeScript : MonoBehaviour
{
    [SerializeField] private Image contentImage; // 表示用のImage
    [Header("ボタン")]
    [SerializeField] private Image[] Buttons; // ボタン配列（2個登録）
    [Header("選択UI")]
    [SerializeField] private Image[] choiceUI; // 対応するUI配列（2個登録）

    private int currentIndex = 0; // 現在選択されているボタンのインデックス
    private float nextMoveTime = 0f; // 次の移動可能な時間
    private float moveCooldown = 0.2f; // 入力クールダウン時間
    private FlagManager flagManager; // フラグマネージャーのインスタンス
    private SceneTransitionManager transitionManager; // シーン遷移マネージャー
    private bool isProcessingFire1 = false;

    void Start()
    {
        contentImage.gameObject.SetActive(false);

        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        if (flagManager == null)
        {
            return;
        }

        transitionManager = FindObjectOfType<SceneTransitionManager>(); // シーン遷移マネージャーを探す
        if (transitionManager == null)
        {
            return;
        }

        UpdateUI(); // 初期状態でUIを更新
    }

    public void Jump()
    {
        contentImage.gameObject.SetActive(true);
    }

    private void Update()
    {
        // optionフラグがtrueでない場合は実行しない
        if (!flagManager.GetFlag(FlagManager.FlagType.Option))
        {
            return;
        }

        if (flagManager.GetFlag(FlagManager.FlagType.Home))
        {
            HandleHorizontalInput(); // 入力を監視
            HandleFire2Input();      // Fire2ボタンの入力処理
        }

        if (flagManager.GetFlag(FlagManager.FlagType.Home) && Input.GetButtonDown("Fire1"))
        {
            HandleFire1Input(); // Fire1の入力処理
        }
    }

    private void HandleHorizontalInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal Stick-L");

        if (Time.time >= nextMoveTime && Mathf.Abs(horizontalInput) > 0.1f)
        {
            if (horizontalInput > 0)
            {
                MoveSelection(1); // 右に移動
            }
            else if (horizontalInput < 0)
            {
                MoveSelection(-1); // 左に移動
            }
            nextMoveTime = Time.time + moveCooldown; // 次の移動時間を設定
        }
    }

    private void HandleFire2Input()
    {
        StartCoroutine(DelayedHandleFire2Input());
    }

    private IEnumerator DelayedHandleFire2Input()
    {
        yield return null; // 1フレーム待機

        if (Input.GetButtonDown("Fire2"))
        {
            // choiceUIが表示されているか確認
            if (choiceUI[currentIndex].gameObject.activeSelf)
            {
                PerformActionBasedOnSelection(); // 選択に応じたアクションを実行
            }
        }
    }


    private void PerformActionBasedOnSelection()
    {
        if (currentIndex == 0)
        {
            // element1のボタンが選択された場合
            HandleFire1Input();
        }
        else if (currentIndex == 1)
        {
            // element2のボタンが選択された場合
            transitionManager.LoadScene("title");
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Gotitle, true);
        }
    }

    private void MoveSelection(int direction)
    {
        currentIndex += direction;

        // インデックスをループさせる
        if (currentIndex >= Buttons.Length)
        {
            currentIndex = 0; // 最初に戻る
        }
        else if (currentIndex < 0)
        {
            currentIndex = Buttons.Length - 1; // 最後に戻る
        }

        UpdateUI(); // 選択状態を更新
    }

    private void UpdateUI()
    {
        // 全てのUIを非表示にする
        for (int i = 0; i < choiceUI.Length; i++)
        {
            choiceUI[i].gameObject.SetActive(false);
        }

        // 選択されているボタンに対応するUIを表示
        choiceUI[currentIndex].gameObject.SetActive(true);
    }

    private void HandleFire1Input()
    {
        if (!isProcessingFire1)
        {
            StartCoroutine(DelayedHandleFire1Input());
        }
    }

    private IEnumerator DelayedHandleFire1Input()
    {
      
        isProcessingFire1 = true; // 処理中フラグを立てる
        yield return null; // 1フレーム待機
        contentImage.gameObject.SetActive(false);
        flagManager.SetFlag(FlagManager.FlagType.Home, false);
        yield return new WaitForSeconds(0.5f); // 0.5秒のクールダウン
        isProcessingFire1 = false; // 処理完了後にフラグを解除
    }

    public void ResetHome()
    {
        // フラグをリセット
        flagManager.SetFlag(FlagManager.FlagType.Home, false);
        contentImage.gameObject.SetActive(false);  // contentImageを非表示にする

        // choiceUIを非表示にする
        foreach (var ui in choiceUI)
        {
            ui.gameObject.SetActive(false);
        }
    }

}
