using UnityEngine;

public class BulletImpactSound : MonoBehaviour
{
    //TODO: 재질 별로 소리 다르게
    [SerializeField]private AudioClip[] bulletImpactSFX;
    [SerializeField]private GameObject pooledAudioPrefab;

    private void OnEnable()
    {
        PlayRandomImpactSound();
    }

    private void PlayRandomImpactSound()
    {
        if (bulletImpactSFX == null || bulletImpactSFX.Length == 0) return;

        int randomIndex = Random.Range(0, bulletImpactSFX.Length);
        SoundManager.Instance.PlaySFXForClip(bulletImpactSFX[randomIndex], gameObject.transform.position);

    }
}
