using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
   [SerializeField] private Unit selectedUnit;
   [SerializeField] private LayerMask unitLayerMask;

   private void Awake()
   {
       if (Instance != null && Instance != this)
       {
           Destroy(this.gameObject);
       }
       else
       {
           Instance = this;
       }
   }

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
                //Invoking the event inside this method.
                SetSelectedUnit(unitComponent);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
