using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestryTime : MonoBehaviour
{
    // Start is called before the first frame update
    public float time = 3;
    void Start()
    {
        StartCoroutine(DestroyTime());
    }

    private IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
