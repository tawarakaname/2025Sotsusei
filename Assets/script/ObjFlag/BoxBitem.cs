using UnityEngine;

public class BoxBitem : MonoBehaviour
{
    public GameObject balloon; // インスペクタで設定するGameObject型のフィールド
    private FlagManager flagManager;

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得

        // 初期状態でballoonを非表示に設定
        if (balloon != null)
        {
            balloon.SetActive(false);
        }
    }

    void Update()
    {
        if (balloon == null) return; // balloonが設定されていない場合は何もしない

        bool isBoxBFlagOn = flagManager.GetFlag(FlagManager.FlagType.BoxB);
        bool isballoonFlagOn = flagManager.GetFlag(FlagManager.FlagType.balloon); //balloonフラグの状態を取得


        if (isBoxBFlagOn && isballoonFlagOn)
        {
            // 両方のフラグがONの場合、balloonを非表示
            if (balloon.activeSelf)
            {
                balloon.SetActive(false);
            }
        }
        else if (isBoxBFlagOn)
        {
            // BoxBフラグがONでballoonフラグがOFFの場合、balloonを表示
            if (!balloon.activeSelf)
            {
                balloon.SetActive(true);
            }
        }
        else
        {
            // BoxBフラグがOFFの場合、balloonを非表示
            if (balloon.activeSelf)
            {
                balloon.SetActive(false);
            }
        }
    }
}
