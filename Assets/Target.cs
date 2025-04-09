using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp;
    public Animator anim;

    void Start()
    {
      currentHp = maxHp;  
      anim.GetComponentInParent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        
        if(currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Die");
        anim.SetBool("Die", true);
    }
}
