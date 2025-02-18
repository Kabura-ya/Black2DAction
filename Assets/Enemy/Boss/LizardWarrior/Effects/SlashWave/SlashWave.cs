using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashWave : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float existTime = 2;
    private Rigidbody2D rb2D = null;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        existTime -= Time.deltaTime;
        if(existTime < 0)
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        rb2D.velocity = transform.right * speed;
    }
}
