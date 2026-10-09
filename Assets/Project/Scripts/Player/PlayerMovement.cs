using System;
using UnityEngine;
using VContainer;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    
    
    private PlayerInputReader inputReader;

    [Inject]
    public void Construct(PlayerInputReader playerInputReader)
    {
        inputReader = playerInputReader;
    }

    public void Update()
    {
        rigidbody.AddForce(inputReader.MovementInput * 10, ForceMode.Force);
    }
}
