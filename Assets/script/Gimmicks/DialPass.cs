using UnityEngine;

public class DialPass : MonoBehaviour
{
    [SerializeField] GameObject Dialpassword;
    private bool iszoom;
    private DialPasswordButton dialPasswordButton; // DialPasswordButton スクリプトへの参照

    [SerializeField] AudioSource GasbaneraudioSource;

    private void Start()
    {
        Dialpassword.SetActive(false);

        GasbaneraudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        var isMovie = FlagManager.Instance.GetFlag(FlagManager.FlagType.Nowanim);
        // GasCamera0 が true かつズームがまだ有効でない場合
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.GasCamera0) && !iszoom && !isMovie)
        {
            iszoom = true;  // ズームフラグを true に設定
            Dialpassword.SetActive(true);  // ダイヤルパスワードのオブジェクトを表示
            DialPasswordButton[] dialPasswordButtons = Dialpassword.GetComponentsInChildren<DialPasswordButton>();
            foreach (var button in dialPasswordButtons)
            {
                button.ResetNumber(); // 数字をリセット
            }
        }
        // GasCamera0 が false かつズームが有効な場合
        else if (isMovie || (!FlagManager.Instance.GetFlag(FlagManager.FlagType.GasCamera0) && iszoom))
        {
            iszoom = false;  // ズームフラグを false に設定
            Dialpassword.SetActive(false); // ダイヤルパスワードのオブジェクトを非表示
        }

        // DialPasswordclear フラグが立ったときに音を再生
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear))
        {
            if (!GasbaneraudioSource.isPlaying)  // すでに再生中でない場合のみ再生
            {
                GasbaneraudioSource.Play(); // 鳴らしたいタイミングに音を再生
            }
        }
    }

}
