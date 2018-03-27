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
            System.Random pseudoRandom = new System.Random(DateTime.Now.Minute * 60000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond);
//            System.Random pseudoRandom = new System.Random(_seed.GetHashCode());
            int point = pseudoRandom.Next(0, 100);
            Debug.Log(point);
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

    void IncertPrefab(GameObject inObject)
    {
        GameObject puzzleOut = _currentPrefab.transform.GetChild(0).Find("PuzzleOut").gameObject;
        if (puzzleOut == null)
        {
            Debug.Log("не найден PuzzleOut");
            return;
        }
        _currentPrefab = Instantiate(inObject, puzzleOut.transform.position, inObject.transform.rotation);
    }
}
