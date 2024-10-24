using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SetObj : MonoBehaviour
{
    [SerializeField] GameObject setObject;
    [SerializeField] Item.Type useItem;
    [SerializeField] Collider triggerCollider;
    [SerializeField] GameObject TextBox; // TextBoxへの参照
    [SerializeField] TextManager textManager; // TextManagerへの参照

    private AudioSource[] audioSources; // 複数のAudioSourceを格納
    private AudioSource audioSource; // 使用するAudioSource

    private string currentKeyword;
    private bool playerInsideCollider = false;

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        if (audioSources == null || audioSources.Length == 0)
        {
            return;
        }

        // AudioSourceが存在する場合、最初のものを使用
        audioSource = audioSources[0];
    }

    private void UpdateSetObjEnabled()
    {
        // DialPasswordclearフラグを基に有効無効を設定
        enabled = FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear);
    }

    private void OnEnable()
    {
        // 初期化時にコンポーネントの有効状態を設定
        UpdateSetObjEnabled();
    }

    private void Update()
    {
        // フラグに基づいてこのコンポーネントの有効状態を更新
        UpdateSetObjEnabled();

        // プレイヤーがコライダー内にいて、ボタンが押されたときのみ処理
        if (playerInsideCollider && Input.GetButtonDown("Fire2") && enabled)
        {
            OnClickThis(); // TextBoxが非表示のときはOnClickThisを呼び出す
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true; // プレイヤーがコライダー内にいることを記録
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false; // プレイヤーがコライダーから出たことを記録
        }
    }

    public bool OnClickThis()
    {
        // アイテムを使用できるか確認し、使用できたら処理を行う
        if (Itembox.instance.TryUseItem(useItem))
        {
            Item selectedItem = Itembox.instance.GetSelectedItem();

            if (selectedItem != null)
            {
                // アイテムのタイプに基づいてフラグを設定
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
            // アイテムが正しく使用されなかった場合
            currentKeyword = "Miss"; // エラーキーワードを設定
            if (audioSource != null)
            {
                audioSource.Play(); // 適切なAudioSourceを使用
            }
            // Textboxが表示されていない場合
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox) && currentKeyword != null)
            {
                OnClickMissTextThis(); // Miss用のテキストを表示
            }
            // Textboxが表示されている場合
            else if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                textManager.DisplayCurrentLine(); // 次のテキストラインを表示
            }

            return false; // アイテムが正しく使用されなかった
        }
    }

    public void OnClickMissTextThis()
    {
        // キーワードに基づいたテキストを表示
        textManager.DisplayTextForKeyword(currentKeyword);
        Debug.Log("a");
    }
}
