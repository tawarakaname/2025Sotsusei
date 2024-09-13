using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itembox : MonoBehaviour
{
    [SerializeField] private Slot[] slots;
    [SerializeField] private Slot selectedSlot = null;
    [SerializeField] private Text notificationText;

    private int currentPosition = 0;  // 現在選択されているスロットの位置
    private bool canMove = true;      // スティック操作の間隔を管理するためのフラグ
    private FlagManager flagManager;

    public static Itembox instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            slots = GetComponentsInChildren<Slot>();
            flagManager = FindObjectOfType<FlagManager>(); // 追加: FlagManager のインスタンスを取得
        }
        else
        {
            Destroy(gameObject);
        }

        // 初期状態でテキストを非表示にする
        if (notificationText != null)
        {
            notificationText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // flagManagerがnullでないことを確認する
        if (flagManager == null)
        {
            Debug.LogError("FlagManager is not assigned.");
            return;
        }

        // Itemboxフラグがtrueでない場合は処理しない
        if (!flagManager.GetFlag(FlagManager.FlagType.itembox))
        {
            return; // Itemboxフラグがfalseなら何もせずUpdateを終了
        }

        // スティックの入力を取得
        float horizontalInput = Input.GetAxisRaw("Horizontal Stick-L");

        // 入力があり、かつスティックを操作できる状態であれば
        if (canMove && horizontalInput != 0)
        {
            if (horizontalInput > 0)
            {
                ShiftSlotRight(); // 右に移動
            }
            else if (horizontalInput < 0)
            {
                ShiftSlotLeft();  // 左に移動
            }

            // スティックを動かしている間は再度操作ができないようにする
            canMove = false;
            Invoke(nameof(ResetCanMove), 0.2f);  // コルーチンをInvokeで置き換え
        }
    }

    private void ShiftSlotRight()
    {
        selectedSlot?.HideBGPanel();  // 現在選択されているスロットを非表示

        // アイテムが設定されているスロットを探す
        currentPosition = GetNextSlotPosition(currentPosition, 1);

        // 新しいスロットを選択
        OnSelectSlot(currentPosition);
    }

    private void ShiftSlotLeft()
    {
        selectedSlot?.HideBGPanel();  // 現在選択されているスロットを非表示

        // アイテムが設定されているスロットを探す
        currentPosition = GetNextSlotPosition(currentPosition, -1);

        // 新しいスロットを選択
        OnSelectSlot(currentPosition);
    }

    // スロットを飛び越えて次のアイテムがあるスロットの位置を取得
    private int GetNextSlotPosition(int startPos, int direction)
    {
        int newPosition = startPos;
        do
        {
            newPosition += direction;
            if (newPosition >= slots.Length) newPosition = 0;
            if (newPosition < 0) newPosition = slots.Length - 1;
        } while (!slots[newPosition].HasItem());

        return newPosition;
    }

    private void ResetCanMove()
    {
        canMove = true;
    }

    // Pickupobjがクリックされたらスロットにアイテムを入れる
    public void SetItem(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(item);
                break;
            }
        }
    }

    public void OnSelectSlot(int position)
    {
        // すべてのスロットの選択パネルを非表示
        foreach (Slot slot in slots)
        {
            slot.HideBGPanel();
        }
        selectedSlot = null;

        // 選択されたスロットの選択パネルを表示
        if (slots[position].OnSelected())
        {
            selectedSlot = slots[position];
        }
    }

    // アイテムの使用を試みる＆使えるなら使ってしまう
    public bool TryUseItem(Item.Type type)
    {
        if (selectedSlot == null)
        {
            return false;
        }

        if (selectedSlot.GetItem().type == type)
        {
            selectedSlot.SetItem(null);
            selectedSlot.HideBGPanel();
            selectedSlot = null;
            return true;
        }
        else
        {
            // アイテムが間違っている場合にテキストを表示
            if (notificationText != null)
            {
                notificationText.text = "このアイテムではないみたい";
                notificationText.gameObject.SetActive(true); // テキストを表示
            }
            return false;
        }
    }

    public Item GetSelectedItem()
    {
        return selectedSlot?.GetItem();
    }
}
