using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai : MonoBehaviour
{
    public Transform _target; // Указываем переменную, к которой будет двигаться наш агент  
    NavMeshAgent _agent; // Указываем переменную агента  
    private Animator _animator;

    void Start()
    {
        _agent = (NavMeshAgent)this.GetComponent("NavMeshAgent"); // Указываем, что переменная _agent - это наш агент. 
        _animator = GetComponent<Animator>();
        _animator.SetBool("Grounded", true);
        _animator.SetFloat("MotionSpeed", 1);
    }

    void Update()
    {
        _agent.SetDestination(_target.position); // Заставляем агента двигаться в сторону _target'а
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
