using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        #region Parameters

        [Header("회전 설정")]
        [SerializeField] private float smooth = 1.0f;
        [SerializeField] private float openAngle = -90.0f;
        [SerializeField] private float closeAngle = 0.0f;

        [Header("사운드")]
        [SerializeField] private AudioClip openDoorClip;
        [SerializeField] private AudioClip closeDoorClip;

        [Header("스테이지 로더 연동")]
        [SerializeField] private StageLoader stageLoader; // 드래그해서 할당
        [SerializeField] private bool triggerNextStageOnOpen = false;

        private bool open = false;
        private bool stageLoaded = false;

        private AudioSource audioSource;

        #endregion

        #region Unity

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            RotateDoor();

            // 문이 열렸고, 다음 스테이지 트리거 활성 상태며 아직 한 번도 안 넘겼다면
            if (open && triggerNextStageOnOpen && !stageLoaded)
            {
                stageLoader?.LoadNextStage();
                stageLoaded = true;
            }
        }

        #endregion

        #region Door Logic

        public void ToggleDoor()
        {
            open = !open;
            PlaySound();
        }

        private void PlaySound()
        {
            audioSource.clip = open ? openDoorClip : closeDoorClip;
            audioSource.Play();
        }

        private void RotateDoor()
        {
            float targetAngle = open ? openAngle : closeAngle;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * 5f * smooth
            );
        }

        #endregion
    }
}
