using UnityEngine;
using UnityEngine.UI;

public class SelectedItem : MonoBehaviour
{
    [SerializeField] private Image image; // UIのImageコンポーネント
    private Item currentItem; // 現在選択されているアイテム

    private void Awake()
    {
        // シーンが切り替わってもこのゲームオブジェクトを保持する
        DontDestroyOnLoad(gameObject);
    }

    // アイテムを更新するメソッド
    public void UpdateSelectedItem(Item item)
    {
        currentItem = item;

        // アイテムがnullでない場合にスプライトを更新
        if (item != null && image != null)
        {
            image.sprite = item.sprite;
        }
        else if (image != null)
        {
            image.sprite = null; // アイテムがnullの場合はスプライトを消去
        }
    }

    // アイテムの使用を試みるメソッド
    public bool TryUseItem(Item.Type type, Slot slot, Text notificationText)
    {
        // 現在のアイテムが正しい場合
        if (currentItem != null && currentItem.type == type)
        {
            // アイテムを使用する処理
            currentItem = null;
            if (image != null)
            {
                image.sprite = null; // アイテムを削除した場合はスプライトも消去
            }

            // スロットが指定されている場合
            if (slot != null)
            {
                slot.SetItem(null); // スロットからアイテムを削除
                slot.HideBGPanel(); // スロットの背景パネルを非表示
            }

            return true;
        }
        else
        {
            // アイテムが間違っている場合に通知テキストを表示
            if (notificationText != null)
            {
                notificationText.text = "このアイテムではないみたい";
                notificationText.gameObject.SetActive(true); // テキストを表示
            }
            return false;
        }
    }
}
