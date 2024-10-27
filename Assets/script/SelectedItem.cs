using UnityEngine;
using UnityEngine.UI;

public class SelectedItem : MonoBehaviour
{
    [SerializeField] private Image image; // UIのImageコンポーネント
    private Item currentItem; // 現在選択されているアイテム
    [SerializeField] private AudioSource audioSource;


    private void Awake()
    {
        // シーンが切り替わってもこのゲームオブジェクトを保持する
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    // アイテムを更新するメソッド
    public void UpdateSelectedItem(Item item)
    {
        currentItem = item;

        // アイテムがある場合は itemmotteru フラグを true に設定
        bool hasItem = item != null;
        FlagManager.Instance.SetFlag(FlagManager.FlagType.itemmotteru, hasItem);

        if (image != null)
        {
            image.sprite = hasItem ? item.sprite : null; // item が null でない場合スプライト更新、null なら消去
            Debug.Log("更新");
        }
    }

    // アイテムの使用を試みるメソッド
    public bool TryUseItemm(Item.Type type, Slot slot)
    {
        if (currentItem == null || currentItem.type != type)
        {
            return false; // アイテムが null、またはタイプが一致しない場合は使用不可
        }

        FlagManager.Instance.SetFlagByType(currentItem.type, true);
        currentItem = null;
        image.sprite = null; // アイテム消去時にスプライトもクリア
        FlagManager.Instance.SetFlag(FlagManager.FlagType.itemmotteru, false); // アイテム消去時にフラグを false に

        if (slot != null)
        {
            slot.HideBGPanel();
            slot.SetItem(null);
        }

        UpdateSelectedItem(null); // アイテム使用後に選択をクリア

        return true; // アイテムが使用された場合
    }
}
