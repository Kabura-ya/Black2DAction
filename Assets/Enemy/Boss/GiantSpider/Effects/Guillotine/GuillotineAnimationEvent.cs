using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuillotineAnimationEvent : MonoBehaviour
{
    private Collider2D col2D = null;

    void Awake()
    {
        col2D = GetComponent<Collider2D>();
        AttackOff();
    }

    void AttackOn()
    {
        col2D.enabled = true;
    }
    void AttackOff()
    {
        col2D.enabled = false;
    }

    void Finish()
    {
        Destroy(this.gameObject);
    }
}
