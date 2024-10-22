using System.Collections;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;
    [SerializeField, Tooltip("1秒間における不透明度の増加量")] private float FadeSpeed = 1.0f;
    private Coroutine animationCoroutine;

    public void Restart()
    {
        Run();
    }

    public void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
    }

    private void Start()
    {
        Run();
    }

    private void Run()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(Seamless());
    }

    private IEnumerator Seamless()
    {
        tmpText.ForceMeshUpdate(true);
        TMP_TextInfo textInfo = tmpText.textInfo;
        TMP_CharacterInfo[] charInfos = textInfo.characterInfo;

        for (var i = 0; i < charInfos.Length; i++)
        {
            SetTextAlpha(tmpText, i, 0);
        }

        for (var i = 0; i < charInfos.Length; i++)
        {
            if (char.IsWhiteSpace(charInfos[i].character)) continue;
            yield return new WaitForSeconds(0.2f);

            float alpha = 0.0f;
            while (true)
            {
                yield return new WaitForFixedUpdate();
                float alphaDelta = FadeSpeed * Time.fixedDeltaTime;
                alpha = Mathf.Min(alpha + alphaDelta, 1.0f);
                SetTextAlpha(tmpText, i, (byte)(255 * alpha));

                if (alpha >= 1.0f) break;
            }
        }
    }

    private void SetTextAlpha(TMP_Text text, int charIndex, byte alpha)
    {
        TMP_TextInfo textInfo = text.textInfo;
        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
        TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];

        for (var i = 0; i < 4; ++i)
        {
            meshInfo.colors32[charInfo.vertexIndex + i].a = alpha;
        }

        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
