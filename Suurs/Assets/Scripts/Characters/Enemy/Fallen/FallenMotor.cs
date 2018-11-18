using System;
using UnityEngine;

public enum FallenSounds {
    None,
    Move,
    Strike,
    Death,
    When_Hit
}

[RequireComponent (typeof (FallenManager))]
public class FallenMotor : MonoBehaviour {

    public float _speed;

    enum FallenState 
    {
        Idle,
        Moves,
        Attack,
        Teleport
    }

    Animator _anima;
    Rigidbody2D _rigidbody;
    AudioManager _audioManager;

    [NonSerialized]
    public bool _reciveDamage = false;

    [NonSerialized]
    public bool _attacks = false;
    protected float _moveLeftSide = -1; //1- вправо ; -1 влево

    bool _teleporting = false;
    MyFSM _fsm;
    FallenManager _manager;

    float _currentHorAxis;
    public float CurrentHorAxis {
        get {
            return _currentHorAxis;
        }
        set {
            _currentHorAxis = value;
        }
    }

    // Use this for initialization
    void Start () 
    {
        _anima = GetComponent<Animator> ();
        _rigidbody = GetComponent<Rigidbody2D> ();
        _audioManager = GetComponent<AudioManager> ();
        _manager = GetComponent<FallenManager> ();

        _fsm = new MyFSM (Moves);
        _fsm.AddStates ((int) FallenState.Moves, Moves);
        _fsm.AddStates ((int) FallenState.Attack, DoAttack);
        _fsm.AddStates ((int) FallenState.Teleport, DoTeleport);
    }

    void Moves () 
    {
        if (_manager._death)
            return;

        _attacks = false;
        _anima.SetBool ("Attack", false);
        _anima.SetBool ("Move", _currentHorAxis != 0);

        if (!_audioManager.IsPlaying (FallenSounds.Move.ToString ()) && _currentHorAxis != 0)
            _audioManager.Play (FallenSounds.Move.ToString ());
        else if (_currentHorAxis == 0)
            _audioManager.Stop (FallenSounds.Move.ToString ());

        _rigidbody.velocity = new Vector2 (_currentHorAxis * _speed, _rigidbody.velocity.y);

        bool actionRight = false;
        if (transform.localScale.x < 0)
            actionRight = true;

        if (Hero.instance.transform.position.x - transform.position.x > 0 && !actionRight)
            Flip ();
        else if (Hero.instance.transform.position.x - transform.position.x < 0 && actionRight)
            Flip ();
    }

    public void Attack () 
    {
        _fsm.RunState ((int) FallenState.Attack);
        if (_fsm.GetCurrentState () == Moves)
            _fsm.FinishState ();
    }

    void DoAttack () 
    {
        if (!_audioManager.IsPlaying (FallenSounds.Strike.ToString ()))
            _audioManager.Play (FallenSounds.Strike.ToString ());

        _anima.SetBool ("Attack", true);
        _attacks = true;
    }

    void DoTeleport () 
    {
        _anima.SetBool ("Move", false);
    }

    public void Teleport () 
    {
        _fsm.RunState ((int) FallenState.Teleport);
        if (_fsm.GetCurrentState () == Moves)
            _fsm.FinishState ();
        _anima.SetTrigger ("Teleport");
    }

    Vector3 newPos;
    public void Teleporting () 
    {
        newPos = Hero.instance.transform.position;
        newPos.x -= 6;
        transform.position = newPos;

        bool actionRight = false;
        if (transform.localScale.x < 0)
            actionRight = true;

        if (Hero.instance.transform.position.x - transform.position.x > 0 && !actionRight)
            Flip ();
        else if (Hero.instance.transform.position.x - transform.position.x < 0 && actionRight)
            Flip ();

    }

    public void ResetState () 
    {
        _fsm.FinishState ();
    }

    private void FixedUpdate () 
    {
        if (_manager._HP > 0) 
        {
            //Debug.Log(gameObject.GetInstanceID().ToString() + " " + (FallenState)_fsm.GetCurrentStateID());
            _fsm.Invoke ();
        }
    }

    protected void Flip () {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}