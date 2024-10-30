using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
            if (targetUI.activeSelf)
            {
                UIdasetayo();
            }
            else
            {
                targetUI.SetActive(true);
                Debug.Log("true");
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

    // SetObj.cs の OnClickThis メソッド修正
    public bool OnClickThis()
    {
        // アイテムの使用を試みる
        if (Itembox.instance.TryUseItem(useItem))
        {
            targetUI.SetActive(false);

            // selectedItem オブジェクトから現在選択されているアイテムを取得
            Item currentSelectedItem = selectedItem.GetSelectedItem();

            // 現在選択されているアイテムが使用アイテムと一致する場合、フラグを設定
            if (currentSelectedItem != null && currentSelectedItem.type == useItem)
            {
                FlagManager.Instance.SetFlagByType(currentSelectedItem.type, true); // 使用されたアイテムタイプのフラグをtrueに設定
                selectedItem.UpdateSelectedItem(null); // 使用後に選択をクリア
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
        if (audioSource != null) // nullチェックを修正
        {
            audioSource.Play(); // 音を再生
        }
        else
        {
            return; // 何もしない場合はreturn
        }
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