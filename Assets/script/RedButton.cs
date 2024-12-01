using UnityEngine;
using System.Collections;

public class RedButton : MonoBehaviour
{
    private FlagManager flagManager;
    public GameObject RedButtonhuta;
    public GameObject gaslamp; // アニメーション対象のオブジェクト
    private Animator RedButtonhutaAnimator;
    private Animator gaslampAnimator; // Animatorコンポーネント
    [SerializeField] GameObject ButtonUIImage; // 表示・非表示を制御するImage
    [SerializeField] private MonoBehaviour playerScript; // プレイヤー操作スクリプトを参照

    void Start()
    {
        flagManager = FlagManager.Instance;
        if (RedButtonhuta != null)
            RedButtonhutaAnimator = RedButtonhuta.GetComponent<Animator>();
        if (gaslamp != null)
            gaslampAnimator = gaslamp.GetComponent<Animator>();
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(false); // 初期状態は非表示
        }
    }

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
            StartCoroutine(PlayGaslampAndRedButtonHuta());
        }

        if (redButtonHutaopen && !pushRedButton && Input.GetButtonDown("Fire2"))
        {
            playredbutton1();
        }
    }

    private IEnumerator PlayGaslampAndRedButtonHuta()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.RedButtonHutaopen)) yield break;

        FlagManager.Instance.SetFlag(FlagManager.FlagType.Nowanim, true);
        // gaslamp のアニメーションを再生
        if (gaslampAnimator != null)
        {
            gaslampAnimator.SetTrigger("gaslamp");
        }

        // gaslamp のアニメーション終了まで待機
        yield return new WaitForSeconds(GetAnimationLength(gaslampAnimator, "gaslamp"));

        // 1秒待機
        yield return new WaitForSeconds(1f);

        // RedButtonhuta のアニメーションを再生
        if (RedButtonhutaAnimator != null)
        {
            RedButtonhutaAnimator.SetTrigger("Redbttonopen");
        }

        // ButtonUIImageを表示
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(true);
        }

        // フラグを設定 (一度だけ)
        if (!flagManager.GetFlag(FlagManager.FlagType.RedButtonHutaopen))
        {
            flagManager.SetFlag(FlagManager.FlagType.RedButtonHutaopen, true);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Nowanim, false);

        }
    }

    private float GetAnimationLength(Animator animator, string animationName)
    {
        if (animator == null || string.IsNullOrEmpty(animationName)) return 0f;
        var runtimeAnimatorController = animator.runtimeAnimatorController;
        foreach (var clip in runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f;
    }

    private void playredbutton1()
    {
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

        // フラグを設定 (一度だけ)
        if (!flagManager.GetFlag(FlagManager.FlagType.PushRedButton))
        {
            flagManager.SetFlag(FlagManager.FlagType.PushRedButton, true);
        }
    }
}
