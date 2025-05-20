using System;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        CreateUnitActionButtons();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }

    private void CreateUnitActionButtons()
    {
        //First destroy all the buttons inside to prevent duplicates.
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        //Then Instantiate the buttons every time the unit changes to represent its actions.
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
          Transform actionButtonTransform =  Instantiate(actionButtonPrefab, actionButtonContainerTransform);
          ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
          actionButtonUI.SetBaseAction(baseAction);
        }
    }
}
