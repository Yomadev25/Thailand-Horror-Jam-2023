using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    float delay;
    public EnemyIdleState(EnemyStateMachine ctx) : base(ctx) { }

    public override void Enter()
    {
        if (_context.overrideAnim != null)
        {
            _context.Anim.runtimeAnimatorController = _context.overrideAnim;
        }        

        if (_context.idleBehavior == EnemyStateMachine.IdleBehavior.PATROL)
        {
            _context.NavMesh.speed = 2f;
            _context.NavMesh.isStopped = false;
        }
    }

    public override void Update()
    {
        if (_context.idleBehavior == EnemyStateMachine.IdleBehavior.IDLE)
        {
            if (Vector3.Distance(_context.transform.position, _context.patrolSequence[_context.currentPatrol].position) > 1)
            {
                _context.NavMesh.speed = 2f;
                _context.NavMesh.isStopped = false;
                _context.NavMesh.SetDestination(_context.patrolSequence[0].position);
            }
        }
        if (_context.idleBehavior == EnemyStateMachine.IdleBehavior.PATROL)
        {
            if (Vector3.Distance(_context.transform.position, _context.patrolSequence[_context.currentPatrol].position) < 1)
            {
                _context.ChangePatrolPoint();
                delay = _context.patrolDelay;
            }

            if (delay <= 0)
            {
                _context.NavMesh.SetDestination(_context.patrolSequence[_context.currentPatrol].position);
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }

        _context.Anim.SetFloat("Speed", _context.NavMesh.velocity.magnitude);
        CheckChangeState();
    }

    public override void FixedUpdate()
    {
        
    }

    private void CheckChangeState()
    {
        if (_context.GetVisibleTarget() != null)
        {
            ChangeState(_context.State.Chase());
        }
    }

    public override void Exit()
    {

    }
}
