using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemAddTime : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
