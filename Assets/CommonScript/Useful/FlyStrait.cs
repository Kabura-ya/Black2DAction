using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyStrait : MonoBehaviour
{
    //�O���itransform.right�j�ɂ܂�������΂����߂����̃X�N���v�g�j
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
