using UnityEngine;

public class RedButton : MonoBehaviour
{
    private FlagManager flagManager;
    public GameObject RedButtonhuta;
    private Animator RedButtonhutaAnimator;
    [SerializeField] GameObject ButtonUIImage; // 表示・非表示を制御するImage

    // Start is called before the first frame update
    void Start()
    {
        flagManager = FlagManager.Instance;
        if (RedButtonhuta != null)
            RedButtonhutaAnimator = RedButtonhuta.GetComponent<Animator>();
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(false); // 初期状態は非表示
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 共通フラグ取得
        bool btbCamera = flagManager.GetFlag(FlagManager.FlagType.BTBCamera);

        // BTBCamera フラグが false なら処理をスキップ
        if (!btbCamera) return;

        // その他のフラグ取得
        bool threePasswordclear = flagManager.GetFlag(FlagManager.FlagType.ThreePasswordclear);
        bool btb = flagManager.GetFlagByType(Item.Type.BTB);
        bool redButtonHutaopen = flagManager.GetFlag(FlagManager.FlagType.RedButtonHutaopen);
        bool pushRedButton = flagManager.GetFlag(FlagManager.FlagType.PushRedButton);

        // 条件を満たす場合に一度だけメソッドを呼び出す
        if (threePasswordclear && btb && !redButtonHutaopen)
        {
            playredbuttonhuta();
        }

        if (redButtonHutaopen && !pushRedButton && Input.GetButtonDown("Fire2"))
        {
            playredbutton1();
        }
    }

    private void playredbuttonhuta()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.PushRedButton) ||
            flagManager.GetFlag(FlagManager.FlagType.RedButtonHutaopen))
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

        flagManager.SetFlag(FlagManager.FlagType.RedButtonHutaopen, true);
    }

    private void playredbutton1()
    {
        // 二重呼び出しを防ぐ
        if (flagManager.GetFlag(FlagManager.FlagType.PushRedButton) ||
            !flagManager.GetFlag(FlagManager.FlagType.RedButtonHutaopen))
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

        flagManager.SetFlag(FlagManager.FlagType.PushRedButton, true);
    }
}
