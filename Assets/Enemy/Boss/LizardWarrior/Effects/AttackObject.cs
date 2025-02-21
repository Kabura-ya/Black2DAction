using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻撃オブジェクトの基本的な設定詰め込んだもの。ドレイン時に特殊な機能がないものならこれをつければいい。
public class AttackObject : MonoBehaviour, IDrainable
{
    [SerializeField] private int power = 1;
    [SerializeField] private Vector2 hitVector = new Vector2 (0, 0);
    [SerializeField] private int type = 0;
    [SerializeField] private bool canDrain = false;
    [SerializeField] private bool canSPDrain = false;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            IDamageable idamageable = collider2D.gameObject.GetComponent<IDamageable>();
            if (idamageable != null)
            {
                idamageable.Damage(power, hitVector, type);
            }
        }
    }

    public bool Drain()
    {
        return canDrain;
    }
    public bool SuperDrain()
    {
        return canSPDrain;
    }
}
