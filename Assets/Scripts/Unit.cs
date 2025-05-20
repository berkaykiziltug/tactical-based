using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private MoveAction moveAction;
    private SpinAction spinAction;
    private GridPosition gridPosition;
    private BaseAction[] baseActionArray;

    void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        //basically gets all the components that are base action.
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        //As soon as the game starts the Unit calculates which grid it stands on. That's why the transform.position
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
      

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            
            LevelGrid.Instance.UnitMovedGridPosition(this,gridPosition, newGridPosition);
            gridPosition = newGridPosition;
            
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }
    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }
}
