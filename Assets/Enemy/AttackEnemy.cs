using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 1;
    public float enableTime = 2f;//�U���R���C�_�[��L�������鎞��
    public Collider2D col;
    public Vector2 vector = new Vector2(10,5);
    public int damageType = 0;
    Coroutine disableTimeCoroutine;
    /*
    private void Update()
    {
        if (col.enabled)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;  // �g���K�[�R���C�_�[�����o����

            int numColliders = col.OverlapCollider(filter, results);

            for (int i = 0; i < numColliders; i++)
            {
                Debug.Log(results[i].gameObject.name + " �ɐG��Ă��܂�");
                if (results[i].gameObject.tag == "Enemy")
                {
                    var damageTarget = results[i].gameObject.GetComponent<IDamageable>();
                    if (damageTarget != null)
                    {
                        damageTarget.Damage(damage);
                    }
                }
            }
        }        
    }
    */

    public void EnableAttack()
    {
        col.enabled = true;
        disableTimeCoroutine = StartCoroutine(EnableCollider());
    }

    IEnumerator EnableCollider()
    {
        // �R���C�_�[��L����
        yield return new WaitForSeconds(enableTime);
        col.enabled = false;//DisableAttack()�ł��R���C�_�[�𖳌����ł��邪�A�o�O�Ȃǂ�DisableAttack���������Ȃ����̂��߂Ɉꉞ���Ԍo�߂ł�k���C�_�[�������ł���悤�ɂ��Ă���
    }

    public void DisableAttack()
    {
        if (disableTimeCoroutine != null) { StopCoroutine(disableTimeCoroutine); }//���Ԍo�߂Ŏ����I�ɃR���C�_�[�𖳌�������R���[�`���𒆎~
        disableTimeCoroutine = null;
        col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                damageTarget.Damage(damage, transform.right * vector.x + transform.up * vector.y, damageType);
            }
        }
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, col.size);
    }
    */
}
