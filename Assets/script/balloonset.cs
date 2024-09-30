using System.Collections;
using UnityEngine;

public class BalloonSet : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // スプライトを表示するためのSpriteRenderer
    public Sprite[] balloonSprites; // 切り替える3枚の画像（スプライト）
    public float frameDuration = 0.5f; // 1枚の画像を表示する時間（秒）

    public Transform animationPosition; // アニメーションを表示する場所
    public Vector3 spriteRotation; // スプライトの回転角度（X, Y, Z）

    private bool hasPlayed = false;
    private int currentFrame = 0;

    void Update()
    {
        // balloon フラグが true になったかどうかを確認
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.balloon) && !hasPlayed)
        {
            StartCoroutine(PlayBalloonSpriteAnimation());
            hasPlayed = true; // 一度だけ再生するようにフラグを立てる
        }
    }

    // スプライトを順番に切り替えるコルーチン
    IEnumerator PlayBalloonSpriteAnimation()
    {
        // スプライトの表示場所を指定
        if (animationPosition != null)
        {
            spriteRenderer.transform.position = animationPosition.position;
        }

        // スプライトの回転を指定
        spriteRenderer.transform.rotation = Quaternion.Euler(spriteRotation);

        while (currentFrame < balloonSprites.Length)
        {
            // 現在のフレームのスプライトを設定
            spriteRenderer.sprite = balloonSprites[currentFrame];

            // frameDuration秒待つ
            yield return new WaitForSeconds(frameDuration);

            // 次のフレームに進む
            currentFrame++;
        }
    }
}

