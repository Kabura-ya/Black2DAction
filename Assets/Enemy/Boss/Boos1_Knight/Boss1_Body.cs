using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Body : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)//Body�ɐU�ꂽ���̂�e�I�u�W�F�N�g�ɓn������
    {
        transform.root.gameObject.GetComponent<Boss1>().BodyStay(collision);
    }

    public void Damage(int damage)
    {
        transform.root.gameObject.GetComponent<Boss1>().Damage(damage);
    }
}
