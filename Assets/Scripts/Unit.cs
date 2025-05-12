using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    
    private void Update()
    {
        float stopDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            // movement is not dependent on framerate since we multiply with Time.deltaTime
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Move(new Vector3(4, 0, 4));
        }
    }
    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
