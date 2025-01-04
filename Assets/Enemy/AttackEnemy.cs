using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 1;
    public float enableTime = 0.4f;//�U���R���C�_�[��L�������鎞��
    public Collider2D col;
    public Vector2 vector = Vector2.zero;
    public int damageType = 0;
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
        StartCoroutine(EnableCollider());
    }

    IEnumerator EnableCollider()
    {
        // �R���C�_�[��L����
        yield return new WaitForSeconds(enableTime);
        col.enabled = false;//DisableAttack()�ł��R���C�_�[�𖳌����ł��邪�A�o�O�Ȃǂ�DisableAttack���������Ȃ����̂��߂Ɉꉞ���Ԍo�߂ł�k���C�_�[�������ł���悤�ɂ��Ă���
    }

    public void DisableAttack()
    {
        col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                damageTarget.Damage(damage, vector, damageType);
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
