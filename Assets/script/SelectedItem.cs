using UnityEngine;
using UnityEngine.UI;

public class SelectedItem : MonoBehaviour
{
    [SerializeField] private Image image; // UIのImageコンポーネント
    private Item selectedItem; // 現在選択されているアイテム
    [SerializeField] private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        CheckSingleton();
    }

    // アイテムを更新するメソッド
    public void UpdateSelectedItem(Item item)
    {
        selectedItem = item;

        // アイテムがある場合は itemmotteru フラグを true に設定
        bool hasItem = item != null;
        FlagManager.Instance.SetFlag(FlagManager.FlagType.itemmotteru, hasItem);

        if (image != null)
        {
            image.sprite = hasItem ? item.sprite : null; // item が null でない場合スプライト更新、null なら消去
            Debug.Log("更新");
        }
    }
    public Item GetSelectedItem()
    {
        return selectedItem;
    }
    // アイテムの使用を試みるメソッド
    // SelectedItem.cs の TryUseItemm メソッド修正
    public bool TryUseItemm(Item.Type type, Slot slot)
    {
        if (selectedItem == null || selectedItem.type != type)
        {
            return false; // アイテムが null、またはタイプが一致しない場合は使用不可
        }

        FlagManager.Instance.SetFlagByType(selectedItem.type, true); // アイテムのフラグを設定
        selectedItem = null; // 使用後にnullを設定
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
    private void CheckSingleton()
    {
        var target = GameObject.FindGameObjectWithTag(gameObject.tag);
        var checkResult = target != null && target != gameObject;

        if (checkResult)
        {
            Destroy(gameObject);
            return;
        }
    }
}
