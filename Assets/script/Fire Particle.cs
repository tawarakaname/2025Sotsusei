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

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得

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
        bool isFireParticleONFlagOn = flagManager.GetFlag(FlagManager.FlagType.FireParticleON);
        bool isCapsuleclearFlagOn = flagManager.GetFlag(FlagManager.FlagType.capsuleclear);

        // アイテムタイプに基づくフラグの取得
        bool isCapsuleAOn = flagManager.GetFlagByType(Item.Type.capsuleA);
        bool isCapsuleBOn = flagManager.GetFlagByType(Item.Type.capsuleB);
        bool isCapsuleCOn = flagManager.GetFlagByType(Item.Type.capsuleC);
        bool isCapsuleDOn = flagManager.GetFlagByType(Item.Type.capsuleD);

        // すべてのcapsuleフラグがtrueの場合、capsuleclearフラグをtrueに設定
        if (!isCapsuleclearFlagOn && isCapsuleAOn && isCapsuleBOn && isCapsuleCOn && isCapsuleDOn)
        {
            flagManager.SetFlag(FlagManager.FlagType.capsuleclear, true);
        }

        // ダイヤルパスワードフラグがオンの場合、通常のFireparticleを表示
        SetParticleActive(FireparticleA, isDialPasswordclearFlagOn);
        SetParticleActive(FireparticleB, isDialPasswordclearFlagOn);
        SetParticleActive(FireparticleC, isDialPasswordclearFlagOn);
        SetParticleActive(FireparticleD, isDialPasswordclearFlagOn);

        if (isDialPasswordclearFlagOn && !isFireParticleONFlagOn)
        {
            flagManager.SetFlag(FlagManager.FlagType.FireParticleON, true);
            Debug.Log("FireParticleFlagON");
        }

        // capsuleclear フラグがオンになったら色を変更 (mainの方)
        if (isCapsuleclearFlagOn)
        {
            ChangeParticleColor(particleSystemAmain, Color.yellow);              // Aは黄色
            ChangeParticleColor(particleSystemBmain, new Color(0.3f, 1f, 0.5f));  // Bはライムグリーン
            ChangeParticleColor(particleSystemCmain, new Color(1f, 0.4f, 0.7f)); // Cはピンク
            ChangeParticleColor(particleSystemDmain, new Color(1f, 0.5f, 0f));   // Dはオレンジ
        }
    }

    private void SetParticleActive(GameObject particle, bool isActive)
    {
        if (particle == null) return;
        particle.SetActive(isActive);
    }

    private void ChangeParticleColor(ParticleSystem particleSystem, Color newColor)
    {
        if (particleSystem == null) return;

        var mainModule = particleSystem.main;
        mainModule.startColor = newColor;
    }
}

