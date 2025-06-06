using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;


public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [FormerlySerializedAs("offset")] [SerializeField] private CinemachineFollow cinemachineFollow;
    private Vector3 targetFollowOffset;

    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    private void Start()
    {
        targetFollowOffset = cinemachineFollow.FollowOffset;
    }

    private void Update()
    { 
       HandleMovement();
       HandleRotation();
       HandleZoom();
    }

    private void HandleMovement()
    {
        
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        float moveSpeed = 10f;
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;

    }

    private void HandleRotation()
    {
        Vector3 rotationVector = Vector3.zero;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }
        
        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;

    }

    private void HandleZoom()
    {
          
        float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y> 0)
        {
            targetFollowOffset.y -=zoomAmount;
        }
        if (Input.mouseScrollDelta.y< 0)
        {
            targetFollowOffset.y +=zoomAmount;
        }

        float zoomSpeed = 5f;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        cinemachineFollow.FollowOffset = Vector3.Lerp(cinemachineFollow.FollowOffset, targetFollowOffset, Time.deltaTime *zoomSpeed);
    }
}
