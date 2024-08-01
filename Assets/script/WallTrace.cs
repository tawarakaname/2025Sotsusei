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
        bool isColorPasswordclearFlagOn = flagManager.GetFlag(FlagManager.FlagType.ColorPasswordclear);


        WalltraceA.SetActive(isColorPasswordclearFlagOn);
        WalltraceB.SetActive(isColorPasswordclearFlagOn);
        WalltraceC.SetActive(isColorPasswordclearFlagOn);
        WalltraceD.SetActive(isColorPasswordclearFlagOn);


    }

}
