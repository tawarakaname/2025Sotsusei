using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItem : MonoBehaviour
{
    [SerializeField] private Image image;
    private Item currentItem;

    // アイテムを更新するメソッドを追加
    public void UpdateSelectedItem(Item item)
    {
        currentItem = item;
        if (item != null)
        {
            image.sprite = item.sprite;  // アイテムに対応するスプライトを表示
        }
    }

    // アイテムの使用を試みる＆使えるなら使ってしまう
    public bool TryUseItem(Item.Type type, Slot slot, Text notificationText)
    {
        if (currentItem != null && currentItem.type == type)
        {
            // アイテムを使用する処理を実行
            currentItem = null;
            image.sprite = null;  // アイテムを削除した場合はスプライトも消去

            if (slot != null)
            {
                slot.SetItem(null); // スロットからアイテムを削除
                slot.HideBGPanel(); // スロットの背景パネルを非表示
            }

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
}
