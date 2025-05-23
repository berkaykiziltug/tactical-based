using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
   public static UnitActionSystem Instance { get; private set; } 
   public event EventHandler OnSelectedUnitChanged;
   public event EventHandler OnSelectedActionChanged;
   public event EventHandler<bool> OnBusyChanged;
   public event EventHandler OnActionStarted;
   
   [SerializeField] private Unit selectedUnit;
   [SerializeField] private LayerMask unitLayerMask;

   private BaseAction selectedAction;
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

   private void Start()
   {
       SetSelectedUnit(selectedUnit);
   }

   void Update()
   {
       if (isBusy)
       {
           return;
       }

       if (EventSystem.current.IsPointerOverGameObject())
       {
           return;
       }
       //check if you can select the unit. If yes then just return until next mouse click so we don't have the character move instantly after selecting the unit.
       if(TryHandleUnitSelection())
       {
           return;
       }
       HandleSelectedAction();
         
   }

   private void SetBusy()
   {
       isBusy = true;
       OnBusyChanged?.Invoke(this, isBusy);
   }

   private void ClearBusy()
   {
       isBusy = false;
       OnBusyChanged?.Invoke(this, isBusy);
   }

   private bool TryHandleUnitSelection(){
       if (Input.GetMouseButtonDown(0))
       {
           //Fire a ray from camera's position towards the mouse position. Simple. Then I store the ray.
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
           {
               if (raycastHit.transform.TryGetComponent<Unit>(out Unit unitComponent))
               {
                   if (unitComponent == selectedUnit)
                   {
                       return false;
                   }
                   //Invoking the event inside this method.
                   SetSelectedUnit(unitComponent);
                   return true;
               }
           }
       }

       return false;
   }

   private void SetSelectedUnit(Unit unit)
   {
       selectedUnit = unit;
       SetSelectedAction(unit.GetMoveAction()); 
       OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
       
   }

   public void SetSelectedAction(BaseAction baseAction)
   {
       selectedAction = baseAction;
       OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
   }

   public Unit GetSelectedUnit()
   {
       return selectedUnit;
       
   }

   private void HandleSelectedAction()
   {
       if (Input.GetMouseButtonDown(0))
       {
           GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMouseWorldPosition());

           if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
           {
               return;
           }

           if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
           {
               return;
           }
           SetBusy();
           selectedAction.TakeAction(mouseGridPosition, ClearBusy);
           OnActionStarted?.Invoke(this, EventArgs.Empty);
       }
   }

   public BaseAction GetSelectedAction()
   {
       return selectedAction;
   }
}
