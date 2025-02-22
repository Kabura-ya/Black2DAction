using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorPattern : MonoBehaviour
{
    [SerializeField] private LizardWarriorAttack lizardWarriorAttack = null;
    [SerializeField] private LizardWarriorEffect lizardWarriorEffect = null;
    [SerializeField] private LizardWarriorStatus lizardWarriorStatus = null;
    [SerializeField] private GroundCheck bottomGroundChecker = null;
    [SerializeField] private LizardWarriorMove lizardWarriorMove = null;

    private float runTime = 0;
    private float coolTime = 0;
    private float feintTime = 0;
    private int rushCount = 3;
    private List<int> patternRoot = new List<int>();

    void Awake()
    {
        runTime = lizardWarriorStatus.RunTime;
        coolTime = 0;
        feintTime = lizardWarriorStatus.FeintTime;
        rushCount = lizardWarriorStatus.RushCount;
        patternRoot = GenerateRoot();
    }

    void Update()
    {
        if (lizardWarriorStatus.IsPreSpawn())
        {
            if (bottomGroundChecker.IsGround())
            {
                lizardWarriorStatus.GroundTrigger();
            }
        }
        else if (bottomGroundChecker.IsGround() && lizardWarriorStatus.IsStand())
        {
            if (rushCount > 0)
            {
                SelectPattern();
                rushCount--;
                if (rushCount <= 0)
                {
                    coolTime = lizardWarriorStatus.CoolTime;
                }
            }
            else
            {
                coolTime -= Time.deltaTime;
                if (coolTime <= 0)
                {
                    patternRoot = GenerateRoot();
                    rushCount = lizardWarriorStatus.RushCount;
                }
            }
        }
        else if (lizardWarriorStatus.IsRun())
        {
            runTime -= Time.deltaTime;
            if (runTime <= 0 || IsClose())
            {
                lizardWarriorStatus.UpperTrigger();
                lizardWarriorEffect.RunOff();
            }
        }
        else if (lizardWarriorStatus.IsPressJump())
        {
            if (lizardWarriorMove.IsReach())
            {
                lizardWarriorStatus.PressTrigger();
                lizardWarriorAttack.PressOn();
            }
        }
        else if (bottomGroundChecker.IsGround() && lizardWarriorStatus.IsPress())
        {
            lizardWarriorStatus.GroundTrigger();
            lizardWarriorAttack.PressOff();
        }
        else if (lizardWarriorStatus.IsFeint())
        {
            feintTime -= Time.deltaTime;
            if (feintTime <= 0)
            {
                lizardWarriorStatus.PunchOn();
                lizardWarriorEffect.FeintOff();
                lizardWarriorAttack.PunchOn();
                feintTime = lizardWarriorStatus.FeintTime;
            }
        }
        else if (lizardWarriorStatus.IsPunch())
        {
            feintTime -= Time.deltaTime;
            if (feintTime <= 0)
            {
                lizardWarriorStatus.PunchOff();
                lizardWarriorAttack.PunchOff();
            }
        }
        else if (lizardWarriorStatus.IsPostTailBlade())
        {
            if (bottomGroundChecker.IsGround())
            {
                lizardWarriorStatus.GroundTrigger();
            }
        }
        else if (lizardWarriorStatus.IsSmashJump())
        {
            if (lizardWarriorMove.IsReach())
            {
                lizardWarriorStatus.SmashTrigger();
                lizardWarriorEffect.SmashJumpOff();
                lizardWarriorAttack.SmashOn();
            }
        }
        else if (bottomGroundChecker.IsGround() && lizardWarriorStatus.IsSmash())
        {
            lizardWarriorStatus.GroundTrigger();
            lizardWarriorAttack.SmashOff();
            lizardWarriorAttack.SmashSlashWave2();
        }
    }

    //プレイヤーとの距離によって技が変わるようにしたい
    private bool IsClose()
    {
        return Mathf.Abs(this.transform.position.x - lizardWarriorStatus.PlayerTrans.position.x) <= 4;
    }
    private bool IsMiddle()
    {
        float distance = Mathf.Abs(this.transform.position.x - lizardWarriorStatus.PlayerTrans.position.x);
        return distance > 4 && distance <= 8;
    }
    private bool IsFar()
    {
        return Mathf.Abs(this.transform.position.x - lizardWarriorStatus.PlayerTrans.position.x) > 8;
    }

    // Fisher-Yates シャッフルアルゴリズム
    void Shuffle(List<int> list)
    {
        System.Random rand = new System.Random();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]); // 要素をスワップ
        }
    }
    private List<int> GenerateRoot()
    {
        List<int> result = new List<int> { 0, 1, 2 };
        Shuffle(result);
        return result;
    }

    private void SelectPattern()
    {
        int randomNumber = patternRoot[0];
        patternRoot.RemoveAt(0);
        if (IsClose())
        {
            if (randomNumber == 0)
            {
                lizardWarriorStatus.ClawTrigger();
            }
            else if (randomNumber == 1)
            {
                lizardWarriorStatus.FeintTrigger();
                lizardWarriorEffect.FeintOn();
                feintTime = lizardWarriorStatus.FeintTime;
            }
            else if (randomNumber == 2)
            {
                lizardWarriorStatus.TailBladeTrigger();
                lizardWarriorMove.TailBladeUp();
            }
        }
        else if (IsMiddle())
        {
            if (randomNumber == 0)
            {
                lizardWarriorStatus.PrePressTrigger();
            }
            else if (randomNumber == 1)
            {
                lizardWarriorStatus.BackSlashTrigger();
            }
            else if (randomNumber == 2)
            {
                lizardWarriorStatus.PowerSlashTrigger();
            }
        }
        else if (IsFar())
        {
            if (randomNumber == 0)
            {
                lizardWarriorStatus.RunTrigger();
                runTime = lizardWarriorStatus.RunTime;
            }
            else if (randomNumber == 1)
            {
                lizardWarriorStatus.SlashTrigger();
            }
            else if (randomNumber == 2)
            {
                lizardWarriorStatus.PreSmashTrigger();
            }
        }
    }
}
