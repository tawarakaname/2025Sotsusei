using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIChoices : MonoBehaviour
{
    private FlagManager flagManager;
    public GameObject UICanvas;
    private int currentPosition = 0; // 0: UIFirstButton, 1: UISecondButton
    public GameObject UIFirstButton, UISecondButton;
    public GameObject ChoiceBGpanel1, ChoiceBGpanel2;

    private float nextMoveTime; // 次の移動を許可する時間

    void Start()
    {
        flagManager = FlagManager.Instance;

        // 初期状態でUIキャンバスを非表示にする
        UICanvas.SetActive(false);
        ChoiceBGpanel1.SetActive(false);
        ChoiceBGpanel2.SetActive(false);
    }

    private void Update()
    {
        // UIキャンバスがアクティブでない場合、Fire2の入力を受け付ける
        if (!flagManager.GetFlag(FlagManager.FlagType.UICanvas))
        {
            if (Input.GetButtonDown("Fire2")) // Fire2ボタンが押された時
            {
                ToggleUICanvas();
            }
            return; // UIが非アクティブのときは残りの処理をスキップ
        }

        // 上下入力を取得
        float verticalInput = Input.GetAxisRaw("Vertical Stick-L");

        // 現在の時間が nextMoveTime より大きい場合に移動を許可
        if (Time.time >= nextMoveTime && verticalInput != 0)
        {
            if (verticalInput > 0)
            {
                ShiftSlotUp(); // 上に移動
            }
            else if (verticalInput < 0)
            {
                ShiftSlotDown(); // 下に移動
            }

            nextMoveTime = Time.time + 0.2f; // 0.2秒後に次の移動を許可
        }

        HandleSelection(); // 選択処理
    }

    private void ShiftSlotUp()
    {
        // 現在の選択を非表示
        HideChoiceBGPanel(currentPosition);

        // 上に移動
        currentPosition = (currentPosition - 1 + 2) % 2; // 0か1に戻す

        // 新しいスロットを選択
        OnSelectSlot(currentPosition);
    }

    private void ShiftSlotDown()
    {
        // 現在の選択を非表示
        HideChoiceBGPanel(currentPosition);

        // 下に移動
        currentPosition = (currentPosition + 1) % 2; // 0か1に戻す

        // 新しいスロットを選択
        OnSelectSlot(currentPosition);
    }

    private void HideChoiceBGPanel(int position)
    {
        // 選択されたパネルを非表示
        if (position == 0)
        {
            ChoiceBGpanel1.SetActive(false);
        }
        else
        {
            ChoiceBGpanel2.SetActive(false);
        }
    }

    private void OnSelectSlot(int position)
    {
        // 新しい選択されたボタンに応じてパネルを表示
        if (position == 0)
        {
            ChoiceBGpanel1.SetActive(true);
            EventSystem.current.SetSelectedGameObject(UIFirstButton);
        }
        else
        {
            ChoiceBGpanel2.SetActive(true);
            EventSystem.current.SetSelectedGameObject(UISecondButton);
        }
    }

    public void ToggleUICanvas()
    {
        bool isActive = UICanvas.activeInHierarchy;
        UICanvas.SetActive(!isActive);
        Time.timeScale = isActive ? 1f : 0f; // タイムスケールを変更
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(UIFirstButton);

        // プレイヤーの移動フラグを設定
        flagManager.SetFlag(FlagManager.FlagType.UICanvas, !isActive); // キャンバスが開かれた時にフラグを設定
    }

    private void HandleSelection()
    {
        if (Input.GetButtonDown("Fire1")) // 丸ボタンが押された場合
        {
            switch (currentPosition)
            {
                case 0:
                    // ボタン1に対応する処理
                    Debug.Log("Button 1 Selected");
                    break;

                case 1:
                    // ボタン2に対応する処理
                    Debug.Log("Button 2 Selected");
                    break;

                default:
                    break;
            }
        }
    }
}
