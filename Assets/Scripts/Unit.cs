using System;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField]private Animator unitAnimator;
    private Vector3 targetPosition;
    private GridPosition gridPosition;


    void Awake()
    {
        targetPosition = transform.position;
    }

    private void Start()
    {
        //As soon as the game starts the Unit calculates which grid it stands on. That's why the transform.position
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        
        float stopDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            unitAnimator.SetBool("isWalking", true);
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

           
            // movement is not dependent on framerate since we multiply with Time.deltaTime
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            float turnSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * turnSpeed);
        }
        else{
            unitAnimator.SetBool("isWalking",false);
        }

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            //Unit changed grid position logic here.
            LevelGrid.Instance.UnitMovedGridPosition(this,gridPosition, newGridPosition);
            gridPosition = newGridPosition;
            
        }
    }
    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
