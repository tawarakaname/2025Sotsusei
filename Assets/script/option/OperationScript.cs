using UnityEngine;
using UnityEngine.UI;

public class OperationScript : MonoBehaviour
{
    [SerializeField] private Image contentImage; // 内容を表示するImage
    private FlagManager flagManager; // フラグマネージャーのインスタンス

    void Start()
    {
        contentImage.gameObject.SetActive(false);

        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        if (flagManager == null)
        {
            return;
        }
    }

    public void Jump()
    {
        ShowContent();
    }

    private void ShowContent()
    {
        contentImage.gameObject.SetActive(true); // 内容Imageを表示
    }

    private void Update()
    {
        // ①optionフラグがtrueでない場合は実行しない
        if (!flagManager.GetFlag(FlagManager.FlagType.Option))
        {
            return;
        }

        // ③optionフラグとZukanフラグがどちらもtrueのときFire1を監視
        if (flagManager.GetFlag(FlagManager.FlagType.Operation) && Input.GetButtonDown("Fire1"))
        {
            HandleFire1Input(); // Fire1の入力処理
        }
    }
    private void HandleFire1Input()
    {
        // ③Fire1が入力された場合の処理
        contentImage.gameObject.SetActive(false);
        flagManager.SetFlag(FlagManager.FlagType.Operation, false);
    }

    public void ResetOperation()
    {
        // フラグをリセット
        flagManager.SetFlag(FlagManager.FlagType.Operation, false);
        contentImage.gameObject.SetActive(false);  // contentImageを非表示にする
    }

}
