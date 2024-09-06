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
            StartCoroutine(ResetCanMove());
        }
    }


    private void ShiftSlotRight()
    {
        // 現在選択されているスロットを非表示
        if (selectedSlot != null)
        {
            selectedSlot.HideBGPanel();
        }

        // アイテムが設定されているスロットを探す
        do
        {
            currentPosition++;
            if (currentPosition >= slots.Length)
            {
                currentPosition = 0; // リストの最後に達したら最初に戻る
            }
        } while (!slots[currentPosition].HasItem()); // アイテムがないスロットをスキップ

        // 新しいスロットを選択
        OnSelectSlot(currentPosition);
    }

    private void ShiftSlotLeft()
    {
        // 現在選択されているスロットを非表示
        if (selectedSlot != null)
        {
            selectedSlot.HideBGPanel();
        }

        // アイテムが設定されているスロットを探す
        do
        {
            currentPosition--;
            if (currentPosition < 0)
            {
                currentPosition = slots.Length - 1; // リストの最初に達したら最後に戻る
            }
        } while (!slots[currentPosition].HasItem()); // アイテムがないスロットをスキップ

        // 新しいスロットを選択
        OnSelectSlot(currentPosition);
    }

    private IEnumerator ResetCanMove()
    {
        // 一定時間操作を待機（例: 0.2秒待機）
        yield return new WaitForSeconds(0.2f);
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
        // 選択スロットがあるか
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
