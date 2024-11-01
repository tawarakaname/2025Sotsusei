using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticle : MonoBehaviour
{
    [SerializeField] private GameObject FireparticleA;
    [SerializeField] private GameObject FireparticleB;
    [SerializeField] private GameObject FireparticleC;
    [SerializeField] private GameObject FireparticleD;
    [SerializeField] private GameObject FireparticleAmain;
    [SerializeField] private GameObject FireparticleBmain;
    [SerializeField] private GameObject FireparticleCmain;
    [SerializeField] private GameObject FireparticleDmain;
    private FlagManager flagManager;

    private ParticleSystem particleSystemAmain;
    private ParticleSystem particleSystemBmain;
    private ParticleSystem particleSystemCmain;
    private ParticleSystem particleSystemDmain;

    private bool lastCapsuleClearFlagState = false;
    private bool lastDialPasswordclearFlagState = false;

    void Start()
    {
        flagManager = FlagManager.Instance;

        // ParticleSystem コンポーネントを取得 (mainの方)
        if (FireparticleAmain != null) particleSystemAmain = FireparticleAmain.GetComponent<ParticleSystem>();
        if (FireparticleBmain != null) particleSystemBmain = FireparticleBmain.GetComponent<ParticleSystem>();
        if (FireparticleCmain != null) particleSystemCmain = FireparticleCmain.GetComponent<ParticleSystem>();
        if (FireparticleDmain != null) particleSystemDmain = FireparticleDmain.GetComponent<ParticleSystem>();

        // 初期状態でFireparticleを非表示に設定
        SetParticleActive(FireparticleA, false);
        SetParticleActive(FireparticleB, false);
        SetParticleActive(FireparticleC, false);
        SetParticleActive(FireparticleD, false);
    }

    void Update()
    {
        bool isDialPasswordclearFlagOn = flagManager.GetFlag(FlagManager.FlagType.DialPasswordclear);
        bool isCapsuleclearFlagOn = flagManager.GetFlag(FlagManager.FlagType.capsuleclear);

        // DialPasswordclear フラグの変化を検知してパーティクルのアクティブ化を制御
        if (isDialPasswordclearFlagOn != lastDialPasswordclearFlagState)
        {
            SetParticleActive(FireparticleA, isDialPasswordclearFlagOn);
            SetParticleActive(FireparticleB, isDialPasswordclearFlagOn);
            SetParticleActive(FireparticleC, isDialPasswordclearFlagOn);
            SetParticleActive(FireparticleD, isDialPasswordclearFlagOn);

            if (isDialPasswordclearFlagOn)
            {
                flagManager.SetFlag(FlagManager.FlagType.FireParticleON, true);
            }
            lastDialPasswordclearFlagState = isDialPasswordclearFlagOn;
        }

        // capsuleclear フラグが変わった場合のみ色を変更
        if (isCapsuleclearFlagOn && !lastCapsuleClearFlagState)
        {
            ChangeParticleColor(particleSystemAmain, Color.yellow);                  // Aは黄色
            ChangeParticleColor(particleSystemBmain, new Color(0.3f, 1f, 0.5f));    // Bはライムグリーン
            ChangeParticleColor(particleSystemCmain, new Color(1f, 0.176f, 0.761f)); // Cはピンク
            ChangeParticleColor(particleSystemDmain, new Color(0.698f, 0.259f, 0f)); // Dはオレンジ
            lastCapsuleClearFlagState = true;
        }

        // すべての capsule フラグが true の場合に capsuleclear を true に設定
        CheckAndSetCapsuleClear();
    }

    private void CheckAndSetCapsuleClear()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.capsuleclear))
        {
            bool isCapsuleAOn = flagManager.GetFlagByType(Item.Type.capsuleA);
            bool isCapsuleBOn = flagManager.GetFlagByType(Item.Type.capsuleB);
            bool isCapsuleCOn = flagManager.GetFlagByType(Item.Type.capsuleC);
            bool isCapsuleDOn = flagManager.GetFlagByType(Item.Type.capsuleD);

            if (isCapsuleAOn && isCapsuleBOn && isCapsuleCOn && isCapsuleDOn)
            {
                flagManager.SetFlag(FlagManager.FlagType.capsuleclear, true);
            }
        }
    }

    private void SetParticleActive(GameObject particle, bool isActive)
    {
        if (particle == null || particle.activeSelf == isActive) return;
        particle.SetActive(isActive);
    }

    private void ChangeParticleColor(ParticleSystem particleSystem, Color newColor)
    {
        if (particleSystem == null) return;
        var mainModule = particleSystem.main;
        mainModule.startColor = newColor;
    }
}

