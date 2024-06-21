using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itembox : MonoBehaviour
{
    //Slotsにslot要素をコードから入れる方法
    [SerializeField] Slot[] slots;
    [SerializeField] Slot selectedSlot = null;
    [SerializeField] Text notificationText;

    //どこでも実行できるやつ(staticはどこでも引っ張っていける変数）
    public static Itembox instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            slots = GetComponentsInChildren<Slot>();
        }

        // 初期状態でテキストを非表示にする
        if (notificationText != null)
        {
            notificationText.gameObject.SetActive(false);
        }
    }

    //Pickupobjがクリックされたらスロットにアイテムを入れる
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
        //一旦全てのスロットの選択パネルを非表示
        foreach (Slot slot in slots)
        {
            slot.HideBGPanel();
        }
        selectedSlot = null;

        //選択されたスロットの選択パネルを表示
        if (slots[position].OnSelected())
        {
            selectedSlot = slots[position];
        }
    }

    //アイテムの使用を試みる＆使えるなら使ってしまう
    public bool TryUseItem(Item.Type type)
    {
         //選択スロットがあるか
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
        if (selectedSlot == null)
        {
            return null;
        }
        return selectedSlot.GetItem();
    }
}
