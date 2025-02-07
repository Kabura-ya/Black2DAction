using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Body : MonoBehaviour, IDamageable, IDrainable, IDashHit //ボスの胴体
{
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)//Bodyに触れたものを親オブジェクトに渡すだけ
    {
        transform.root.gameObject.GetComponent<Boss1>().BodyStay(collision);
    }

    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }

    public void Damage(int value, Vector2 vector, int type)
    {
        transform.root.gameObject.GetComponent<Boss1>().Damage(value, vector, type);
    }

    public bool Drain()
    {
        return transform.root.gameObject.GetComponent<Boss1>().Drain();
    }

    public bool SuperDrain()
    {
        return transform.root.gameObject.GetComponent<Boss1>().SuperDrain();
    }
    public void NormalDashHit()
    {
        transform.root.gameObject.GetComponent<Boss1>().NormalDashHit();
    }
    public void SuperDashHit()
    {
        transform.root.gameObject.GetComponent<Boss1>().SuperDashHit();
    }
}
