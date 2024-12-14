using UnityEngine;

public class RedButton2 : MonoBehaviour
{
    private FlagManager flagManager;
    public GameObject RedButton2huta;
    private Animator RedButtonhutaAnimator;
    [SerializeField] private GameObject ButtonUIImage; // 表示・非表示を制御するImage
    private bool isRedButtonHutaTriggered = false; // トリガーが既に実行されたかどうかを管理

    void Start()
    {
        flagManager = FlagManager.Instance;
        if (RedButton2huta != null)
            RedButtonhutaAnimator = RedButton2huta.GetComponent<Animator>();
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(false); // 初期状態は非表示
        }
    }

    void Update()
    {
        // 共通フラグ取得
        bool ButtonstandCamera = flagManager.GetFlag(FlagManager.FlagType.LeverCamera);

        // ButtonstandCamera フラグが false なら処理をスキップ
        if (!ButtonstandCamera) return;

        // その他のフラグ取得
        bool ToyPasswordclear = flagManager.GetFlag(FlagManager.FlagType.ToyPasswordclear);
        bool pushRedButton = flagManager.GetFlag(FlagManager.FlagType.PushRedButton2);

        // 条件を満たす場合に一度だけメソッドを呼び出す
        if (ToyPasswordclear)
        {
            playredbuttonhuta();
        }

        if (!pushRedButton && Input.GetButtonDown("Fire2"))
        {
            playredbutton1();
        }
    }

    private void playredbuttonhuta()
    {
        // 既にトリガーが実行されている場合は処理を終了
        if (isRedButtonHutaTriggered || flagManager.GetFlag(FlagManager.FlagType.PushRedButton2))
            return;

        if (RedButtonhutaAnimator != null)
        {
            RedButtonhutaAnimator.SetTrigger("Redbttonopen");
        }

        // ButtonUIImageを表示
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(true);
        }

        // トリガー済みフラグを設定
        isRedButtonHutaTriggered = true;
    }

    private void playredbutton1()
    {
        // 二重呼び出しを防ぐ
        if (flagManager.GetFlag(FlagManager.FlagType.PushRedButton2))
            return;

        if (RedButtonhutaAnimator != null)
        {
            RedButtonhutaAnimator.SetTrigger("Redbutton1");
        }

        // ButtonUIImageを非表示
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(false);
        }

        flagManager.SetFlag(FlagManager.FlagType.PushRedButton2, true);
    }
}
