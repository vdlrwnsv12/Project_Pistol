using System.Collections.Generic;
using UnityEngine;

public class RandomSit : MonoBehaviour
{
    Animator animator;

    [SerializeField] private bool sit1 = true;
    [SerializeField] private bool sit2 = true;
    [SerializeField] private bool sit3 = true;


    void Start()
    {
        animator = GetComponent<Animator>();

        List<string> anims = new List<string>();
        if (sit1) anims.Add("Sit");
        if (sit2) anims.Add("Sit2");
        if (sit3) anims.Add("Sit3");

        string selectedSit = anims[Random.Range(0, anims.Count)];
        animator.Play(selectedSit);
    }
}
