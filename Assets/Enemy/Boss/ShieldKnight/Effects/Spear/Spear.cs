using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//毎フレームRayCastするので多分重い。使いすぎ厳禁。
public class Spear : MonoBehaviour
{
    [Header("待機時間"), SerializeField] private float waitTime = 1;
    [Header("残留時間"), SerializeField] private float leaveTime = 1;
    [Header("限界長さ"), SerializeField] private float maxLength = 5;
    private float initLength = 1;
    private float nowLength = 1;

    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private BoxCollider2D boxCollider2D = null;

    void Awake()
    {
        boxCollider2D.enabled = false;

        initLength = this.transform.localScale.x;
        nowLength = initLength;

        Vector2 rendCurrentSize = spriteRenderer.size;
        Vector2 colliderSize = boxCollider2D.size;
        Vector2 colliderOffset = boxCollider2D.offset;

        // 新しいサイズを設定
        rendCurrentSize.x = nowLength / this.transform.localScale.x;
        colliderSize.x = nowLength / this.transform.localScale.x;
        colliderOffset.x = nowLength / (2 * this.transform.localScale.x);

        // 新しいサイズをSpriteRendererに設定
        spriteRenderer.size = rendCurrentSize;
        boxCollider2D.size = colliderSize;
        boxCollider2D.offset = colliderOffset;
    }


    void Update()
    {
        if(waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            if(waitTime <= 0 )
            {
                boxCollider2D.enabled = true;
            }
        }
        else if (nowLength - initLength < maxLength)
        {

            // 現在のサイズを取得
            Vector2 rendCurrentSize = spriteRenderer.size;
            Vector2 colliderSize = boxCollider2D.size;
            Vector2 colliderOffset = boxCollider2D.offset;

            // 新しいサイズを設定
            rendCurrentSize.x = nowLength / this.transform.localScale.x;
            colliderSize.x = nowLength / this.transform.localScale.x;
            colliderOffset.x = nowLength / (2 * this.transform.localScale.x);

            // 新しいサイズをSpriteRendererに設定
            spriteRenderer.size = rendCurrentSize;
            boxCollider2D.size = colliderSize;
            boxCollider2D.offset = colliderOffset;

            nowLength += Time.deltaTime * 10;

            if(nowLength - initLength >= maxLength)
            {
                boxCollider2D.enabled = false;
            }
        }
        else if(leaveTime > 0)
        {
            leaveTime -= Time.deltaTime;
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }

}
