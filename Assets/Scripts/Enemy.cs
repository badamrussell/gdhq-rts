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
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        Enemy enemy = GetComponent<Enemy>();

        if (!enemy)
        {
            Debug.Log("EnemyNavigation is not attached to Enemy");
        }
        else
        {
            _enemyType = enemy.EnemyType;
        }
        if (!_navMeshAgent)
        {
            Debug.LogError("NavMesh Agent is NULL");
        }
    }

    private void OnEnable()
    {
        Vector3 goal = SpawnManager.Instance.GoalPosition;
        _navMeshAgent.SetDestination(goal);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "goal")
        {
            SpawnManager.Instance.EnemyReachedGoal(_enemyType, this.gameObject);
        }
    }
}
