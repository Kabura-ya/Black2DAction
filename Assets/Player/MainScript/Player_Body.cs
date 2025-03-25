using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Body : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    //public void Damage(int value) { Damage(value, Vector2.zero); }
    //public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }
    public void Damage(int value, Vector2 vector, int type)
    {
        transform.root.gameObject.GetComponent<Player>().Damage(value, vector, type);
    }
    private void OnTriggerEnter2D(Collider2D collision)//Body部分に触れたらそのまま親オブジェクトのスクリプトに情報を送る
    {
        Debug.Log("Player_OnTrigger");
        transform.root.gameObject.GetComponent<Player>().BodyEnter(collision);
    }
}
