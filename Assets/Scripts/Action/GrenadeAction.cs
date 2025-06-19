using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private Transform grenadeProjectilePrefab;
    private int maxThrowDistance = 7;
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
       
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab,unit.GetWorldPosition(),Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);
        Debug.Log("GrenadeAction");
        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
       List<GridPosition> validGridPositionList = new List<GridPosition>();
       
       GridPosition unitGridPosition = unit.GetGridPosition();

        // GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                //Cycle through all the grid positions in maxMoveDistance range. Starting from - to + (left to right and bottom to top) and make grid positions
                //I can get whatever data I want on the cycled positions. Then add it to current Unit's position and store it inside test grid position.
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance)
                {
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
            actionValue = 0,
        };
    }
}
