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

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得

        // 初期状態でFireparticleを非表示に設定
        if (WalltraceA != null) WalltraceA.SetActive(false);
        if (WalltraceB != null) WalltraceB.SetActive(false);
        if (WalltraceC != null) WalltraceC.SetActive(false);
        if (WalltraceD != null) WalltraceD.SetActive(false);
    }

    void Update()
    {
        bool isColorPasswordclearFlagOn = flagManager.GetFlag(FlagManager.FlagType.DialPasswordclear);

        if (isColorPasswordclearFlagOn && flagOnTime < 0f)
        {
            flagOnTime = Time.time; // フラグがオンになった時刻を記録
        }

        if (flagOnTime >= 0f && Time.time >= flagOnTime + 8f)　//3秒後
        {
            if (WalltraceA != null) WalltraceA.SetActive(true);
            if (WalltraceB != null) WalltraceB.SetActive(true);
            if (WalltraceC != null) WalltraceC.SetActive(true);
            if (WalltraceD != null) WalltraceD.SetActive(true);
        }
    }

}
