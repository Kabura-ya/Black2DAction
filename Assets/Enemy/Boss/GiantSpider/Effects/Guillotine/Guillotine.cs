using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guillotine : MonoBehaviour, IDrainable
{
    [SerializeField] private int power = 1;
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            IDamageable idamageable = collider2D.gameObject.GetComponent<IDamageable>();
            if (idamageable != null)
            {
                idamageable.Damage(power, Vector2.zero , 2);
            }
        }
    }

    public bool Drain()
    {
        return false;
    }
    public bool SuperDrain()
    {
        return false;
    }
}
