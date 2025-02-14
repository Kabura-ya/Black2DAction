using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorAnimationEvent : MonoBehaviour
{
    [SerializeField] private LizardWarriorAttack lizardWarriorAttack = null;

    void Awake()
    {

    }

    void PreDead()
    {

    }
    void Dead()
    {
        Destroy(transform.root.gameObject);
    }
}
