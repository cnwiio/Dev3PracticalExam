using System;
using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;

    private float counterF;
    private int counter;

    private void Start()
    {
        counterF = 2;
    }

    private void Update()
    {
        counterF -= Time.deltaTime;
        counter = (int)MathF.Ceiling(counterF);
        textUI.SetText($"-- {counter.ToString()} --");
        Debug.Log("Float :" + counterF);
    }
}
