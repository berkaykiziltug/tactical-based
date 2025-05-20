using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private Vector3 targetPosition;
    [SerializeField]private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;
   

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive) return;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float stopDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            unitAnimator.SetBool("isWalking", true);
           
            // movement is not dependent on framerate since we multiply with Time.deltaTime
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

        }
        else{
            unitAnimator.SetBool("isWalking",false);
            isActive = false;
            onActionComplete();
        }
        
        float turnSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * turnSpeed);
    }

    public void Move(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                //Cycle through all the grid positions in maxMoveDistance range. Starting from - to + (left to right and bottom to top) and make grid positions
                //I can get whatever data I want on the cycled positions. Then add it to current Unit's position and store it inside test grid position.
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    //Same grid position where unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //Grid position already occupied with another unit.
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
                
            }
        }

        return validGridPositionList;
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }
}
