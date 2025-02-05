using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
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
}
