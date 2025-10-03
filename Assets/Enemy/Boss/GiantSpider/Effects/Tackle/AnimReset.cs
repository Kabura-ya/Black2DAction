using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//表示されたらアニメーションをデフォルトステートに戻す。
public class AnimReset : MonoBehaviour
{
    private Animator anim = null;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        anim.Rebind();//表示されたら最初から再生できるように
    }
}
