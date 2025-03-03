using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float speed = 5f; // 追従速度
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0f; // 2DではZ軸を0に

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;
    }
}
