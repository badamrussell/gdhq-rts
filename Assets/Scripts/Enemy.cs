using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitType
{
    Heavy,
    Infantry,
    None
}

public class Enemy : MonoBehaviour
{
    private Vector3 _destination;

    [SerializeField]
    private int _health;

    [SerializeField]
    private int _warFund;
    
    private NavMeshAgent _navMeshAgent;

    public UnitType unitType;
    
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _navMeshAgent.SetDestination(_destination);
    }

    public void SetDestination(Vector3 targetPos)
    {
        _destination = targetPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "goal")
        {
            SpawnManager.Instance.EnemyReachedTarget(this.gameObject);
        }
    }
}
