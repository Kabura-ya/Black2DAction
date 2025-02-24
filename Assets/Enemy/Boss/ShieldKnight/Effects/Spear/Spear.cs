using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���t���[��RayCast����̂ő����d���B�g���������ցB
public class Spear : MonoBehaviour
{
    [Header("�ҋ@����"), SerializeField] private float waitTime = 1;
    [Header("�c������"), SerializeField] private float leaveTime = 1;
    [Header("���E����"), SerializeField] private float maxLength = 5;
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

        // �V�����T�C�Y��ݒ�
        rendCurrentSize.x = nowLength / this.transform.localScale.x;
        colliderSize.x = nowLength / this.transform.localScale.x;
        colliderOffset.x = nowLength / (2 * this.transform.localScale.x);

        // �V�����T�C�Y��SpriteRenderer�ɐݒ�
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

            // ���݂̃T�C�Y���擾
            Vector2 rendCurrentSize = spriteRenderer.size;
            Vector2 colliderSize = boxCollider2D.size;
            Vector2 colliderOffset = boxCollider2D.offset;

            // �V�����T�C�Y��ݒ�
            rendCurrentSize.x = nowLength / this.transform.localScale.x;
            colliderSize.x = nowLength / this.transform.localScale.x;
            colliderOffset.x = nowLength / (2 * this.transform.localScale.x);

            // �V�����T�C�Y��SpriteRenderer�ɐݒ�
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
