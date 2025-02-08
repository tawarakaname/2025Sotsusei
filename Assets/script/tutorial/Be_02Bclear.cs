using UnityEngine.Playables;
using UnityEngine;

public class Be_02Bclear : MonoBehaviour
{
    private FlagManager flagManager;
    [SerializeField] private PlayableDirector director;
    private TextManager textManager;
    private bool isTextboxActive = false;
    private string currentKeyword;
    [SerializeField] private GameObject hanamaru;
    [SerializeField] private Collider Collider; // 台座コライダー

    void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
        flagManager = FlagManager.Instance;
        Collider.enabled = false;

        if (!flagManager.GetFlag(FlagManager.FlagType.Greeting))
        {
            DisableScript();
            return;
        }

        if (flagManager.GetFlag(FlagManager.FlagType.Be_Bclear))
        {
            DisableScript();
            return;
        }

        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }

        if (!flagManager.GetFlag(FlagManager.FlagType.Be_Bclear))
        {
            Greeting();
        }
    }

    private void Greeting()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.Be_Bclear) &&
            flagManager.GetFlag(FlagManager.FlagType.Result_B) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim))
        {
            flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);
            director.Play();
        }
    }


    private void OnPlayableDirectorStopped(PlayableDirector obj)
    {
        currentKeyword = "Be_Bclear";

        hanamaru.SetActive(true);
        // Timeline 終了後に Textbox を表示
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            textManager.DisplayTextForKeyword(currentKeyword); // 現在のキーワードでテキストを表示
        }
    }

    void Update()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.Greeting))
        {
            return;
        }

        if (flagManager.GetFlag(FlagManager.FlagType.Result_C))
        {
            return;
        }

        if (!flagManager.GetFlag(FlagManager.FlagType.Be_Bclear) &&
              flagManager.GetFlag(FlagManager.FlagType.Result_B) &&
              flagManager.GetFlag(FlagManager.FlagType.Nowanim))
        {
            // Fire2 ボタンで次のテキストを表示
            if (Input.GetButtonDown("Fire2") && isTextboxActive && FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                textManager.DisplayCurrentLine();
            }

            // テキスト終了後の処理
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox) && isTextboxActive)
            {
                isTextboxActive = false;
                Collider.enabled = true;
                flagManager.SetFlag(FlagManager.FlagType.Be_Bclear, true);
                flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
            }

            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                isTextboxActive = true;
            }
        }
    }

    private void DisableScript()
    {
        if (director != null)
        {
            director.stopped -= OnPlayableDirectorStopped;
            director.Stop();
        }
        this.enabled = false;
    }
}
