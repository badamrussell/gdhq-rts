using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Heavy,
    Infantry,
    None
}

public class Enemy : MonoBehaviour
{

    [SerializeField] private EnemyType _enemyType;
    public EnemyType EnemyType
    {
        get
        {
            return _enemyType;
        }
    }

    [SerializeField] private int _health;
    [SerializeField] private int _warFund;
    
}
