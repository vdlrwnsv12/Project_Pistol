using UnityEngine;

public class RandomSit : MonoBehaviour
{
    Animator animator;

    // 애니메이션 상태 이름 목록
    private string[] sitAnimations = { "Sit", "Sit2", "Sit3", "Sit4" };

    void Start()
    {
        animator = GetComponent<Animator>();

        // 랜덤으로 애니메이션 선택
        string selectedSit = sitAnimations[Random.Range(0, sitAnimations.Length)];
        animator.Play(selectedSit);
    }
}
