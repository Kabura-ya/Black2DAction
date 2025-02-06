using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTrap : MonoBehaviour
{
    [SerializeField] private float existTime = 2;

    void Update()
    {
        existTime -= Time.deltaTime;
        if( existTime < 0 )
        {
            Destroy(this.gameObject);
        }
    }
}
