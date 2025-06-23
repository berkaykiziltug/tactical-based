using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    private int maxInteractDistance = 1;

    private void Update()
    {
        if (!isActive) return;
        
    }

    public override string GetActionName()
    {
        return  "Interact";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
        interactable.Interact(OnInteractComplete);
        ActionStart(onActionComplete);
    }

    private void OnInteractComplete()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
       
        GridPosition unitGridPosition = unit.GetGridPosition();

        // GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {
            for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
            {
                //Cycle through all the grid positions in maxMoveDistance range. Starting from - to + (left to right and bottom to top) and make grid positions
                //I can get whatever data I want on the cycled positions. Then add it to current Unit's position and store it inside test grid position.
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);
                if (interactable == null)
                {
                    //No interactable on this gridPosition
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
            actionValue = 0
        };
    }
}
