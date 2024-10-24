using UnityEngine;
using UnityEngine.UI;

public class SelectedItem : MonoBehaviour
{
    [SerializeField] private Image image; // UIのImageコンポーネント
    private Item currentItem; // 現在選択されているアイテム
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
    public bool TryUseItem(Item.Type type, Slot slot)
    {
        // currentItemがnullの場合は早期リターン
        if (currentItem == null)
        {
            return false; // アイテムがnullの場合、使用できないためfalseを返す
        }

        // 現在のアイテムが正しい場合
        if (currentItem.type == type)
        {
            // 何のアイテムを使用するのかをコンソールに表示
            Debug.Log($"使用したアイテム: {currentItem.type}");

            // フラグを立てる処理
            FlagManager.Instance.SetFlagByType(currentItem.type, true);

            // アイテムを削除する処理
            currentItem = null;
            if (image != null)
            {
                image.sprite = null; // アイテムを削除した場合はスプライトも消去
            }

            // スロットが指定されている場合
            if (slot != null)
            {
                slot.HideBGPanel(); // スロットの背景パネルを非表示
                slot.SetItem(null); // スロットからアイテムを削除
            }

            return true; // アイテムが正しく使用された場合trueを返す
        }

        // アイテムのタイプが一致しない場合
        return false;
        
    }
}
