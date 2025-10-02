using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour, IDamageable
{
    public GameObject damageEffect;
    public GameObject nextObject1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int value, Vector2 vector, int type)
    {
        Debug.Log("Damage(int value, Vector2 vector, int type)");
        if (value >= 15)//ƒGƒlƒ‹ƒM[‹Z
        {
            Instantiate(damageEffect, transform.position, transform.rotation);
            nextObject1.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
