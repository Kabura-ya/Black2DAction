using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShot : MonoBehaviour, IDrainable
{
    [SerializeField] private int power = 1;
    [SerializeField] private GameObject webTrap = null;
    private Rigidbody2D rb2D = null;
    [SerializeField] private float maxSpeed = 5;
    private float holSpeed = 0;
    private float verSpeed = 0;
    private Vector3 originScale = new Vector3(0, 0, 0);

    GiantSpiderAttack giantSpiderAttack = null;

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

        Vector2 initderection = transform.right;
        holSpeed = initderection.x * maxSpeed;
        verSpeed = initderection.y * maxSpeed;
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

    public void RegistGSA(GiantSpiderAttack giantSpiderAttack)
    {
        this.giantSpiderAttack = giantSpiderAttack;
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
            GameObject trap = Instantiate(webTrap, this.transform.position, Quaternion.identity);
            giantSpiderAttack.DestroyRegist(trap);
            Destroy(this.gameObject);
        }
        else if (collider2D.gameObject.tag == "Ground")
        {
            GameObject trap = Instantiate(webTrap, this.transform.position, Quaternion.identity);
            giantSpiderAttack.DestroyRegist(trap);
            Destroy(this.gameObject);
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
