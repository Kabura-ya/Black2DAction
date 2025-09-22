using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour, IDrainable
{
    // Start is called before the first frame update
    public GameObject nextObject1;
    public GameObject nextObject2;
    public GameObject destroyObject;
    public string damageTag = "Player";
    public int attack = 1;
    public int type = 0;
    public Vector2 vector;//ノックバックの方向。ただし、このスクリプトをつけたオブジェクトの向きを基準に方向を定めるために、transform.right * vector.x + transform.up * vector.yの方向にノックバックする
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

    public bool Drain()
    {
        nextObject1.SetActive(true);
        nextObject2.SetActive(true);
        Destroy(destroyObject);
        Destroy(gameObject);
        return true;
    }

    public bool SuperDrain()
    {
        Destroy(gameObject);
        return true;
    }
}
