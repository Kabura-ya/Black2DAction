using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Body1 : MonoBehaviour, IDamageable, IDrainable //�{�X�̓���
{
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)//Body�ɐG�ꂽ���̂�e�I�u�W�F�N�g�ɓn������
    {
        transform.root.gameObject.GetComponent<EnemyBase1>().BodyStay(collision);
    }

    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }

    public void Damage(int value, Vector2 vector, int type)
    {
        transform.root.gameObject.GetComponent<EnemyBase1>().Damage(value, vector, type);
        Debug.Log("EnemyBody_Damage");
    }

    public bool Drain()
    {
        return transform.root.gameObject.GetComponent<EnemyBase1>().Drain();
    }
}
