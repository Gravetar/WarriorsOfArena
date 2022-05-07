using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai : MonoBehaviour
{
    public Transform _target; // ��������� ����������, � ������� ����� ��������� ��� �����  
    NavMeshAgent _agent; // ��������� ���������� ������  
    private Animator _animator;

    void Start()
    {
        _agent = (NavMeshAgent)this.GetComponent("NavMeshAgent"); // ���������, ��� ���������� _agent - ��� ��� �����. 
        _animator = GetComponent<Animator>();
        _animator.SetBool("Grounded", true);
        _animator.SetFloat("MotionSpeed", 1);
    }

    void Update()
    {
        _agent.SetDestination(_target.position); // ���������� ������ ��������� � ������� _target'�
        _animator.SetFloat("Speed", _agent.speed);

        if (Vector3.Distance(_agent.transform.position, _target.transform.position) < 3)
        {
            _agent.speed = 0;
        }
        else
        {
            _agent.speed = 3.5f;
        }
    }
}
