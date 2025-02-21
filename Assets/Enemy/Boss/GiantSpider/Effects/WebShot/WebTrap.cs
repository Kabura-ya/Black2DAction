using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTrap : MonoBehaviour, IDrainable
{
    [SerializeField] private int power = 1;
    [SerializeField] private GameObject breakedWeb = null;
    [SerializeField] private float existTime = 2;

    void Update()
    {
        existTime -= Time.deltaTime;
        if( existTime < 0 )
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            IDamageable idamageable = collider2D.gameObject.GetComponent<IDamageable>();
            if (idamageable != null)
            {
                idamageable.Damage(power);
            }
        }
        else if (collider2D.gameObject.tag == "PlayerAttack")
        {
            GameObject breaked = Instantiate(breakedWeb, this.transform.position, Quaternion.identity);
            breaked.transform.localScale = this.transform.localScale;
            Destroy(this.gameObject);
        }
    }

    public bool Drain()
    {
        Destroy(this.gameObject);
        GameObject breaked = Instantiate(breakedWeb, this.transform.position, Quaternion.identity);
        breaked.transform.localScale = this.transform.localScale;
        return true;
    }
    public bool SuperDrain()
    {
        Destroy(this.gameObject);
        GameObject breaked = Instantiate(breakedWeb, this.transform.position, Quaternion.identity);
        breaked.transform.localScale = this.transform.localScale;
        return true;
    }
}
