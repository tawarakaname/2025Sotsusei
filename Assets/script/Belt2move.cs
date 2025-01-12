using UnityEngine;
using UnityEngine.Playables;

public class Belt2move : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;           // プレイヤーのスクリプト（操作を無効化するため）
    [SerializeField] private GameObject targetCamera;              // アニメーション後に無効化するカメラ
    [SerializeField] private GameObject ckey;              //プレイヤーが持つ鍵のかけらアイテム
    [SerializeField] private GameObject ckeyfalse;              //プレイヤーが持つ鍵のかけらをとったらanim内のかけらを貸さないといけない
    private FlagManager flagManager;                               // フラグマネージャー
    public PlayableDirector director;

    private bool animationCompleted = false;                       // アニメーションの完了フラグ
    private bool canFire2 = false;                                 // fire2入力許可フラグ

    void Start()
    {
        flagManager = FlagManager.Instance;

    }

    void Update()
    {
        if (flagManager.GetFlagByType(Item.Type.batteryC) &&
            flagManager.GetFlagByType(Item.Type.batteryD) &&
            !flagManager.GetFlag(FlagManager.FlagType.Belt2move))
        {
            Movebelt2();
        }

        if (canFire2 && Input.GetButtonDown("fire2"))
        {
            HandleFire2Input();
        }

        if (animationCompleted)
        {
            if (targetCamera != null)
            {
                targetCamera.SetActive(false);  // カメラを無効化
            }
            animationCompleted = false;
        }
    }

    private void Movebelt2()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.Belt2move) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim))
        {
            director.Play();

            DisablePlayerControls();

            flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);

            // 20秒後にfire2入力を許可
            Invoke(nameof(AllowFire2Input), 20f);
        }
    }

    private void AllowFire2Input()
    {
        ckey.SetActive(true);
        canFire2 = true;
    }

    private void HandleFire2Input()
    {
        // 再生を停止し、タイムライン終了処理を呼び出す
        if (director != null && director.state == PlayState.Playing)
        {
            director.Stop();
            OnPlayableDirectorStopped(director);
            ckeyfalse.SetActive(false);
        }
        canFire2 = false; // fire2入力を無効化
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;
        }
    }

    private void EnablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = true;
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        animationCompleted = true;
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
        flagManager.SetFlag(FlagManager.FlagType.Belt2move, true);
        EnablePlayerControls();
    }
}
