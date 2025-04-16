using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellFallSound : MonoBehaviour
{
    public AudioClip shellSound;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Gun")
        {
            SoundManager.Instance.PlaySFX(shellSound);
            Debug.Log("탄피");
        }
    }
}
