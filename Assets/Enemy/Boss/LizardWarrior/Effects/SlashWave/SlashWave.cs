using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashWave : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float distance = 10;

    private Vector2 startPos = new Vector2 (0, 0);
    private Rigidbody2D rb2D = null;
    [SerializeField] private GroundCheck groundChecker = null;

    void Awake()
    {
        startPos = this.transform.position;
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Vector2.Distance(startPos, this.transform.position) > distance || groundChecker.IsGround())
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        rb2D.velocity = transform.right * speed;
    }
}
