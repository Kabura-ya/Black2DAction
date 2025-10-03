using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Enemy1_2 : MonoBehaviour, IDamageable
{
    public GameObject nextObject;
    public GameObject nextObject2;
    public GameObject destroyObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage(int value, Vector2 vector, int type)
    {
        nextObject.SetActive(true);
        nextObject2.SetActive(true);
        Destroy(destroyObject);
        Destroy(this.gameObject);
    }
}
