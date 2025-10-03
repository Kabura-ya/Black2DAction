using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SummonSpear : MonoBehaviour
{
    [SerializeField] private List<GameObject> summonedPoints = new List<GameObject>();
    [SerializeField] private GameObject spear = null;
    [SerializeField] private float summonInterval = 1.0f;
    private float nowSummonInterval = 1.0f;
    private int summonNum = 0;
    private GameObject lastSpear = null;

    private Transform playerTrans = null;

    public UnityEvent clearEvent;

    void Awake()
    {
        nowSummonInterval = 0;
        summonNum = 0;
    }

    void Start()
    {
        playerTrans = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = playerTrans.position;
        if (summonNum < summonedPoints.Count)
        {
            if (nowSummonInterval > 0)
            {
                nowSummonInterval -= Time.deltaTime;
            }
            else
            {
                GameObject generated = null;
                generated = Instantiate(spear, summonedPoints[summonNum].transform.position, summonedPoints[summonNum].transform.rotation);
                if(summonNum ==  summonedPoints.Count - 1)
                {
                    lastSpear = generated;
                }
                DestroyRegist(generated);
                summonNum++;
                nowSummonInterval = summonInterval;
            }
        }
        else
        {
            if(lastSpear == null)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnDestroy()
    {
        clearEvent.Invoke();
    }

    //攻撃オブジェクトは死亡、スタン時に消去させたい。
    public void DestroyRegist(GameObject attackObject)
    {
        clearEvent.AddListener(() => { Destroy(attackObject); });
    }
}
