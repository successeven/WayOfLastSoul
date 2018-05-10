using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(HeroManager))]
public class HeroController : Unit
{
    
    [SerializeField]
    float _rollLength = 20F;

    Rigidbody2D _rigidbody;

    [NonSerialized]
    public Animator _anima;

    [NonSerialized]
    public int _comboAttack = 1;

    float _lastAttackTime = 0;
    float _catchTime = .25f;
    bool _doubleAttack = false;
    bool _acingRight = true;

    [NonSerialized]
    public bool _jumping = false;

    [NonSerialized]
    public bool _attacks = false;
    [NonSerialized]
    public bool _blocking = false;
    float _lastJumpTime = 0;
    [NonSerialized]
    public bool _interfaceBlocked = true;
    HeroManager _manager;
    GameObject _GlobalhealthTriger;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anima = GetComponent<Animator>();
        _manager = GetComponent<HeroManager>();
        _lastJumpTime = Time.fixedTime;
    }

    private void Update()
    {
        if (_GlobalhealthTriger == null)
            _GlobalhealthTriger = GameObject.FindGameObjectWithTag("GlobalHealth");

        if (CrossPlatformInputManager.GetButtonDown("GlobalHealth"))
            HideGlobalHealth();

        if (CrossPlatformInputManager.GetButtonUp("GlobalHealth"))
           ShowGlobalHealth();

    }

    private void HideGlobalHealth()
    {
        _GlobalhealthTriger.SetActive(false);
    }

    private void ShowGlobalHealth()
    {
        _GlobalhealthTriger.SetActive(true);
    }

    public void Move( float Axis)
    {
        Move(_rigidbody, _speed, ref _acingRight, Axis);
        _anima.SetFloat("Speed", Axis);
    }

    private void FixedUpdate()
    {
        if (_manager._HP <= 0)
            return;

        if (_interfaceBlocked)
            return;

        if (Time.fixedTime - _lastAttackTime > 2f)
            _comboAttack = 1;

				if (_comboAttack > 2)
						_comboAttack = 2;


				if (!_attacks && !_jumping && !_blocking)
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            Move(_rigidbody, _speed, ref _acingRight, h);
            _anima.SetFloat("Speed", Mathf.Abs(h));
        }

        if (CrossPlatformInputManager.GetButtonDown("Attack") && !_jumping)
            Attack(_comboAttack);

        int deltaJump = (int)Math.Truncate((Time.fixedTime - _lastJumpTime) * 1000);
        if (CrossPlatformInputManager.GetButtonDown("Jump") && !_attacks && (deltaJump > _manager._DeltaRoll))
            Jump();

        if (CrossPlatformInputManager.GetButtonDown("Block"))
            SetBlock();

        if (CrossPlatformInputManager.GetButtonUp("Block"))
            UnSetBlock();
    }

    public void DisableAnima()
    {
        _anima.enabled = false;
    }

    private void SetBlock()
    {
        _blocking = true;
        _anima.SetTrigger("Block");
        _anima.SetBool("Blocking", _blocking);
    }

    private void UnSetBlock()
    {
        _blocking = false;
        _anima.SetBool("Blocking", _blocking);
    }
    private void Jump()
    {
        _jumping = true;
        _lastJumpTime = Time.fixedTime;
        _anima.SetFloat("Speed", 0);
        _anima.SetTrigger("Jump");
        _rigidbody.velocity = new Vector2(_rollLength * transform.localScale.x, 1);
    }

    private void Attack(float inTypeAttack)
    {
        _attacks = true;
        _anima.SetFloat("Speed", _comboAttack);
        _anima.SetFloat("Attack", inTypeAttack);
				_lastAttackTime = Time.fixedTime;

		}

    public void ResetAttack()
    {
        if (_attacks)
        {
            _manager.ResetHeroDealAttack();
            _anima.SetFloat("Attack", 0);
            _attacks = false;
        }
    }

    public void ResetJumping()
    {
        _jumping = false;
    }



}
