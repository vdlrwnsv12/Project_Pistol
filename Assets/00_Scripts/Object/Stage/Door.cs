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
        //[SerializeField] private PrototypeStageManager stageLoader;

        [Header("프리팹 연결")]
        [SerializeField] private GameObject rewardUIPrefab;

        private GameObject spawnedRewardUI;
        private bool open = false;
        private AudioSource audioSource;

        #endregion

        #region Unity

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
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

        /// <summary>
        /// 보상 UI 프리팹을 화면에 표시
        /// </summary>
        private void ShowRewardUI()
        {
            if (rewardUIPrefab == null)
            {
                Debug.LogWarning("[Door] rewardUIPrefab이 할당되지 않았습니다.");
                return;
            }

            if (spawnedRewardUI == null)
            {
                spawnedRewardUI = Instantiate(rewardUIPrefab);
                Debug.Log("[Door] 보상 UI 생성 완료");
            }
        }


        #endregion
    }
}
