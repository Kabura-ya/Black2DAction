using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//毎フレームRayCastするので多分重い。使いすぎ厳禁。
public class WebBeem : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayer;
    [Header("回転限度"), SerializeField] private float maxAngle = 360;
    private float nowAngle = 0;
    [Header("1秒で何度回転するか"), SerializeField] private float rotateSpeed = 180;
    [Header("限界長さ"), SerializeField] private float maxDistance = 20;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private BoxCollider2D boxCollider2D = null;

    void Awake()
    {
        nowAngle = 0;
    }

    void Update()
    {
        float distance = 0;

        // Rayを発射し、衝突情報を取得
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, transform.right, maxDistance, hitLayer);

        // 衝突があった場合
        if (hit.collider != null)
        {
            // 衝突位置
            Vector2 hitPoint = hit.point;
            distance = Vector2.Distance(this.transform.position, hitPoint);
        }

        // 現在のサイズを取得
        Vector2 rendCurrentSize = spriteRenderer.size;
        Vector2 colliderSize = boxCollider2D.size;
        Vector2 colliderOffset = boxCollider2D.offset;

        // 新しいサイズを設定
        rendCurrentSize.x = distance / this.transform.localScale.x;
        colliderSize.x = distance / this.transform.localScale.x;
        colliderOffset.x = distance / (2 * this.transform.localScale.x);

        // 新しいサイズをSpriteRendererに設定
        spriteRenderer.size = rendCurrentSize;
        boxCollider2D.size = colliderSize;
        boxCollider2D.offset = colliderOffset;

        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        nowAngle += rotateSpeed * Time.deltaTime;
        if(nowAngle >= maxAngle)
        {
            Destroy(this.gameObject);
        }
    }
}
