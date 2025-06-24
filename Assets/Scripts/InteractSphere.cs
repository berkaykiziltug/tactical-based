using System;
using Unity.Mathematics;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField]private MeshRenderer meshRenderer;
    [SerializeField] private Transform enemyUnitPrefab;
    [SerializeField] private Transform[] enemySpawnPositionArray;

    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private float timer;
    private bool isActive;
    private bool isGreen;
    private bool canInteract;
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        SetColorGreen();
        canInteract = true;
    }

    private void Update()
    {
        if (!isActive) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    private void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;
    }

    private void SetColorRed()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = .5f;
        if (isGreen)
        {
            SetColorRed();
        }
        else
        {
            SetColorGreen();
        }

        if (canInteract)
        {
            foreach (Transform enemyUnitSpawnPosition in enemySpawnPositionArray)
            {
                Instantiate(enemyUnitPrefab, enemyUnitSpawnPosition);
            }   
        }
        else
        {
            return;
        }

    }
}
