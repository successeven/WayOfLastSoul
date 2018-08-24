
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class HeroMotor : CharacterMotor
{
    enum AttackEnum
    {
        None = 0,
        BackSlide = 1,
        Rapira = 2,
        StrikeRoll = 3,
        Strike_1 = 4,
        Strike_2 = 5,
        Strike_3 = 6
    }

    MyFSM _fsm;


    [SerializeField]
    float _attacksLength = 2f;

    public PolygonCollider2D SwordCollider;

    [NonSerialized]
    public bool _attacks = false;
    [NonSerialized]
    public int _attacksIndex = 0;

    [NonSerialized]
    public bool _holdAttack = false;

    [NonSerialized]
    public float _lastAttackTime = 0;

    [NonSerialized]
    public bool _blocking = false;

    public bool _canMove = true;


    #region Roll
    [NonSerialized]
    public bool _rolling = false;
    [NonSerialized]
    public float _lastRollTime = 0;

    [SerializeField]
    float _rollLength = 20f;
    #endregion

    #region Rapira
    [SerializeField]
    public float _deltaRapiraTime = 1.5f;
    [SerializeField]
    public float _deltaRapiraLength = 1.5f;
    #endregion

    #region Back Slide
    [SerializeField]
    float _backSlideLength = 5f;
    [NonSerialized]
    public float _lastBack_SlideTime = 0;
    #endregion

    AttackEnum currentAttackEnum = AttackEnum.None;

    private void Start()
    {
        _fsm = new MyFSM(Moves);
        _fsm.AddStates((int)AttackEnum.BackSlide, Back_Slide);
        _fsm.AddStates((int)AttackEnum.Rapira, Rapira);
        _fsm.AddStates((int)AttackEnum.StrikeRoll, StrikeRoll);
        _fsm.AddStates((int)AttackEnum.Strike_1, Strike_1);
        _fsm.AddStates((int)AttackEnum.Strike_2, Strike_2);
        _fsm.AddStates((int)AttackEnum.Strike_3, Strike_3);
    }

    private void Update()
    {
        _fsm.Invoke();
    }

    public void Attack()
    {
        if (_rolling)
        {
            currentAttackEnum = AttackEnum.StrikeRoll;

        }
        else if (_deltaRapiraTime > Time.fixedTime - _lastAttackTime)
        {
            _lastAttackTime = Time.fixedTime;
            if (_fsm.GetCurrentState() == Moves)
            {
                currentAttackEnum = AttackEnum.Strike_1;
            }
            else if (_fsm.GetCurrentState() != Strike_3)
                currentAttackEnum++;
            else
                return;
        }
        else
            currentAttackEnum = AttackEnum.Rapira;

        _fsm.RunState((int)currentAttackEnum);
        if (_fsm.GetCurrentState() == Moves)
            _fsm.FinishState();
    }

    private IEnumerator DoStrikeRoll(float time)
    {
        _attacks = true;
        SwordCollider.enabled = true;
        Hero.instance.audioSource.Play();
        for (float t = 0; t <= time; t += Time.deltaTime)
            yield return null;

    }

    private IEnumerator DoAttack(AudioClip sound, float time)
    {
        _attacks = true;
        Hero.instance.audioSource.clip = sound;
        Hero.instance.audioSource.pitch = 1;
        Hero.instance.audioSource.loop = false;
        Hero.instance.audioSource.PlayOneShot(sound);
        SwordCollider.enabled = true;
        Debug.Log(Hero.instance.audioSource.isPlaying);
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            _rigidbody.velocity = new Vector2(_attacksLength * transform.localScale.x, _rigidbody.velocity.y);
            yield return null;
        }

    }

    private IEnumerator DoRapira(float time)
    {
        _attacks = true;
        Hero.instance.audioSource.Play();
        SwordCollider.enabled = true;
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            _rigidbody.velocity = new Vector2(_deltaRapiraLength * transform.localScale.x, _rigidbody.velocity.y);
            yield return null;
        }
    }

    private IEnumerator DoBack_Slide(float time)
    {
        _attacks = true;
        Hero.instance.audioSource.Play();
        SwordCollider.enabled = true;
        yield return new WaitForSeconds(0.15f);
        for (float t = 0; t <= time - 0.15f; t += Time.deltaTime)
        {
            _rigidbody.velocity = new Vector2(-_backSlideLength * transform.localScale.x, _rigidbody.velocity.y);
            yield return null;
        }
    }

    protected override bool CanMove()
    {
        return (!_attacks && !_rolling && !_blocking && _canMove);
    }

    public void ResetAttack()
    {
        SwordCollider.enabled = false;
        _fsm.FinishState();
        _attacks = false;
    }

    public void SetBlock()
    {
        _blocking = true;
        _anima.SetBool("Blocking", _blocking);
    }

    public void UnSetBlock()
    {
        _blocking = false;
        _anima.SetBool("Blocking", _blocking);
    }

    void Moves()
    {
        _anima.SetInteger("Attack Index", 0);
        _anima.SetBool("Attack", false);
    }

    void Rapira()
    {
        Hero.instance.audioSource.clip = Hero.instance.Manager._AttackRapira;
        Hero.instance.audioSource.pitch = 1;
        Hero.instance.audioSource.loop = false;
        if (_anima.GetInteger("Attack Index") != (int)AttackEnum.Rapira)
            StartCoroutine(DoRapira(.33f));
        _anima.SetInteger("Attack Index", (int)AttackEnum.Rapira);
        _anima.SetBool("Attack", true);
    }

    void StrikeRoll()
    {
        Hero.instance.audioSource.clip = Hero.instance.Manager._AttackStrike_2;
        Hero.instance.audioSource.pitch = 1;
        Hero.instance.audioSource.loop = false;
        if (_anima.GetInteger("Attack Index") != (int)AttackEnum.StrikeRoll)
            StartCoroutine(DoStrikeRoll(.37f));
        _anima.SetInteger("Attack Index", (int)AttackEnum.StrikeRoll);
        _anima.SetBool("Attack", true);

    }

    Coroutine AttackCoroutine;
    void Strike_1()
    {
        if (_anima.GetInteger("Attack Index") != (int)AttackEnum.Strike_1)
        {
            Debug.Log("StartCoroutine Strike_1");
            //StopCoroutine(AttackCoroutine);
            AttackCoroutine = StartCoroutine(DoAttack(Hero.instance.Manager._AttackStrike_1, 0.9f));
        }
        _anima.SetInteger("Attack Index", (int)AttackEnum.Strike_1);
        _anima.SetBool("Attack", true);
    }

    void Strike_2()
    {
        if (_anima.GetInteger("Attack Index") != (int)AttackEnum.Strike_2)
        {
            Debug.Log("StartCoroutine Strike_2");

            StopCoroutine(AttackCoroutine);
            AttackCoroutine = StartCoroutine(DoAttack(Hero.instance.Manager._AttackStrike_2, .03f));
        }
        _anima.SetInteger("Attack Index", (int)AttackEnum.Strike_2);
        _anima.SetBool("Attack", true);
    }

    void Strike_3()
    {
        if (_anima.GetInteger("Attack Index") != (int)AttackEnum.Strike_3)
        {
            Debug.Log("StartCoroutine Strike_3");
            StopCoroutine(AttackCoroutine);
            AttackCoroutine = StartCoroutine(DoAttack(Hero.instance.Manager._AttackStrike_3, .4f));
        }
        _anima.SetInteger("Attack Index", (int)AttackEnum.Strike_3);
        _anima.SetBool("Attack", true);
    }

    public void Back_Slide()
    {
        _attacks = true;
        Hero.instance.audioSource.clip = Hero.instance.Manager._AttackBack_Slide;
        Hero.instance.audioSource.pitch = 1;
        Hero.instance.audioSource.loop = false;
        _lastBack_SlideTime = Time.fixedTime;
        _attacks = true;
        if (_anima.GetInteger("Attack Index") != (int)AttackEnum.BackSlide)
            StartCoroutine(DoBack_Slide(.45f));
        _anima.SetInteger("Attack Index", (int)AttackEnum.BackSlide);
        _anima.SetBool("Attack", true);
        _blocking = false;
    }

    public void Roll()
    {
        Hero.instance.audioSource.clip = Hero.instance.Manager._RollSound;
        Hero.instance.audioSource.pitch = 1;
        Hero.instance.audioSource.loop = false;
        Hero.instance.audioSource.Play();
        _rolling = true;
        _lastRollTime = Time.fixedTime;
        _anima.SetTrigger("Roll");
        StartCoroutine(DoRolling(1f));
    }

    private IEnumerator DoRolling(float time)
    {
        Physics.IgnoreLayerCollision(9, 11, true);
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            _rigidbody.velocity = new Vector2(_rollLength * transform.localScale.x, _rigidbody.velocity.y);
            yield return null;
        }
        _rolling = false;
        Physics.IgnoreLayerCollision(9, 11, false);
    }
}
