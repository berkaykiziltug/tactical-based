using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    
    //Late update runs after every update. Just a tiny bit of delay. Better for this case. Used usually in camera movements.
    private void LateUpdate()
    {
        if (invert)
        {
            //This is just to make the objects which are numbers in this case, to face the same way as camera. Not face the camera. This gives us the normal look of numbers instead of inverted numbers.
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCamera * -1);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}
