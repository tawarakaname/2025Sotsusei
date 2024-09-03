using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticle : MonoBehaviour
{
    [SerializeField] private GameObject FireparticleA;
    [SerializeField] private GameObject FireparticleB;
    [SerializeField] private GameObject FireparticleC;
    [SerializeField] private GameObject FireparticleD;
    private FlagManager flagManager;

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得

        // 初期状態でFireparticleを非表示に設定
        if (FireparticleA != null) FireparticleA.SetActive(false);
        if (FireparticleB != null) FireparticleB.SetActive(false);
        if (FireparticleC != null) FireparticleC.SetActive(false);
        if (FireparticleD != null) FireparticleD.SetActive(false);
    }

    void Update()
    {
        bool isMotosenFlagOn = flagManager.GetFlag(FlagManager.FlagType.Motosen);
        bool isFireParticleONFlagOn = flagManager.GetFlag(FlagManager.FlagType.FireParticleON);


        SetParticleActive(FireparticleA, isMotosenFlagOn);
        SetParticleActive(FireparticleB, isMotosenFlagOn);
        SetParticleActive(FireparticleC, isMotosenFlagOn);
        SetParticleActive(FireparticleD, isMotosenFlagOn);

        if (isMotosenFlagOn && !isFireParticleONFlagOn)
        {
            // フラグを設定する
            FlagManager.Instance.SetFlag(FlagManager.FlagType.FireParticleON, true);
            Debug.Log("FireParticleFlagON");
        }
    }

    private void SetParticleActive(GameObject particle, bool isActive)
    {
        if (particle == null) return;

        if (isActive && !particle.activeSelf)
        {
            particle.SetActive(true);
        }
        else if (!isActive && particle.activeSelf)
        {
            particle.SetActive(false);
        }
    }
}
