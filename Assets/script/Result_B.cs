using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class Result_B : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    private bool hasStarted = false;
    private SceneTransitionManager transitionManager;

    private void Start()
    {
        // SceneTransitionManager を取得してキャッシュ
        transitionManager = FindObjectOfType<SceneTransitionManager>();
        if (transitionManager == null)
        {
            Debug.LogError("SceneTransitionManager がシーン内に存在しません");
        }
    }

    void Update()
    {
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.potopen))
        {
            return;
        }

        // フラグが true かつ、まだ処理を開始していない場合
        if (!hasStarted && FlagManager.Instance.GetFlag(FlagManager.FlagType.Bdooropen) && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Result_B))
        {
            hasStarted = true; // 処理を一度だけ実行
            PlayTimeline();
        }
    }

    private void PlayTimeline()
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Nowanim, true);
        if (director != null)
        {
            director.Play();
            director.stopped += OnTimelineFinished; // Timeline終了時に呼ばれる
        }
        else
        {
            Debug.LogWarning("PlayableDirector が設定されていません");
        }
    }

    private void OnTimelineFinished(PlayableDirector obj)
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Result_B, true);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Nowanim, false);

        if (transitionManager != null)
        {
            transitionManager.LoadScene("Z");
        }
        else
        {
            Debug.LogError("SceneTransitionManager が見つかりません。シーン遷移できません。");
        }
    }
}
