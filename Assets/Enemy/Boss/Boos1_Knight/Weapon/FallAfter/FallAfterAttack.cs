using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallAfterAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public int attack = 1;
    public float beforeTime = 1.0f;
    public float afterTime = 1.0f;
    private bool onDamage = false;
    private Animator anim;//アニメーター

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Main());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Main()//最初の演出をする
    {
        yield return new WaitForSeconds(beforeTime);
        onDamage = true;
        anim.SetTrigger("onDamage");
        yield return new WaitForSeconds(afterTime);
        Destroy(this.gameObject);
    }


    public void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("FallAfterAttack_OnTrigger");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("OntrrigerEnter_Player");
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null && onDamage)
            {
                damageTarget.Damage(attack);
            }
        }
    }
}
