using UnityEngine;

public class AkeyItem : MonoBehaviour
{
    public GameObject Akey; // インスペクタで設定するGameObject型のフィールド
    private FlagManager flagManager;
    private bool hasAkeyShown = false; // Akeyが表示されたかどうかを管理するフラグ

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得

        // 初期状態でAkeyを非表示に設定
        if (Akey != null)
        {
            Akey.SetActive(false);
        }
    }

    void Update()//鍵が2回表示されるバグの根源
    {
        if (Akey == null) return; // Akeyが設定されていない場合は何もしない

        // Akeygetフラグがtrueかどうかを確認
        bool isAkeygetFlagOn = flagManager.GetFlag(FlagManager.FlagType.Akeyget);
        bool isAdooropenFlagOn = flagManager.GetFlag(FlagManager.FlagType.Adooropen);

        // Akeygetフラグがtrueで、まだAkeyを表示していない場合
        if (isAkeygetFlagOn && !hasAkeyShown && !isAdooropenFlagOn)
        {
            // Akeyを表示
            Akey.SetActive(true);
            hasAkeyShown = true; // Akeyが表示されたのでフラグを設定
        }
    }
}
