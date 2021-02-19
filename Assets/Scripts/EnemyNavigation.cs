using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private EnemyType _enemyType;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        Enemy enemy = GetComponent<Enemy>();

        if (!enemy)
        {
            Debug.Log("EnemyNavigation is not attached to Enemy");
        } else
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
