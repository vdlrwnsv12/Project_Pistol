using UnityEngine;

namespace CameraDoorScript
{
    /// <summary>
    /// 카메라 앞 문과의 거리 체크 후 문 열기 처리
    /// </summary>
    public class CameraOpenDoor : MonoBehaviour
    {
        [Header("문과의 감지 거리")]
        [SerializeField] private float detectDistance = 3f;

        [Header("문구 UI")]
        [SerializeField] private GameObject interactText;

        private void Update()
        {
            CheckForDoor();
        }

        /// <summary>
        /// Raycast를 이용하여 문을 감지하고, 문 앞에서 상호작용
        /// </summary>
        private void CheckForDoor()
        {
            RaycastHit hit;
            bool doorFound = false;

            if (Physics.Raycast(transform.position, transform.forward, out hit, detectDistance))
            {
                DoorScript.Door door = hit.transform.GetComponent<DoorScript.Door>();

                if (door != null)
                {
                    doorFound = true;
                    interactText.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        door.ToggleDoor(); // OpenDoor → ToggleDoor로 리팩토링된 메서드 사용
                    }
                }
            }

            if (!doorFound)
            {
                interactText.SetActive(false);
            }
        }
    }
}
