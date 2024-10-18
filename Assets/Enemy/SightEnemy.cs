using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightEnemy : MonoBehaviour
{
    private bool isPlayerInCollider = false;//プレイヤーがコライダーの中にいればtrue
    // Start is called before the first frame update
    public Collider2D sight;//視界を表すコライダー

    void OnTriggerEnter2D(Collider2D collision)
    {
        //コライダーにプレイヤーが入った事を記録
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInCollider = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        //コライダーからプレイヤーが出た事を記録
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInCollider = false;
        }
    }
    public bool IsPlayerinSight()
    {
        return isPlayerInCollider;
    }
}
