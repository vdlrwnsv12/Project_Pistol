using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        #region Parameters

        [Header("문 열림 설정")]
        [SerializeField] private float smooth = 1.0f;
        [SerializeField] private float openAngle = -90f;
        [SerializeField] private float closeAngle = 0f;

        [Header("사운드")]
        [SerializeField] private AudioClip openClip;
        [SerializeField] private AudioClip closeClip;

        [Header("스테이지 연동")]
        [SerializeField] private StageLoader stageLoader;

        [Header("동작 선택")]
        [SerializeField] private bool isNext = false;   // 다음 스테이지 로드
        [SerializeField] private bool isRemove = false; // 이전 스테이지 제거

        private bool open = false;
        private bool stageHandled = false;
        private AudioSource audioSource;

        #endregion

        #region Unity

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            if (stageLoader == null)
            {
                stageLoader = FindObjectOfType<StageLoader>();
            }
        }

        private void Update()
        {
            RotateDoor();

            if (open && !stageHandled && stageLoader != null)
            {
                if (isNext)
                {
                    stageLoader.LoadNextStage();
                }

                if (isRemove)
                {
                    stageLoader.RemovePreviousStage();
                }

                stageHandled = true;
            }
        }

        #endregion

        #region Logic

        public void ToggleDoor()
        {
            open = !open;
            audioSource.clip = open ? openClip : closeClip;
            audioSource.Play();
        }

        private void RotateDoor()
        {
            float target = open ? openAngle : closeAngle;
            Quaternion targetRot = Quaternion.Euler(0, target, 0);

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRot,
                Time.deltaTime * 5f * smooth
            );
        }

        #endregion
    }
}
