using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public string damageTag = "Player";
    public int attack = 1;
    public int type = 0;
    public Vector2 vector;//�m�b�N�o�b�N�̕����B�������A���̃X�N���v�g�������I�u�W�F�N�g�̌�������ɕ������߂邽�߂ɁAtransform.right * vector.x + transform.up * vector.y�̕����Ƀm�b�N�o�b�N����
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Damage_OnTrigger");
        if (collision.gameObject.tag == damageTag)
        {
            //Debug.Log("OntrrigerEnter_Player");
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                damageTarget.Damage(attack, transform.right * vector.x + transform.up * vector.y, type);
            }
        }
    }
}
