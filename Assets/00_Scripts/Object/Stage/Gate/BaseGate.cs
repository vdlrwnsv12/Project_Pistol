using System;
using UnityEngine;

public abstract class BaseGate : MonoBehaviour
{
    public event Action OnPassingGate;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPassingGate?.Invoke();
        }
    }
}
