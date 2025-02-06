using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleEffect : MonoBehaviour
{
    private Animator anim = null;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        anim.Rebind();//•\¦‚³‚ê‚½‚çÅ‰‚©‚çÄ¶‚Å‚«‚é‚æ‚¤‚É
    }
}
