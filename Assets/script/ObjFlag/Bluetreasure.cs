using UnityEngine;

public class Bluetreasure : MonoBehaviour
{
    public GameObject BTB; // インスペクタで設定するGameObject型のフィールド
    private FlagManager flagManager;

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得

        // 初期状態でBTBを非表示に設定
        if (BTB != null)
        {
            BTB.SetActive(false);
        }
    }

    void Update()
    {
        if (BTB == null) return; // BTBが設定されていない場合は何もしない

        bool isBluetreasureFlagOn = flagManager.GetFlag(FlagManager.FlagType.Bluetureasure);
        bool isBTBFlagOn = flagManager.GetFlag(FlagManager.FlagType.BTB); // BTBフラグの状態を取得

        if (isBluetreasureFlagOn && isBTBFlagOn)
        {
            // 両方のフラグがONの場合、BTBを非表示
            if (BTB.activeSelf)
            {
                BTB.SetActive(false);
            }
        }
        else if (isBluetreasureFlagOn)
        {
            // BluetreasureフラグがONでBTBフラグがOFFの場合、BTBを表示
            if (!BTB.activeSelf)
            {
                BTB.SetActive(true);
            }
        }
        else
        {
            // BluetreasureフラグがOFFの場合、BTBを非表示
            if (BTB.activeSelf)
            {
                BTB.SetActive(false);
            }
        }
    }
}
