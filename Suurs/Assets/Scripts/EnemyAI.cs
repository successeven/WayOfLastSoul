using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public GameObject player;
    public float health;
    public float speed;
    public float attackForce;
    public enum Actions
    {
        Idle,
        MeleeAttack,
        RangeAttack,
        Death,
    }

    [Header("Текущее действие")]
    public Actions currentAction;
    [Header("Дистанция обнаружения игрока")]
    public float distanceRange = 7;
    public float distanceMelee = 2;
    public float distanceToStopAttack = 10;
  //  public float heightDiference = 1;
    [Header("Позиции движения")]
    public Transform[] positions;
    int currentAct = 0;
    int x = 0;

    [Header("Перерывы между атаками")]
    public float rangeCooldown = 4;
    public float meleeCooldown = 2;
    float rc = 0;
    float mc = 0;

    [Header("Префабы VFX")]
    public GameObject RangeAttackVFX;
    public GameObject MeleeAttackVFX;
    public GameObject DeathVFX;
    public Transform startPlaceVFX;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentAction = Actions.Idle;

    }

    // Update is called once per frame
    void Update()
    {
        BasicAIController();
        ChangeRotationOfEnemyLookForPlayer();
    }

    void BasicAIController()
    {
        if (health > 0)
            switch (currentAct)
            {
                case 0:
                    Idle();
                    break;
                case 1:
                    RangeAttack();
                    break;
                case 2:
                    MeleeAttack();
                    break;
            }

        else
            Death();
    }

    void Idle()
    {
        currentAction = Actions.Idle;
        if (positions[x] !=null)
        if (Vector2.Distance(transform.position, positions[x].transform.position) > 1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, positions[x].transform.position, speed / 100);
        }
        else
        {
            x++;
            if (x >= positions.Length)
                x = 0;

        }


        if (DistanceChecker() < distanceRange && DistanceChecker() > distanceMelee && DistanceChecker() < distanceToStopAttack)
        {
            float diference;
            float playerY = player.transform.position.y;
            float myY = transform.position.y;

            if (playerY > myY)
                diference = playerY - myY;
            else
                diference = myY - playerY;

            if (diference < 1)
            {
                currentAct = 1;
            }
        }

        if (DistanceChecker() < distanceMelee)
        {
            float diference;
            float playerY = player.transform.position.y;
            float myY = transform.position.y;

            if (playerY > myY)
                diference = playerY - myY;
            else
                diference = myY - playerY;

            if (diference < 1)
            {
                currentAct = 2;
            }
        }
        
    }




    void RangeAttack()
    {
        mc = 0;
        if (rc > 0)
            rc -= Time.deltaTime;
        else
        {
            GameObject rangeAttackPref = Instantiate(RangeAttackVFX);
            rangeAttackPref.transform.position = startPlaceVFX.transform.position;
            rangeAttackPref.transform.localScale = new Vector3(scaleChecker(), transform.localScale.y, transform.localScale.z);
            Destroy(rangeAttackPref, 1);
            rc = rangeCooldown;
        }

        currentAction = Actions.RangeAttack;

        if (DistanceChecker() < distanceMelee)
        {
            currentAct = 2;
        }

        if (DistanceChecker() > distanceRange && DistanceChecker() < distanceToStopAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed / 100);
        }

        ReturnToIdleForRange();
    }

    void MeleeAttack()
    {
        rc = 0;

        if (mc > 0)
            mc -= Time.deltaTime;
        else
        {

            GameObject meleeAttackPref = Instantiate(MeleeAttackVFX);
            meleeAttackPref.transform.position = startPlaceVFX.transform.position - new Vector3(scaleChecker(),0,0);
            meleeAttackPref.transform.localScale = new Vector3( scaleChecker(), transform.localScale.y, transform.localScale.z);
            Destroy(meleeAttackPref, 1);
            mc = meleeCooldown;
        }

        currentAction = Actions.MeleeAttack;

        if (DistanceChecker() > distanceMelee)
        {
            currentAct = 1;
        }

        ReturnToIdleForRange();
    }

    void ReturnToIdleForRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= distanceToStopAttack)
        {
            currentAction = Actions.Idle;
        }
    }

    void ChangeRotationOfEnemyLookForPlayer()
    {
        float playerX = player.transform.position.x;
        float myX = transform.position.x;
        if (playerX < myX)
            transform.rotation = new Quaternion(0, 0, 0, 0);
        else
            transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    float DistanceChecker()
    {
        float dist;
        dist = Vector2.Distance(transform.position, player.transform.position);
        return dist;
    }



    float scaleChecker()
    {
        float scale;
        float playerX = player.transform.position.x;
        float myX = transform.position.x;
        if (playerX < myX)
            scale = transform.localScale.x;
        else
            scale = -transform.localScale.x;
        return scale;
    }

    void Death()
    {
        currentAction = Actions.Death;
        GameObject dth = Instantiate(DeathVFX);
        dth.transform.position = startPlaceVFX.transform.position;
        dth.transform.localScale = new Vector3(scaleChecker(), transform.localScale.y, transform.localScale.z);
       Destroy(dth, 1);
        Destroy(gameObject);
    }
}