using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticle : MonoBehaviour
{
    private FlagManager flagManager;

    private bool lastCapsuleClearFlagState = false;
    private bool lastDialPasswordclearFlagState = false;
    [SerializeField] private GameObject[] fireparticles;
    [SerializeField] private GameObject[] fireParticleMains;
    private ParticleSystem[] particleSystems;
    private Color[] particleColors =
        {
    　　　　new Color(1.0f, 0.933f, 0.0f), // FFEE00
    　　　　new Color(0.3f, 1f, 0.5f),     // そのまま
    　　　　new Color(0.988f, 0.573f, 0.969f), // FC92F7
    　　　　new Color(1.0f, 0.38f, 0.0f)   // FF6100
        };

    void Start()
    {
        flagManager = FlagManager.Instance;
        particleSystems = new ParticleSystem[fireParticleMains.Length];
        for (var i = 0; i < fireParticleMains.Length; i++)
        {
            if (fireParticleMains[i] == null) continue;
            particleSystems[i] = fireParticleMains[i].GetComponent<ParticleSystem>();
        }

        // 初期状態でFireparticleを非表示に設定
        foreach (var fire in fireparticles)
        {
            SetParticleActive(fire, false);
        }
    }

    void Update()
    {
        bool isDialPasswordclearFlagOn = flagManager.GetFlag(FlagManager.FlagType.DialPasswordclear);
        bool isCapsuleclearFlagOn = flagManager.GetFlag(FlagManager.FlagType.capsuleclear);

        // DialPasswordclear フラグの変化を検知してパーティクルのアクティブ化を制御
        if (isDialPasswordclearFlagOn != lastDialPasswordclearFlagState)
        {
            foreach (var fire in fireparticles)
            {
                SetParticleActive(fire, isDialPasswordclearFlagOn);
            }

            if (isDialPasswordclearFlagOn)
            {
                flagManager.SetFlag(FlagManager.FlagType.FireParticleON, true);
            }
            lastDialPasswordclearFlagState = isDialPasswordclearFlagOn;
        }

        // capsuleclear フラグが変わった場合のみ色を変更
        if (isCapsuleclearFlagOn && !lastCapsuleClearFlagState)
        {
            for (var i = 0; i < particleSystems.Length; i++)
            {
                if (i >= particleColors.Length) continue;
                ChangeParticleColor(particleSystems[i], particleColors[i]);
            }

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
        mainModule.startColor = newColor; // startColorを変更

        // ColorOverLifetimeの色を単色に設定
        var colorOverLifetime = particleSystem.colorOverLifetime;
        if (colorOverLifetime.enabled) // モジュールが有効なら変更
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(newColor, 0f), new GradientColorKey(newColor, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
            );
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        }

        // 再生
        particleSystem.Stop();
        particleSystem.Clear();
        particleSystem.Play();
    }

}