using UnityEngine;

public class BoxBitem : MonoBehaviour
{
    public GameObject balloon; // インスペクタで設定するGameObject型のフィールド
    private FlagManager flagManager;
    private bool hasBalloonShown = false; // balloonが表示されたかどうかを管理するフラグ

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

        bool isColorPasswordclearFlagOn = flagManager.GetFlag(FlagManager.FlagType.ColorPasswordclear);
        bool isballoonFlagOn = flagManager.GetFlag(FlagManager.FlagType.balloon); // balloonフラグの状態を取得

        if (isColorPasswordclearFlagOn && isballoonFlagOn)
        {
            // 両方のフラグがONの場合、balloonを非表示
            if (balloon.activeSelf)
            {
                balloon.SetActive(false);
                hasBalloonShown = false; // balloonが非表示になったのでフラグをリセット
            }
        }
        else if (isColorPasswordclearFlagOn && !hasBalloonShown)
        {
            // ColorPasswordclearフラグがONでballoonフラグがOFFの場合、balloonを表示
            if (!balloon.activeSelf)
            {
                balloon.SetActive(true);
                hasBalloonShown = true; // balloonが表示されたのでフラグを更新
            }
        }
        else if (!isColorPasswordclearFlagOn)
        {
            // ColorPasswordclearフラグがOFFの場合、balloonを非表示
            if (balloon.activeSelf)
            {
                balloon.SetActive(false);
                hasBalloonShown = false; // balloonが非表示になったのでフラグをリセット
            }
        }
    }
}
