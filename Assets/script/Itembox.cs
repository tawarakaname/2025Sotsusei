using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itembox : MonoBehaviour
{
    [SerializeField] private Slot[] slots;
    [SerializeField] private Slot selectedSlot = null;
    [SerializeField] private SelectedItem selectedItemPanel;


    private int currentPosition = 0;  // 現在選択されているスロットの位置
    private FlagManager flagManager;
    private float nextMoveTime = 0f; // 次に移動できる時間を記録

    public static Itembox instance { get; private set; }

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            // FlagManager のインスタンスを取得
            flagManager = FindObjectOfType<FlagManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (slots == null || slots.Length == 0)
        {
            slots = GetComponentsInChildren<Slot>();
        }
        // Start メソッドでは flagManager を再設定しない
        // flagManager = FlagManager.Instance;
    }

    private void Update()
    {
        bool isItemboxFlagOn = flagManager.GetFlag(FlagManager.FlagType.itembox);
        bool isZoomPanelFlagOn = flagManager.GetFlag(FlagManager.FlagType.zoompanel);

        if (!isItemboxFlagOn || isZoomPanelFlagOn)
        {
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal Stick-L");

        // 現在の時間が nextMoveTime より大きい場合に移動を許可
        if (Time.time >= nextMoveTime && horizontalInput != 0)
        {
            if (horizontalInput > 0)
            {
                ShiftSlotRight();
            }
            else if (horizontalInput < 0)
            {
                ShiftSlotLeft();
            }

            nextMoveTime = Time.time + 0.2f; // 0.2秒後に次の移動を許可
        }

        // Fire2ボタンが押された瞬間にのみアイテム情報を更新
        if (Input.GetButtonDown("Fire2")) // "押された瞬間"のみに変更
        {
            if (selectedSlot != null && selectedSlot.HasItem())
            {
                Item selectedItem = selectedSlot.GetItem();
                selectedItemPanel.UpdateSelectedItem(selectedItem);
            }
            else
            {
                selectedItemPanel.UpdateSelectedItem(null); // アイテムがない場合はnullで更新
            }
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
        int loopCount = 0;  // ループ回数のカウント

        do
        {
            newPosition += direction;
            if (newPosition >= slots.Length) newPosition = 0;
            if (newPosition < 0) newPosition = slots.Length - 1;

            loopCount++;
            if (loopCount > slots.Length)  // ループ回数がスロットの数を超えたら終了
            {
                return startPos;  // アイテムが見つからない場合は元の位置を返す
            }
        } while (!slots[newPosition].HasItem());

        return newPosition;
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


    public bool TryUseItem(Item.Type type)
    {
        // selectedItemPanelのアイテムを取得
        Item selectedPanelItem = selectedItemPanel.GetSelectedItem();

        // selectedItemPanelのアイテムがnull、またはタイプが一致しない場合は使用不可
        if (selectedPanelItem == null || selectedPanelItem.type != type)
        {
            return false;
        }
        // すべてのスロットから指定のタイプのアイテムを持つスロットを検索し、該当スロットのアイテムを削除
        foreach (Slot slot in slots)
        {
            Item slotItem = slot.GetItem();
            if (slotItem != null && slotItem.type == type)
            {
                bool result = selectedItemPanel.TryUseItemm(type, slot);
                if (result)
                {
                    slot.SetItem(null); // スロットのアイテムを削除
                    selectedItemPanel.UpdateSelectedItem(null); // selectedItemPanel内のアイテムも削除
                    slot.HideBGPanel(); // スロットの選択パネルを非表示
                    return true; // アイテムが正しく使用された場合に成功を返す
                }
            }
        }

        return false; // アイテムが正しく使用されなかった場合に失敗を返す
    }



 　　public Item GetSelectedItem()
    {
        return selectedSlot?.GetItem();
    }

    public void ResetItembox()
    {
        Debug.Log("deleteitems");
        // スロットの内容をクリア
        foreach (Slot slot in slots)
        {
            slot.SetItem(null);  // スロットのアイテムを消去
            slot.HideBGPanel();  // 背景パネルを非表示
        }

        selectedSlot = null;  // 選択されているスロットもクリア
        selectedItemPanel.UpdateSelectedItem(null);  // 選択アイテムパネルをクリア
    }


}
