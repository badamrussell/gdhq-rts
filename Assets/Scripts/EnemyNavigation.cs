using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
 
    private void Start()
    {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();

        if (!navMeshAgent)
        {
            Debug.LogError("NavMesh Agent is NULL");
        }

        Vector3 goal = SpawnManager.Instance.GoalPosition;

        navMeshAgent.SetDestination(goal);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "goal")
        {
            SpawnManager.Instance.EnemyReachedGoal(this.gameObject);
        }
    }
}
