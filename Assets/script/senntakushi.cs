using UnityEngine;
using UnityEngine.UI;

public class SimpleButtonSelector : MonoBehaviour
{
    public Button element1; // ボタン1
    public Button element2; // ボタン2
    private Button[] buttons; // ボタンの配列
    private int selectedIndex = 0; // 現在選択中のインデックス
    private float inputThreshold = 0.5f; // スティックの感度（0.5f以上で反応）
    private float switchDelay = 0.2f; // 選択を切り替えるまでの遅延
    private float lastSwitchTime = 0; // 最後の切り替え時間

    void Start()
    {
        buttons = new Button[] { element1, element2 };
        UpdateButtonSelection(); // 初期選択の更新
    }

    void Update()
    {
        // 左スティックの上下入力を取得
        float verticalInput = Input.GetAxis("Vertical");

        // 現在の時刻を取得
        float currentTime = Time.time;

        if (verticalInput > inputThreshold && currentTime - lastSwitchTime > switchDelay) // 上に移動
        {
            selectedIndex = (selectedIndex == 0) ? buttons.Length - 1 : selectedIndex - 1;
            lastSwitchTime = currentTime; // 切り替え時間を更新
            UpdateButtonSelection();
        }
        else if (verticalInput < -inputThreshold && currentTime - lastSwitchTime > switchDelay) // 下に移動
        {
            selectedIndex = (selectedIndex + 1) % buttons.Length;
            lastSwitchTime = currentTime; // 切り替え時間を更新
            UpdateButtonSelection();
        }

        // 決定ボタン（まるボタン）で選択
        if (Input.GetButtonDown("Fire2")) // "Fire1"はまるボタンに割り当てられていると仮定
        {
            buttons[selectedIndex].onClick.Invoke(); // 選択されたボタンのクリックを呼び出す
        }
    }

    private void UpdateButtonSelection()
    {
        // 全てのボタンを非選択状態にし、選択中のボタンを強調表示
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = (i == selectedIndex);
            // ボタンの見た目を変更する場合はここで色やスケールを変更できます
        }
    }
}
