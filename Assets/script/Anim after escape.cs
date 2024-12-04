using System.Collections.Generic;
using UnityEngine;

public class Animafterescape : MonoBehaviour
{
    public GameObject ueafter;
    public GameObject shitaafter;
    private Animator ueafterAnimator;
    private Animator shitaafterAnimator;
    private FlagManager flagManager;

    // Start is called before the first frame update
    void Awake()
    {
        if (ueafter != null) ueafterAnimator = ueafter.GetComponent<Animator>();
        if (shitaafter != null) shitaafterAnimator = shitaafter.GetComponent<Animator>();

        // フラグマネージャーのインスタンスを取得
        flagManager = FlagManager.Instance;

        // Adooropen フラグが false の場合、このスクリプトを終了
        if (!flagManager.GetFlag(FlagManager.FlagType.Adooropen))
        {
            return;
        }

        // Adooropen フラグが true の場合、アニメーションを設定
        if (ueafterAnimator != null) ueafterAnimator.SetTrigger("Aboxafter");
        if (shitaafterAnimator != null) shitaafterAnimator.SetTrigger("Aboxaftershita");
    }
}
