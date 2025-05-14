using System;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform debugPrefab;
    private GridSystem gridSystem;
    private void Start()
    {
        
        gridSystem = new GridSystem(10, 10,2);
        gridSystem.CreateDebugObjects(debugPrefab);
        Debug.Log(new GridPosition(5,7));
    }

    private void Update()
    {
        Debug.Log(gridSystem.GetGridPosition(MouseWorld.GetMouseWorldPosition()));
    }
}
