using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    Transform target;

    public EnemyChaseState(EnemyStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        _context.Anim.runtimeAnimatorController = _context.defaultAnim;

        _context._alert.SetActive(true);
        _context.NavMesh.speed = 5f;
        _context.NavMesh.isStopped = false;
    }

    public override void Update()
    {
        target = _context.GetChasedTarget();

        if (target != null)
        {
            _context.NavMesh.SetDestination(target.position);
        }
        _context.Anim.SetFloat("Speed", _context.NavMesh.velocity.magnitude);

        CheckChangeState();
    }

    public override void FixedUpdate()
    {

    }

    private void CheckChangeState()
    {
        if (target == null)
        {
            ChangeState(_context.State.Idle());
        }
    }

    public override void Exit()
    {
        _context._alert.SetActive(false);
        _context.NavMesh.velocity = Vector3.zero;
        _context.NavMesh.Stop();
    }
}
