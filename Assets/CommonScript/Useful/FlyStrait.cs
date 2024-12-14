using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyStrait : MonoBehaviour
{
    //前方（transform.right）にまっすぐ飛ばすためだけのスクリプト）
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public float speed = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = speed * transform.right;
    }
}
