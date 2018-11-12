using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenBrain : MonoBehaviour {

    #region Singleton

    public static FallenBrain instance;

    void Awake () {
        instance = this;
    }

    #endregion

    [HideInInspector]
    public int _rightFallenID = -1;
    [HideInInspector]
    public int _leftFallenID = -1;

    // private void Start()
    // {
    // 		_all = new List<GameObject>();
    // 		_visibles = new List<GameObject>();				
    // }


    public void ClearPosition(int fallenID)
    {
        if (_leftFallenID == fallenID)
          _leftFallenID = -1;
          if (_rightFallenID == fallenID)
          _rightFallenID = -1;

    }

    public bool CheckPosition (int fallenID) {
        if (_rightFallenID == -1) {
            _rightFallenID = fallenID;
            return true;
        } else if (_rightFallenID == fallenID)
            return true;

        if (_leftFallenID == -1) {
            _leftFallenID = fallenID;
            return true;
        } else if (_leftFallenID == fallenID)
            return true;

        return false;
    }
}