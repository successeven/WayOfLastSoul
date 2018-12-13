using System;
using System.Collections;
using UnityEngine;

public class HeroMotor : CharacterMotor {
    enum StatsEnum {
        None = 0,
        Uppercut = 1,
        Shield_Attack = 2,
        Strike_1 = 4,
        Strike_2 = 5,
        Strike_3 = 6
    }

    MyFSM _fsm;

    public int AttackIndex {
        get {
            return _anima.GetInteger ("Attack Index");
        }
    }

    [SerializeField]
    float _attacksLength = 2f;

    [NonSerialized]
    public bool _attacks = false;

    [NonSerialized]
    public bool _jump = false;

    [NonSerialized]
    public bool _dodge = false;

    float _lastAttackTime = 0;

    [HideInInspector]
    public float LastAttackTime {
        get {
            return _blocking ? Time.fixedTime : _lastAttackTime;
        }
        set {
            _lastAttackTime = value;
        }
    }

    [NonSerialized]
    public bool _blocking = false;

    public bool _canMove = true;

    #region Shield_Attack
    [Space (10)]
    [SerializeField]
    public float _shield_AttackTime = 1.5f;
    [SerializeField]
    float _shield_AttackLength = 1.5f;
    [SerializeField]
    float _shield_AttackHeight = 1.5f;
    [SerializeField]
    float _shield_AttackForceX = 1.5f;
    #endregion

    #region Dodge
    bool _isDodging = false;
    [Space (10)]
    [SerializeField]
    float _dodgingTime = 0.5f;
    [SerializeField]
    float _dodgeLength = 1.5f;
    [SerializeField]
    float _dodgeForceX = 5f;
    [SerializeField]
    float _dodgeHeight = 5f;
    [NonSerialized]
    public float _lastDodgeTime = 0;

    #endregion

    StatsEnum currentAttackEnum = StatsEnum.None;

    private void Start () {
        _fsm = new MyFSM (Moves);
        _fsm.AddStates ((int) StatsEnum.Shield_Attack, Shield_Attack);
        _fsm.AddStates ((int) StatsEnum.Strike_1, Strike_1);
        _fsm.AddStates ((int) StatsEnum.Strike_2, Strike_2);
        _fsm.AddStates ((int) StatsEnum.Strike_3, Strike_3);
        _fsm.AddStates ((int) StatsEnum.Uppercut, DoUppercut);
    }

    private void FixedUpdate () {
        if (Hero.instance.Manager._Health > 0) {
            _fsm.Invoke ();
            CheckGround ();
        }
    }

    public void FinishAllAttacks () {
        _attacks = false;
        _fsm.FinishAllStates ();
    }

    protected override bool CanMove () {
        return (!_attacks && !_isDodging && !_blocking && _canMove);
    }

    public void ApplyGravityScale () {
        _rigidbody.gravityScale = 10;
    }

    public void ShieldAttack () {
        _fsm.RunState ((int) StatsEnum.Shield_Attack);
        if (_fsm.GetCurrentState () == Moves)
            _fsm.FinishState ();
    }

    int comboCount = 0;
    public void Attack () {

        if (_blocking)
            return;

        if (comboCount == 0)
            comboCount = (int) StatsEnum.Strike_1;
        else if (comboCount < (int) StatsEnum.Strike_3)
            comboCount++;
        else
            return;

        _fsm.RunState (comboCount);
        if (_fsm.GetCurrentState () == Moves)
            _fsm.FinishState ();
    }

    void Moves () {
        comboCount = 0;
        _attacks = false;
        currentAttackEnum = StatsEnum.None;

        if (_blocking || _isDodging)
            return;

        if (m_Grounded && _jump) {
            _jump = false;
            m_Grounded = false;
            Hero.instance.audioManager.Stop (Hero.AudioClips.Run.ToString ());
            Hero.instance.audioManager.Play (Hero.AudioClips.Jump.ToString ());
						_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
						_rigidbody.AddForce (new Vector2 (_rigidbody.velocity.x, _JumpForce), ForceMode2D.Impulse);
            _anima.SetTrigger ("Jump");
            return;
        }

        if (m_Grounded && _dodge) {
            _dodge = false;
            if (!_isDodging) {
                StartCoroutine (DoDodge (_dodgingTime, transform.position));
                _anima.SetTrigger ("Dodge");
                return;
            }
        }
        Move ();
    }

    public void SoundsLanded () {
        Hero.instance.audioManager.Play (Hero.AudioClips.Respawn.ToString ());
    }

    public bool CanBreakAnim () {
        if (currentAttackEnum == StatsEnum.Strike_3 ||
            currentAttackEnum == StatsEnum.Uppercut ||
            currentAttackEnum == StatsEnum.Shield_Attack)
            return false;
        else
            return true;
    }

    public void ResetState () {
        _anima.SetInteger ("Attack Index", 0);
        _anima.SetBool ("Attack", false);
        _fsm.FinishState ();
    }

    public void SetBlock () {
        _fsm.FinishAllStates ();
        _blocking = true;
        _anima.SetBool ("Blocking", _blocking);
    }

    public void UnSetBlock () {
        _blocking = false;
        _anima.SetBool ("Blocking", _blocking);
    }

    public void OnLanding () {
        _anima.SetBool ("IsFly", !m_Grounded);
    }

    void Shield_Attack () {
        if (currentAttackEnum != StatsEnum.Shield_Attack) {
            _fsm.FinishAllStates ();
            StartCoroutine (DoShield_Attack (1.06f, transform.position));
        }
        currentAttackEnum = StatsEnum.Shield_Attack;
        _anima.SetInteger ("Attack Index", (int) currentAttackEnum);
        _anima.SetBool ("Attack", true);
    }

    Coroutine AttackCoroutine;
    void Strike_1 () {
        if (currentAttackEnum != StatsEnum.Strike_1) {
            AttackCoroutine = StartCoroutine (DoAttack (Hero.AudioClips.Strike_1.ToString (), 0.6f));
        }
        currentAttackEnum = StatsEnum.Strike_1;
        _anima.SetInteger ("Attack Index", (int) currentAttackEnum);
        _anima.SetBool ("Attack", true);
    }

    void Strike_2 () {
        if (currentAttackEnum != StatsEnum.Strike_2) {
            StopCoroutine (AttackCoroutine);
            AttackCoroutine = StartCoroutine (DoAttack (Hero.AudioClips.Strike_2.ToString (), .03f));
        }
        currentAttackEnum = StatsEnum.Strike_2;
        _anima.SetInteger ("Attack Index", (int) currentAttackEnum);
        _anima.SetBool ("Attack", true);
    }

    void Strike_3 () {
        if (currentAttackEnum != StatsEnum.Strike_3) {
            StopCoroutine (AttackCoroutine);
            AttackCoroutine = StartCoroutine (DoAttack (Hero.AudioClips.Strike_3.ToString (), .4f));
        }
        currentAttackEnum = StatsEnum.Strike_3;
        _anima.SetInteger ("Attack Index", (int) currentAttackEnum);
        _anima.SetBool ("Attack", true);
    }

    public void Jump () {
        _jump = true;
        Hero.instance.audioManager.Stop (Hero.AudioClips.Run.ToString ());
        _fsm.FinishOtherStates ();
    }

    public void Uppercut () {
        _fsm.RunState ((int) StatsEnum.Uppercut);
        if (_fsm.GetCurrentState () == Moves)
            _fsm.FinishState ();
    }

    void DoUppercut () {
        _attacks = true;
        if (currentAttackEnum != StatsEnum.Uppercut) {
            _anima.SetFloat ("Speed", 0);
            Hero.instance.audioManager.Play (Hero.AudioClips.Uppercut.ToString ());
        }
        currentAttackEnum = StatsEnum.Uppercut;
        _anima.SetInteger ("Attack Index", (int) currentAttackEnum);
        _anima.SetBool ("Attack", true);
    }

    public void Dodge () {
        _dodge = true;
        _fsm.FinishAllStates ();
    }

    private IEnumerator DoDodge (float time, Vector2 startPos) {
        _isDodging = true;
        _anima.SetFloat ("Speed", 0);
        _rigidbody.velocity = Vector2.zero;
        Hero.instance.audioManager.Play (Hero.AudioClips.Dodge.ToString ());
        yield return new WaitForSeconds (.04f);
        Vector2 EndPos = startPos;
        for (float t = 0; t <= time; t += Time.fixedDeltaTime) 
            if (Math.Abs (startPos.x - transform.position.x) > _dodgeLength) {
                EndPos.x = startPos.x + (-_dodgeLength * transform.localScale.x);
                EndPos.y = transform.position.y;
                transform.position = EndPos;
                _rigidbody.velocity = new Vector2 (_rigidbody.velocity.x / 2, 0);
                _isDodging = false;
                yield break;
            } 
            else
            {
                _rigidbody.AddForce (new Vector2 (-_dodgeForceX * transform.localScale.x, _dodgeHeight), ForceMode2D.Impulse);
                yield return new WaitForFixedUpdate ();
            }
        _isDodging = false;

    }

    private IEnumerator DoAttack (string AudioClipName, float time) {
        _attacks = true;
        _anima.SetFloat ("Speed", 0);
        Hero.instance.audioManager.Play (AudioClipName);
        _lastAttackTime = Time.fixedTime;
        for (float t = 0; t <= time; t += Time.fixedDeltaTime) {
            _rigidbody.velocity = new Vector2 (_attacksLength * transform.localScale.x, 0f);
            yield return null;
        }

    }

    private IEnumerator DoShield_Attack (float time, Vector2 startPos) {
        _attacks = true;
        _anima.SetFloat ("Speed", 0);
        Hero.instance.audioManager.Play (Hero.AudioClips.Shield_Attack.ToString ());
        _lastAttackTime = Time.fixedTime;
        Vector2 EndPos = startPos;
        float tempH = _shield_AttackHeight;
        for (float t = 0; t <= time; t += Time.fixedDeltaTime) {
            if (Math.Abs (startPos.x - transform.position.x) > _shield_AttackLength) {
                EndPos.x = startPos.x + (_shield_AttackLength * transform.localScale.x);
                EndPos.y = transform.position.y;
                transform.position = EndPos;
                _rigidbody.velocity = new Vector2 (_rigidbody.velocity.x / 2, 0);
                yield break;
            } else {
                _rigidbody.AddForce (new Vector2 (_shield_AttackForceX * transform.localScale.x, tempH), ForceMode2D.Impulse);
                tempH = 0f;
                yield return new WaitForFixedUpdate ();
            }
        }
    }

}