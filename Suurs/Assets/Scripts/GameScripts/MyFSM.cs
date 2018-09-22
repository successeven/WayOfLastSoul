using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFSM
{

    public delegate void State();       // Функция состояния

    Queue<State> _queueStates;
    private readonly Dictionary<int, State> _AllStates;      // имя состояния, функция 
    State _defaultState;

    State _currentState;

    public MyFSM(State DefaultState)
    {
        _queueStates = new Queue<State>();
        _AllStates = new Dictionary<int, State>();
        _defaultState = DefaultState;
        _currentState = _defaultState;
    }

    public State GetCurrentState()
    {
        return  _currentState;
    }

		public bool isNextState(int IndexState)
		{				
				var item = _queueStates.Peek();
				State tempState;
				if (_AllStates.TryGetValue(IndexState, out tempState) && item == tempState)
						return true;
				else
						return false;
		}

		public void AddStates(int IndexState, State newState)
    {
        if (!_AllStates.ContainsKey(IndexState))
            _AllStates.Add(IndexState, newState);
    }

    public void RunState(int IndexState)
    {
        State tempState;

        if (_AllStates.TryGetValue(IndexState, out tempState) && _currentState != tempState)
            _queueStates.Enqueue(tempState);


        return;
    }

    public void Invoke()
    {
        GetCurrentState().Invoke();
    }

    public void FinishState()
    {
        if (_queueStates.Count > 0)
            _currentState = _queueStates.Dequeue();
        else
            _currentState = _defaultState;

    }

    public int CountStates()
    {
        return _AllStates.Count;
    }

    public int Count()
    {
        return _queueStates.Count;
    }

    public void FinishAllStates()
    {
        _queueStates.Clear();
        FinishState();
    }

}
