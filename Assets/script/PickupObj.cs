using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObj : MonoBehaviour
{
    [SerializeField] Item.Type itemType;
    [SerializeField] Collider triggerCollider;
    [SerializeField] private GameObject TextBox;
    [SerializeField] private GameObject DTextBox;
    [SerializeField] private GameObject modelobj;
    Item item;
    [SerializeField] GameObject targetImage; // 表示・非表示を制御するImage
    [SerializeField] AudioSource audioSource;

    private bool playerInsideCollider = false;
    private bool ispickup = false;
    private bool hasDisplayedText = false;

    private void Start()
    {
        // itemタイプに応じてitemを作成する
        item = ItemGenerater.instance.Spawn(itemType);

        // Imageが設定されていない場合の警告
        if (targetImage == null)
        {
            return;
        }
        else
        {
            // Imageを非表示にする
            targetImage.SetActive(false);
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Textboxフラグがfalseになったときにアイテムを非表示にする処理
        if (ispickup && hasDisplayedText && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            gameObject.SetActive(false);
        }

        if (playerInsideCollider)
        {
            // PS4コントローラーの⚪︎ボタンは「Fire2」として認識されます
            if (Input.GetButtonDown("Fire2"))
            {
                OnClickObj();

                // audioSourceがnullならreturn、そうでなければPlay
                if (audioSource == null)
                {
                    return;
                }

                audioSource.Play(); // 鳴らしたいタイミングで再生
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;

            if (targetImage != null)
            {
                targetImage.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;

            if (targetImage != null)
            {
                targetImage.SetActive(false); // Imageを非表示
            }
        }
    }

    // クリックしたらobjを消す
    public void OnClickObj()
    {
        if (!ispickup)
        {
            // アイテムを設定
            Itembox.instance.SetItem(item);
            ispickup = true;
        }

        // アイテムが設定された後の処理
        modelobj.SetActive(false);

        // アイテムのタイプに対応するテキストを表示
        TextManager textManager = FindObjectOfType<TextManager>();
        if (textManager != null)
        {
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                textManager.DisplayTextForItemType(item.type);
            }
            else
            {
                textManager.DisplayCurrentLine();
            }
            hasDisplayedText = true; // テキスト表示が行われたことを記録
        }

        // itemTypeが"key1"だった場合のみ、Flagを設定
        if (itemType == Item.Type.key1)
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.havekey1, true);
        }
    }
}
