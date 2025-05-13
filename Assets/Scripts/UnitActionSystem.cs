using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    
   [SerializeField] private Unit selectedUnit;
   [SerializeField] private LayerMask unitLayerMask;

    void Update()
    {
         if (Input.GetMouseButtonDown(0))
        { 
            //check if you can select the unit. If yes then just return until next mouse click so we don't have the character move instantly after selecting the unit.
            if(TryHandleUnitSelection()) return;
           selectedUnit.Move(MouseWorld.GetMouseWorldPosition());
        }
    }


    private bool TryHandleUnitSelection(){ 
        //Fire a ray from camera's position towards the mouse position. Simple. Then I store the ray.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask)){ 
            if(raycastHit.transform.TryGetComponent<Unit>(out Unit unitComponent))
            {
                selectedUnit = unitComponent;
                return true;
            }
        }
        return false;
    }
}
