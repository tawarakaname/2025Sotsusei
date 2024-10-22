using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrace : MonoBehaviour
{
    [SerializeField] private GameObject WalltraceA;
    [SerializeField] private GameObject WalltraceB;
    [SerializeField] private GameObject WalltraceC;
    [SerializeField] private GameObject WalltraceD;
    private FlagManager flagManager;
    private float flagOnTime = -1f; // フラグがオンになった時刻を記録するための変数
    private bool isFadingIn = false; // フェードインが開始されたかどうかのフラグ
    private float fadeDuration = 3f; // フェードインにかかる時間

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得

        // 初期状態でWalltraceを非表示にし、透明に設定
        SetWalltraceAlpha(WalltraceA, 0f);
        SetWalltraceAlpha(WalltraceB, 0f);
        SetWalltraceAlpha(WalltraceC, 0f);
        SetWalltraceAlpha(WalltraceD, 0f);
    }

    void Update()
    {
        bool isColorPasswordclearFlagOn = flagManager.GetFlag(FlagManager.FlagType.DialPasswordclear);

        if (isColorPasswordclearFlagOn && flagOnTime < 0f)
        {
            flagOnTime = Time.time; // フラグがオンになった時刻を記録
        }

        if (flagOnTime >= 0f && Time.time >= flagOnTime + 3f && !isFadingIn) // 3秒後にフェードインを開始
        {
            isFadingIn = true;
            StartCoroutine(FadeInWalltrace(WalltraceA));
            StartCoroutine(FadeInWalltrace(WalltraceB));
            StartCoroutine(FadeInWalltrace(WalltraceC));
            StartCoroutine(FadeInWalltrace(WalltraceD));
        }
    }

    // フェードインを行うコルーチン
    IEnumerator FadeInWalltrace(GameObject walltrace)
    {
        Renderer renderer = walltrace.GetComponent<Renderer>();
        Color color = renderer.material.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration); // 0から1の間でアルファ値を変化させる
            SetWalltraceAlpha(walltrace, alpha);
            yield return null;
        }

        SetWalltraceAlpha(walltrace, 1f); // 最終的に完全に表示
    }

    // オブジェクトのアルファ値を設定するヘルパーメソッド
    void SetWalltraceAlpha(GameObject walltrace, float alpha)
    {
        if (walltrace != null)
        {
            Renderer renderer = walltrace.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color color = renderer.material.color;
                color.a = alpha;
                renderer.material.color = color;
            }
        }
    }
}

