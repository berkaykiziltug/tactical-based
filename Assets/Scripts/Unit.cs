using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField]private Animator unitAnimator;
    private Vector3 targetPosition;
    
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
        }else{
            unitAnimator.SetBool("isWalking",false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetMouseWorldPosition());
        }
    }
    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
