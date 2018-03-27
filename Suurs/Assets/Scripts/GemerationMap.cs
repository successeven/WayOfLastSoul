using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GemerationMap : MonoBehaviour
{

    public int _lenghtmap;
    public GameObject _currentPrefab;
    public GameObject[] _prefabs;
    public int[] _percentSpawn;

    // Use this for initialization
    void Start()
    {
        Generation();
    }

    private void Generation()
    {
        for (int i = 0; i < _lenghtmap; i++)
        {
//            System.Random pseudoRandom = new System.Random(_seed.GetHashCode());
            int point = GetPercentRandom();
            int minPercent = 1;
            int maxPercent = 0;
            for (int j = 0; j < _percentSpawn.Length; j++)
            {
                maxPercent += _percentSpawn[j];
                if (point < maxPercent && point >= minPercent)
                {
                    IncertPrefab(_prefabs[j]);
                    break;
                }
                minPercent = maxPercent + 1;
            }
        }
    }

    int GetPercentRandom()
    {
        System.Random _pseudoRandom = new System.Random(DateTime.Now.Minute * 60000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond);
        return _pseudoRandom.Next(0, 100);
    } 

    void IncertPrefab(GameObject inObject)
    {
        GameObject puzzleOut = _currentPrefab.transform.Find("ConnectionPuzzle").gameObject;
        //Будем ли разворачивать обьет (для наклонной земли)

        bool isRotation = GetPercentRandom() >= 50;
        Quaternion rotation = new Quaternion(puzzleOut.transform.rotation.x, 0,
            puzzleOut.transform.rotation.z, puzzleOut.transform.rotation.w);
        Debug.Log(rotation);

        Vector3 position = new Vector3(puzzleOut.transform.position.x, puzzleOut.transform.position.y, puzzleOut.transform.position.z);

        _currentPrefab = Instantiate(inObject, position, rotation);
         if (isRotation)
        {
            rotation.y = 180f;
            GameObject puzzleIn = _currentPrefab.transform.Find("ConnectionPuzzle").gameObject;
            position.x = puzzleIn.transform.position.x;
            position.y += position.y - puzzleIn.transform.position.y;
            _currentPrefab.transform.position = position;
            _currentPrefab.transform.rotation = rotation;
            puzzleIn.transform.localPosition = new Vector3(0, 0, 0);
        }

    }
}
