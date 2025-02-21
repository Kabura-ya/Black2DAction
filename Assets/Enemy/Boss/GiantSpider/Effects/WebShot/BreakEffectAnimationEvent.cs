using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEffectAnimationEvent : MonoBehaviour
{
    public void Break()
    {
        Destroy(this.gameObject);
    }
}
