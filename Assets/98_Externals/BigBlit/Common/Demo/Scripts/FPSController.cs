using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BigBlit.ActivePack.Buttons
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {

        [SerializeField] Camera playerCamera = null;

        [Range(0.01f, 1.0f)]
        [SerializeField] float _smoothTime = 0.25f;
        [SerializeField] float walkingSpeed = 7.5f;
        [SerializeField] float runningSpeed = 11.5f;

        [SerializeField] float lookSpeed = 2.0f;
        [SerializeField] float lookXLimit = 45.0f;

        [SerializeField] bool _pointerEnable = true;
        [SerializeField] bool _controllerEnable = false;
        [SerializeField] bool _run = false;
        [SerializeField] bool softwareCursor = true;

        [SerializeField] Texture2D _cursorTexture = null;
        [SerializeField] Texture2D _cursorTextureDown = null;


        Vector2 _mouseRotValues;
        Vector2 _mouseSmootingVel;
        CharacterController characterController;
        Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        private bool canMove = true;



        void Start() {
            characterController = GetComponent<CharacterController>();

        }

        private void OnEnable() {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (softwareCursor)
                Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }

        private void OnDisable() {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        void Update() {
            if (wasPointerPressed()) {
                if (softwareCursor)
                    Cursor.SetCursor(_cursorTextureDown, Vector2.zero, CursorMode.ForceSoftware);

            }
            else if (wasPointerReleased()) {
                if (softwareCursor)
                    Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            }

            if (isRunModifierPressed()) {
                _run = true;
            }
            else
                _run = false;

            if (wasPointerTogglePressed()) {
                if (_pointerEnable) {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                _pointerEnable = !_pointerEnable;
            }

            if (wasControllerTogglePressed()) {
                _controllerEnable = !_controllerEnable;
            }

            if (!_controllerEnable)
                return;


            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
           
            _mouseRotValues = Vector2.SmoothDamp(
                _mouseRotValues, new Vector3(getPointerXAxis() * lookSpeed,
                getPointerYAxis() * lookSpeed), ref _mouseSmootingVel, _smoothTime);

            float curSpeedX = canMove ? (_run ? runningSpeed : walkingSpeed) * getVerticalAxis() : 0;
            float curSpeedY = canMove ? (_run ? runningSpeed : walkingSpeed) * getHorizontalAxis() : 0;

          

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            if (!characterController.isGrounded) {
                moveDirection += Physics.gravity;
            }
            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove) {
                rotationX += -_mouseRotValues.y;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, _mouseRotValues.x, 0);
            }

        }

        bool wasPointerReleased() => Input.GetMouseButtonUp(0);

        bool wasPointerPressed() =>Input.GetMouseButtonDown(0);

        bool isRunModifierPressed() => Input.GetKey(KeyCode.LeftShift);

        bool wasPointerTogglePressed() => Input.GetKeyDown(KeyCode.Space);

        bool wasControllerTogglePressed() => Input.GetKeyDown(KeyCode.Tab);

        float getVerticalAxis() =>  Input.GetAxis("Vertical");
        
        float getHorizontalAxis() =>   Input.GetAxis("Horizontal");
        
        float getPointerXAxis() =>   Input.GetAxis("Mouse X");
        
        float getPointerYAxis() =>   Input.GetAxis("Mouse Y");
        
    }
}
