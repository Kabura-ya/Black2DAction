using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ChaseWalk : EnemyBase1
{
    public SightEnemy sightEnemy;//ここにプレイヤーが入ると追跡開始
    private void Start()
    {
        base.Start();
        StartCoroutine(Idle());
    }

    private void Update()
    {
        base.Update();
    }

    private IEnumerator Idle()
    {
        while (true)
        {
            if (sightEnemy.IsPlayerinSight())
            {
                StartCoroutine(Chase());
                yield break;
            }
            yield return null;
        }
    }
    private IEnumerator Chase()
    {
        while(true){
            FlipToPlayer();
            Debug.Log("Enemy_ChaseWalk Update Test");
            rigidbody2d.AddForce(transform.right * speed);
            yield return null;
        }
    }
}
