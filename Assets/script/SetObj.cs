using System.Collections.Generic;
using UnityEngine;

public class SetObj : MonoBehaviour
{
    [System.Serializable]
    public class ItemSet
    {
        public Item.Type useItem; // 使用するアイテムのタイプ
        public GameObject setObject; // 対応するオブジェクト
    }

    [SerializeField] private List<ItemSet> itemSets = new List<ItemSet>(); // アイテムとオブジェクトのペア
    [SerializeField] private GameObject targetUI; // 条件が揃った時に表示するUI
    [SerializeField] private SelectedItem selectedItem; // 選択されたアイテムの参照

    private AudioSource audioSource; // 通常音再生用のAudioSource
    private AudioSource audioSource2; // 追加音再生用のAudioSource
    private bool playerInsideCollider = false; // プレイヤーがコライダー内にいるかどうか
    public bool IsFreeInteract = true;
    private TextManager textManager; // テキストを管理するクラスの参照
    private string currentKeyword; // 現在のキーワード（エラーメッセージ用）

    private void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
        selectedItem = GameObject.FindWithTag("SelectedItem").GetComponent<SelectedItem>();
        targetUI.SetActive(false); // 初期状態でUIを非表示に設定
        Debug.Log("e?");
        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得
        audioSource2 = GetComponents<AudioSource>()[1]; // 2つ目のAudioSourceコンポーネントを取得
    }

    private void Update()
    {
        if (!IsFreeInteract) return;
        UpdateUIVisibility(); // UIの表示状態を更新
    }

    private void UpdateUIVisibility()
    {
        // コライダー内にプレイヤーがいる場合のみ条件を満たす
        if (!playerInsideCollider)
        {
            if (targetUI.activeSelf)
            {
                targetUI.SetActive(false);
            }
            return;
        }

        bool isCameraZoomObj = FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj);
        bool hasItem = FlagManager.Instance.GetFlag(FlagManager.FlagType.itemmotteru);
        bool componentEnabled = IsComponentEnabled();
        var isMovie = FlagManager.Instance.GetFlag(FlagManager.FlagType.Nowanim);

        if (!isMovie && isCameraZoomObj && hasItem && componentEnabled)
        {
            if (targetUI.activeSelf)
            {
                UIdasetayo();
            }
            else
            {
                targetUI.SetActive(true);
            }
        }
        else
        {
            if (targetUI.activeSelf)
            {
                targetUI.SetActive(false);
            }
        }
    }

    private bool IsComponentEnabled()
    {
        bool isCameraZoomObj = FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj);
        return isCameraZoomObj;
    }

    private void UIdasetayo()
    {
        if (targetUI.activeSelf && playerInsideCollider && Input.GetButtonDown("Fire2"))
        {
            OnClickThis();
        }
    }

    public bool OnClickThis()
    {
        // 現在選択されているアイテムを取得
        Item currentSelectedItem = selectedItem.GetSelectedItem();
        if (currentSelectedItem == null) return false;

        // アイテムリストから対応するItemSetを探す
        foreach (var itemSet in itemSets)
        {
            if (currentSelectedItem.type == itemSet.useItem)
            {
                // アイテム使用を試みる
                if (Itembox.instance.TryUseItem(itemSet.useItem))
                {
                    // 使用成功時の処理
                    targetUI.SetActive(false);
                    ActivateItemSet(itemSet);
                    return true; // アイテムが使用された場合、早期リターン
                }
                else
                {
                    // アイテム使用失敗時の処理
                    HandleMiss();
                    return false; // 失敗した場合のリターン
                }
            }
        }

        // 一致するItemSetが見つからなかった場合
        HandleMiss();
        return false;
    }


    private void ActivateItemSet(ItemSet itemSet)
    {
        if (itemSet.setObject != null)
        {
            itemSet.setObject.SetActive(true);
            audioSource2?.Play(); // 条件に応じてaudioSource2を再生
        }
        selectedItem.UpdateSelectedItem(null); // 使用後は選択アイテムをクリア
    }

    private void HandleMiss()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // アイテム使用失敗時に通常の音を再生
        }
        currentKeyword = "Miss";
        // アイテムが間違っている場合の処理
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            OnClickMissTextThis();
        }
        else
        {
            textManager.DisplayCurrentLine(); // Miss時のテキスト表示
        }
    }

    // エラー時のキーワードに基づいてテキストを表示
    public void OnClickMissTextThis()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;
        }
    }
}