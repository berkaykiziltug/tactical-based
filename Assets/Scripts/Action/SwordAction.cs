using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;
    public static event EventHandler OnAnySwordHit;

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }

    private State state;
    private float stateTimer;
    private int maxSwordDistance = 1;
    private Unit targetUnit;
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;
        
        switch (state)
        {
            case State. SwingingSwordBeforeHit:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float turnSpeed = 10f;
                transform.forward = Vector3.Slerp(transform.forward, aimDir, Time.deltaTime * turnSpeed);
                break;
            case State.SwingingSwordAfterHit:
             
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }
    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = .5f;
                stateTimer = afterHitStateTime;
                targetUnit.Damage(100);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted(this, EventArgs.Empty);
               ActionComplete();
                break;
        }

    }

    public override string GetActionName()
    {
        return "Sword";
    }
    
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = .7f;
        stateTimer = beforeHitStateTime;
        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        
        List<GridPosition> validGridPositionList = new List<GridPosition>();
       
        GridPosition unitGridPosition = unit.GetGridPosition();

        // GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                //Cycle through all the grid positions in maxMoveDistance range. Starting from - to + (left to right and bottom to top) and make grid positions
                //I can get whatever data I want on the cycled positions. Then add it to current Unit's position and store it inside test grid position.
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //Grid position is empty. No unit.
                    continue;
                }
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //Both Units on the same team.
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }

    public int GetMaxSwordDistance()
    {
        return maxSwordDistance;
    }
}
