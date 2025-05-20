using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
   public static UnitActionSystem Instance { get; private set; } 
   public event EventHandler OnSelectedUnitChanged;
   [SerializeField] private Unit selectedUnit;
   [SerializeField] private LayerMask unitLayerMask;

   private bool isBusy;

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
       if (isBusy) return;
         if (Input.GetMouseButtonDown(0))
         { 
             //check if you can select the unit. If yes then just return until next mouse click so we don't have the character move instantly after selecting the unit.
             if(TryHandleUnitSelection()) return;
             
             GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMouseWorldPosition());
             if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
             {
                 SetBusy();
                 selectedUnit.GetMoveAction().Move(mouseGridPosition, ClearBusy);
             }
             
         }

         if (Input.GetMouseButtonDown(1))
         {
             SetBusy();
             selectedUnit.GetSpinAction().Spin(ClearBusy);
         }
   }

   private void SetBusy()
   {
       isBusy = true;
   }

   private void ClearBusy()
   {
       isBusy = false;
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
