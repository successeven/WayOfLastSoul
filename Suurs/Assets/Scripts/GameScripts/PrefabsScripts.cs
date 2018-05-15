using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System;

public class PrefabsScripts : MonoBehaviour
{
    [SerializeField]
    SpriteAtlas _Lands;    
    public SpriteAtlas Lands
    {
        get
        {
            return _Lands;
        }
    }


    [SerializeField]
    SpriteAtlas _ObjectsAtlas;
    public SpriteAtlas Objects
    {
        get
        {
            return _ObjectsAtlas;
        }
    }

    [SerializeField]
    GameObject _DrawObject;
    public GameObject DrawObject
    {
        get
        {
            return _DrawObject;
        }
    }

    [Space(10)]

    [SerializeField]
    List<GameObject> _Grounds;
        
    [SerializeField]
    List<GameObject> _Enemy;
    public List<GameObject> Enemys
    {
        get
        {
            return _Enemy;
        }
    }


    Dictionary<string, GameObject> _GroundsbyName;
    
    private void Awake()
    {
        _GroundsbyName = new Dictionary<string, GameObject>();
        foreach(var item in _Grounds)
            _GroundsbyName.Add(item.name, item);
    }

    public GameObject GetGroundbyName(GroundName a_Name)
    {
        string name = a_Name.ToString();
        if (!_GroundsbyName.ContainsKey(name))
        {
            Debug.LogError(string.Format("ERROR! Ground \"{0}\" not found!", name));
            return null;
        }
        else
            return _GroundsbyName[name];
    }

}
