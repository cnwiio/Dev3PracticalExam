using System;
using UnityEngine;
using VContainer;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rg;
    
    
    private PlayerInputReader inputReader;

    [Inject]
    public void Construct(PlayerInputReader playerInputReader)
    {
        inputReader = playerInputReader;
    }

    public void Update()
    {
        HandleHorizontalMovement();
    }

    private void HandleHorizontalMovement()
    {
        Vector3 movementDirection = new Vector3(inputReader.MovementInput.x, 0, inputReader.MovementInput.y).normalized * 10;
        rg.AddForce(movementDirection, ForceMode.Force);    
    }
}
