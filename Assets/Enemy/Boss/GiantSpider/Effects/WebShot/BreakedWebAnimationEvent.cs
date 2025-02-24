using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakedWebAnimationEvent : MonoBehaviour
{
    public void Break()
    {
        Destroy(this.gameObject);
    }
}
