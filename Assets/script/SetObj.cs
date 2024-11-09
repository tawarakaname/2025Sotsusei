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

    private AudioSource audioSource; // 通常音再生用のAudioSource
    private AudioSource audioSource2; // 追加音再生用のAudioSource
    private bool playerInsideCollider = false; // プレイヤーがコライダー内にいるかどうか
    public bool IsFreeInteract = true;

    private void Start()
    {
        targetUI.SetActive(false); // 初期状態でUIを非表示に設定
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
        bool isCameraZoomObj = FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj);
        bool hasItem = FlagManager.Instance.GetFlag(FlagManager.FlagType.itemmotteru);
        bool componentEnabled = IsComponentEnabled(); // コンポーネントが有効かどうかを確認
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
        //stageBのテストプレイのためにコメントアウトしてるけど、Aから通しでplayする場合dialpasswordを戻す
        //bool isDialPasswordClear = FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear);
        bool isCameraZoomObj = FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj);
        return isCameraZoomObj;//&& isDialPasswordClear
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
        if (Itembox.instance.TryUseItem(useItem))
        {
            targetUI.SetActive(false);

            Item currentSelectedItem = selectedItem.GetSelectedItem();

            if (currentSelectedItem != null && currentSelectedItem.type == useItem)
            {
                FlagManager.Instance.SetFlagByType(currentSelectedItem.type, true);
                selectedItem.UpdateSelectedItem(null);
            }

            if (setObject != null)
            {
                setObject.SetActive(true);
                audioSource2?.Play(); // 条件に応じてaudioSource2を再生
            }
            return true;
        }
        else
        {
            HandleMiss();
            return false;
        }
    }

    private void HandleMiss()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // アイテム使用失敗時に通常の音を再生
        }
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