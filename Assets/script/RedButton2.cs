using UnityEngine;
using System.Collections;

public class RedButton2 : MonoBehaviour
{
    private FlagManager flagManager;

    [SerializeField] private GameObject ButtonUIImage; // 表示・非表示を制御するImage
    [SerializeField] private GameObject Lever; // 回転させるオブジェクト
    private bool isleverdown = false; // トリガーが既に実行されたかどうかを管理
    private bool isRotationTriggered = false; // 回転が既に開始されたかどうかを管理

    void Start()
    {
        flagManager = FlagManager.Instance;
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(false); // 初期状態は非表示
        }
    }

    void Update()
    {
        // 共通フラグ取得
        bool LeverCamera = flagManager.GetFlag(FlagManager.FlagType.LeverCamera);

        // LeverCamera フラグが false なら処理をスキップ
        if (!LeverCamera) return;

        // その他のフラグ取得
        bool ToyPasswordclear = flagManager.GetFlag(FlagManager.FlagType.ToyPasswordclear);
        bool leverdown = flagManager.GetFlag(FlagManager.FlagType.Leverdown);

        // `Fire2` 押下時の処理
        if (ToyPasswordclear && !leverdown )
        {
            if (ButtonUIImage.activeSelf && !isRotationTriggered && Input.GetButtonDown("Fire2"))
            {
                StartCoroutine(RotateLever());
            }
            else if (!isleverdown)
            {
                pushleverdown();
            }
        }
    }

    private void pushleverdown()
    {
        // 既にトリガーが実行されている場合は処理を終了
        if (isleverdown || flagManager.GetFlag(FlagManager.FlagType.Leverdown))
            return;

        // ButtonUIImageを表示
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(true);
        }

        // トリガー済みフラグを設定
        isleverdown = true;

    }

    private IEnumerator RotateLever()
    {
        // 二重呼び出しを防ぐ
        isRotationTriggered = true;

        // 回転量と時間を設定
        Quaternion startRotation = Lever.transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0,90, 0);
        float duration = 1.0f; // 回転にかける時間（秒）
        float elapsedTime = 0f;

        // 回転をスムーズに行う
        while (elapsedTime < duration)
        {
            Lever.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 最終位置を確定
        Lever.transform.rotation = targetRotation;

        // 回転完了後の処理
        playleverdown();
    }

    private void playleverdown()
    {
        // Leverdown フラグを設定
        flagManager.SetFlag(FlagManager.FlagType.Leverdown, true);

        // ButtonUIImageを非表示
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(false);
        }

        // フラグを解除
        isleverdown = false;
        isRotationTriggered = false;
    }
}
