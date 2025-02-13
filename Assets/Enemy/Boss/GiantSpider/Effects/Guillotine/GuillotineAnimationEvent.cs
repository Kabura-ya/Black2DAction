using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuillotineAnimationEvent : MonoBehaviour
{
    [SerializeField] private Collider2D collider2D = null;

    void Awake()
    {
        AttackOff();
    }

    void AttackOn()
    {
        collider2D.enabled = true;
    }
    void AttackOff()
    {
        collider2D.enabled = false;
    }

    void Finish()
    {
        Destroy(this.gameObject);
    }
}
