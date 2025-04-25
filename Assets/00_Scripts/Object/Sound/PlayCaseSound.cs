using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCaseSound : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Gun"))
        {
            SoundManager.Instance.PlaySFX("Shell");
        }
    }
}
