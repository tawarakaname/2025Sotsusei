using UnityEngine;

public class MotosenItem : MonoBehaviour
{
    public GameObject HintA; // インスペクタで設定するGameObject型のフィールド
    private FlagManager flagManager;

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得

        // 初期状態でHintAを非表示に設定
        if (HintA != null)
        {
            HintA.SetActive(false);
        }
    }

    void Update()
    {
        if (HintA == null) return; // HintAが設定されていない場合は何もしない

        bool isMotosenFlagOn = flagManager.GetFlag(FlagManager.FlagType.Motosen);
        bool isHintAFlagOn = flagManager.GetFlag(FlagManager.FlagType.HintA); // HintAフラグの状態を取得


        if (isMotosenFlagOn && isHintAFlagOn)
        {
            // 両方のフラグがONの場合、HintAを非表示
            if (HintA.activeSelf)
            {
                HintA.SetActive(false);
            }
        }
        else if (isMotosenFlagOn)
        {
            // MotosenフラグがONでHintAフラグがOFFの場合、HintAを表示
            if (!HintA.activeSelf)
            {
                HintA.SetActive(true);
            }
        }
        else
        {
            // MotosenフラグがOFFの場合、HintAを非表示
            if (HintA.activeSelf)
            {
                HintA.SetActive(false);
            }
        }
    }
}
