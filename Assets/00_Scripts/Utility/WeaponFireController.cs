// using UnityEngine;
// using System.Collections.Generic;
//
// namespace Delete
// {
//     public class WeaponFireController : MonoBehaviour
//     {
//         public WeaponStatHandler weaponStatHandler;
//         [SerializeField] public int currentAmmo;
//         private Vector3 camRootOriginPos;
//         private Vector3 currentCamRootTargetPos;
//         private Quaternion currentHandTargetRot;
//         private Quaternion initialLocalRotation;
//         
//         [SerializeField] private List<GameObject> optics;
//
//         [SerializeField] private float targetCamY = 0.165f;
//
//
//         #region Unity Methods
//
//         public void InitReferences()
//         {
//             weaponStatHandler = GetComponent<WeaponStatHandler>();
//             Player player = weaponStatHandler.playerObject.GetComponent<Player>();
//
//             if (weaponStatHandler.weaponData == null)
//             {
//                 string nameToSerch = gameObject.name.Replace("(Clone)", "").Trim();
//                 weaponStatHandler.weaponData = Resources.Load<WeaponSO>($"Data/SO/WeaponSO/{nameToSerch}");
//                 if (weaponStatHandler.weaponData == null)
//                 {
//                     Debug.Log($"[InitReferences] WeaponData '{nameToSerch}'을(를) 찾을 수 없습니다.");
//                 }
//                 else
//                 {
//                     Debug.Log($"[InitReferences] WeaponData '{nameToSerch}'자동 할당.");
//                 }
//             }
//             
//             weaponStatHandler.WeaponDataFromSO();
//             initialLocalRotation = weaponStatHandler.handransform.localRotation;
//             camRootOriginPos = weaponStatHandler.camRoot.localPosition;
//             currentAmmo = weaponStatHandler.MaxAmmo;
//             //weaponStatHandler.BindToWeapon(this);
//             weaponStatHandler.onAmmoChanged(currentAmmo, weaponStatHandler.MaxAmmo);
//             optics = new List<GameObject> { weaponStatHandler.redDot, weaponStatHandler.holographic };
//             //player.weaponFireController = this;
//         }
//
//         #endregion
//
//         #region ADS
//
//         public void HandleADS()
//         {
//             Debug.Log("HandleADS");
//             if (!weaponStatHandler.isReloading)
//             {
//                 Debug.Log("true false전환");
//                 weaponStatHandler.isADS = !weaponStatHandler.isADS;
//             }
//
//             if (weaponStatHandler.isADS)
//             {
//                 currentCamRootTargetPos = weaponStatHandler.adsPosition;
//                 currentHandTargetRot = initialLocalRotation;
//
//                 // redDot 상태에 따라 타겟 Y 설정
//                 // targetCamY = (weaponStatHandler.redDot != null && weaponStatHandler.redDot.activeSelf) ? 0.18f : 0.16f;
//                 bool isOpticActive = optics.Exists(optics => optics.activeSelf); //조준경이 하나라도 켜져 있으면
//                 targetCamY = isOpticActive ? 0.18f : 0.16f;
//             }
//             else
//             {
//                 currentCamRootTargetPos = camRootOriginPos;
//                 currentHandTargetRot = initialLocalRotation;
//
//                 // 정조준 해제 시 기본값으로 복구
//                 targetCamY = 0.16f;
//             }
//
//             // FOV 보간
//             float targetFOV = weaponStatHandler.isADS ? 40f : 60f;
//             weaponStatHandler.playerCam.fieldOfView = Mathf.Lerp(weaponStatHandler.playerCam.fieldOfView, targetFOV,
//                 Time.deltaTime * 10f);
//
//             // 위치/회전 보간
//             weaponStatHandler.camRoot.localPosition = Vector3.Lerp(weaponStatHandler.camRoot.localPosition,
//                 currentCamRootTargetPos, Time.deltaTime * weaponStatHandler.camMoveSpeed);
//             weaponStatHandler.handransform.localRotation = Quaternion.Lerp(weaponStatHandler.handransform.localRotation,
//                 currentHandTargetRot, Time.deltaTime * 10f);
//
//             //Y 위치만 따로 부드럽게 보간
//             Vector3 camLocalPos = weaponStatHandler.playerCam.transform.localPosition;
//             camLocalPos.y = Mathf.Lerp(camLocalPos.y, targetCamY, Time.deltaTime * 10f);
//             weaponStatHandler.playerCam.transform.localPosition = camLocalPos;
//         }
//
//         #endregion
//     }
// }
//
