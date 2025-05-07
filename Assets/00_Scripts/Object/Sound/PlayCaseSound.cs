using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCaseSound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Gun"))
        {
            SoundManager.Instance.PlaySFXForName("Shell");
        }
    }
}
