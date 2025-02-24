using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���t���[��RayCast����̂ő����d���B�g���������ցB
public class WebBeem : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayer;
    [Header("��]���x"), SerializeField] private float maxAngle = 360;
    private float nowAngle = 0;
    [Header("1�b�ŉ��x��]���邩"), SerializeField] private float rotateSpeed = 180;
    [Header("���E����"), SerializeField] private float maxDistance = 20;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private BoxCollider2D boxCollider2D = null;

    void Awake()
    {
        nowAngle = 0;
    }

    void Update()
    {
        float distance = 0;

        // Ray�𔭎˂��A�Փˏ����擾
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, transform.right, maxDistance, hitLayer);

        // �Փ˂��������ꍇ
        if (hit.collider != null)
        {
            // �Փˈʒu
            Vector2 hitPoint = hit.point;
            distance = Vector2.Distance(this.transform.position, hitPoint);
        }

        // ���݂̃T�C�Y���擾
        Vector2 rendCurrentSize = spriteRenderer.size;
        Vector2 colliderSize = boxCollider2D.size;
        Vector2 colliderOffset = boxCollider2D.offset;

        // �V�����T�C�Y��ݒ�
        rendCurrentSize.x = distance / this.transform.localScale.x;
        colliderSize.x = distance / this.transform.localScale.x;
        colliderOffset.x = distance / (2 * this.transform.localScale.x);

        // �V�����T�C�Y��SpriteRenderer�ɐݒ�
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
