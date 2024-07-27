using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticle : MonoBehaviour
{
    [SerializeField] private GameObject Fireparticle;
    private FlagManager flagManager;

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得

        // 初期状態でFireparticleを非表示に設定
        if (Fireparticle != null)
        {
            Fireparticle.SetActive(false);
        }
    }

    void Update()
    {
        if (Fireparticle == null) return; // Fireparticleが設定されていない場合は何もしない

        bool isColorPasswordclearFlagOn = flagManager.GetFlag(FlagManager.FlagType.ColorPasswordclear);

        if (isColorPasswordclearFlagOn)
        {
            // ColorPasswordclearフラグがONの場合、Fireparticleを表示
            if (!Fireparticle.activeSelf)
            {
                Fireparticle.SetActive(true);
            }
        }
        else
        {
            // ColorPasswordclearフラグがOFFの場合、Fireparticleを非表示
            if (Fireparticle.activeSelf)
            {
                Fireparticle.SetActive(false);
            }
        }
    }
}
