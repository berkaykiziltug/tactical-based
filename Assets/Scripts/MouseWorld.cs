using System;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask mouseLayerMask;
    
    private static MouseWorld instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    
    public static Vector3 GetMouseWorldPosition()
    {
        //Fire a ray from camera's position towards the mouse position. Simple. Then I store the ray.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Setting the layermask so It "only" collides with the ground.
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mouseLayerMask);
        return raycastHit.point;
    }
}
