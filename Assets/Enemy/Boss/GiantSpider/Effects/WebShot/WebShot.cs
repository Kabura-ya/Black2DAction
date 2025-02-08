using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShot : MonoBehaviour
{
    [SerializeField] private GroundCheck groundChecker = null;
    [SerializeField] private GameObject webTrap = null;
    private Rigidbody2D rb2D = null;
    [SerializeField] private float maxSpeed = 5;
    private float holSpeed = 0;
    private float verSpeed = 0;
    private Vector3 originScale = new Vector3(0, 0, 0);

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        originScale = this.transform.localScale;
        if (originScale.x < 0)
        {
            originScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
        if (originScale.y < 0)
        {
            originScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, this.transform.localScale.z);
        }
        if (originScale.z < 0)
        {
            originScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, -this.transform.localScale.z);
        }
    }

    void Update()
    {
        if(groundChecker.IsGround())
        {
            Instantiate(webTrap, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        verSpeed -= maxSpeed * Time.deltaTime;
        if(verSpeed < 0)
        {
            this.transform.localScale = new Vector3(originScale.x, -originScale.y, originScale.z);
        }
        if(verSpeed < -maxSpeed)
        {
            verSpeed = -maxSpeed;
        }

        rb2D.velocity = new Vector2(holSpeed, verSpeed);

        Vector2 derection = new Vector2(rb2D.velocity.x, rb2D.velocity.y).normalized;
        this.transform.rotation = Quaternion.FromToRotation(Vector2.right, derection);
    }

    public void Launch(Vector2 launchVec)
    {
        launchVec = launchVec.normalized;
        holSpeed = launchVec.x * maxSpeed;
        verSpeed = launchVec.y * maxSpeed;
    }
}
