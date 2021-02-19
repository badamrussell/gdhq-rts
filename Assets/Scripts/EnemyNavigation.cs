using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
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
            SpawnManager.Instance.EnemyReachedGoal(this.gameObject);
        }
    }
}
