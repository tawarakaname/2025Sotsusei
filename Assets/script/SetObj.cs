using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetObj : MonoBehaviour
{
    [SerializeField] private GameObject setObject; // 表示するオブジェクト
    [SerializeField] private Item.Type useItem; // 使用するアイテムのタイプ
    [SerializeField] private GameObject targetUI; // 条件が揃った時に表示するUI
    [SerializeField] private SelectedItem selectedItem; // 選択されたアイテムの参照
    

    private AudioSource audioSource; // 音を再生するためのAudioSource
    private bool playerInsideCollider = false; // プレイヤーがコライダー内にいるかどうか

    private void Start()
    {
        targetUI.SetActive(false); // 初期状態でUIを非表示に設定
        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得
    }

    private void Update()
    {
        UpdateUIVisibility(); // UIの表示状態を更新
    }

    private void UpdateUIVisibility()
    {
        // 各フラグの状態を取得
        bool isCameraZoomObj = FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj);
        bool hasItem = FlagManager.Instance.GetFlag(FlagManager.FlagType.itemmotteru);
        bool componentEnabled = IsComponentEnabled(); // コンポーネントが有効かどうかを確認

        // 全ての条件がtrueの場合にのみUIを表示
        if (isCameraZoomObj && hasItem && componentEnabled)
        {
            if (!targetUI.activeSelf) // すでに表示されていない場合のみアクティブ化
            {
                targetUI.SetActive(true);
                Debug.Log("true");
                UIdasetayo();
            }
        }
        else
        {
            if (targetUI.activeSelf) // 表示されている場合のみ非アクティブ化
            {
                targetUI.SetActive(false);
            }
        }
    }


    private bool IsComponentEnabled()
    {
        // ダイアルパスワードがクリアされているかとカメラズームオブジェクトフラグの状態を取得
        bool isDialPasswordClear = FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear);
        bool isCameraZoomObj = FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj);
        return isDialPasswordClear && isCameraZoomObj; // 両方がtrueの場合にのみ有効
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
        // アイテムの使用を試みる
        if (Itembox.instance.TryUseItem(useItem))
        {
            targetUI.SetActive(false);
            Item selectedItem = Itembox.instance.GetSelectedItem();
            if (selectedItem != null)
            {
                // 使用したアイテムのフラグを設定
                FlagManager.Instance.SetFlagByType(selectedItem.type, true);
            }
            // setObjectが指定されていればアクティブにする
            if (setObject != null)
            {
                setObject.SetActive(true);
            }
            return true; // アイテムが正しく使用された
        }
        else
        {
            HandleMiss(); // アイテムの使用に失敗した場合の処理
            return false; // アイテムが正しく使用されなかった
        }
    }

    private void HandleMiss()
    {
        audioSource?.Play(); // 音を再生
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがコライダーに入ったときの処理
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true; // プレイヤーがコライダー内にいることを記録
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // プレイヤーがコライダーから出たときの処理
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false; // プレイヤーがコライダーから出たことを記録
        }
    }

}
