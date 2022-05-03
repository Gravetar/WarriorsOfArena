using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai : MonoBehaviour
{
    public Transform _target; // ��������� ����������, � ������� ����� ��������� ��� �����  
    NavMeshAgent _agent; // ��������� ���������� ������  

    void Start()
    {
        _agent = (NavMeshAgent)this.GetComponent("NavMeshAgent"); // ���������, ��� ���������� _agent - ��� ��� �����.  
    }

    void Update()
    {
        _agent.SetDestination(_target.position); // ���������� ������ ��������� � ������� _target'�

        if (Vector3.Distance(_agent.transform.position, _target.transform.position) < 4)
        {
            _agent.speed = 0;
        }
        else
        {
            _agent.speed = 3.5f;
        }
    }
}
