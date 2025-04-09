using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsCamera : MonoBehaviour
{
    [SerializeField] private float rotCamXAxisSpeed = 5;
    [SerializeField] float rotCamYAxisSpeed = 5;

    private float limitMinx = -80;
    private float limitMaxX = 50;
    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateRotate(float mouseX, float mouseY)
    {

        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamXAxisSpeed;

        eulerAngleX = ClampAngle(eulerAngleX, limitMinx, limitMaxX);
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);

    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle,min,max);  
    }


}
