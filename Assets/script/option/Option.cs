using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [Header("図鑑、操作方法、ホーム")]
    [SerializeField] private Image[] selectButtons; // 図鑑、操作方法、ホームのボタン

    [Header("背景イメージ1")]
    [SerializeField] private Image[] bgImages; // 背景イメージ1

    [Header("背景イメージ2")]
    [SerializeField] private Image[] bgImages2; // 背景イメージ2

    [Header("ジャンプ対象スクリプト")]
    [SerializeField] private MonoBehaviour[] targetScripts; // 対象スクリプト (例: ZukanScript, OperationScript, HomeScript)

    private FlagManager flagManager;
    private int currentIndex = 0; // 現在選択中のボタンのインデックス
    private float nextMoveTime = 0f; // 次の移動可能な時間
    private const float moveCooldown = 0.2f; // 移動のクールダウン時間

    // Start is called before the first frame update
    void Start()
    {
        flagManager = FindObjectOfType<FlagManager>();
        UpdateSelectionUI(); // 初期状態のUIを更新
    }

    // Update is called once per frame
    void Update()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.Option))
        {
            HandleHorizontalInput(); // 横方向の入力処理
            HandleFire2Input();      // Fire2ボタンの入力処理
        }
    }

    // 横方向の入力処理
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
        if (Input.GetButtonDown("Fire2"))

        { // 選択されているボタンのGameObjectを取得
            GameObject selectedObject = selectButtons[currentIndex].gameObject;

            // ZukanScriptがアタッチされていればJumpメソッドを呼び出し
            ZukanScript zukanScript = selectedObject.GetComponent<ZukanScript>();
            if (zukanScript != null)
            {
                zukanScript.Jump();
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Zukan, true);
            }

            // OperationScriptがアタッチされていればJumpメソッドを呼び出し
            OperationScript operationScript = selectedObject.GetComponent<OperationScript>();
            if (operationScript != null)
            {
                operationScript.Jump();
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Operation, true);
            }

            // HomeScriptがアタッチされていればJumpメソッドを呼び出し
            HomeScript homeScript = selectedObject.GetComponent<HomeScript>();
            if (homeScript != null)
            {
                homeScript.Jump();
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Home, true);
            }
        }
    }


    // 選択を移動
    private void MoveSelection(int direction)
    {
        currentIndex = (currentIndex + direction + selectButtons.Length) % selectButtons.Length; // インデックスを更新
        UpdateSelectionUI(); // UIを更新
    }

    // 選択中のUIを更新
    private void UpdateSelectionUI()
    {
        for (int i = 0; i < bgImages.Length; i++)
        {
            if (i == currentIndex)
            {
                bgImages[i].gameObject.SetActive(true); // 選ばれている要素の背景1を表示
                bgImages2[i].gameObject.SetActive(true); // 選ばれている要素の背景2を表示
            }
            else
            {
                bgImages[i].gameObject.SetActive(false); // 選ばれていない要素の背景1を非表示
                bgImages2[i].gameObject.SetActive(false); // 選ばれていない要素の背景2を非表示
            }
        }
    }
}
