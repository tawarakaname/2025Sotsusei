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

    private ParticleSystem particleSystemA;
    private ParticleSystem particleSystemB;
    private ParticleSystem particleSystemC;
    private ParticleSystem particleSystemD;

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得

        // ParticleSystem コンポーネントを取得
        if (FireparticleA != null) particleSystemA = FireparticleA.GetComponent<ParticleSystem>();
        if (FireparticleB != null) particleSystemB = FireparticleB.GetComponent<ParticleSystem>();
        if (FireparticleC != null) particleSystemC = FireparticleC.GetComponent<ParticleSystem>();
        if (FireparticleD != null) particleSystemD = FireparticleD.GetComponent<ParticleSystem>();

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

        SetParticleActive(FireparticleA, isDialPasswordclearFlagOn);
        SetParticleActive(FireparticleB, isDialPasswordclearFlagOn);
        SetParticleActive(FireparticleC, isDialPasswordclearFlagOn);
        SetParticleActive(FireparticleD, isDialPasswordclearFlagOn);

        if (isDialPasswordclearFlagOn && !isFireParticleONFlagOn)
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.FireParticleON, true);
            Debug.Log("FireParticleFlagON");
        }

        // capsuleclear フラグがオンになったら色を変更
        if (isCapsuleclearFlagOn)
        {
            ChangeParticleColor(particleSystemA, Color.yellow);              // Aは黄色
            ChangeParticleColor(particleSystemB, new Color(0.3f, 1f, 0.5f));  // Bはライムグリーン
            ChangeParticleColor(particleSystemC, new Color(1f, 0.4f, 0.7f)); // Cはピンク
            ChangeParticleColor(particleSystemD, new Color(1f, 0.5f, 0f));   // Dはオレンジ
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
