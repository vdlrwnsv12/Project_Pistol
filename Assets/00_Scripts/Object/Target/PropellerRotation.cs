using UnityEngine;
using System.Collections;

public class PropellerRotation : MonoBehaviour
{

    [SerializeField] private Transform[] propellerTransforms;
    [SerializeField] private float power = 10f;
    public bool counterclockwise = false;

    // 오우 씥
    // power를 25.715로 하면 프로펠러가 멈추는것 처럼 보임 
    // 이유는 fixedDeltatime은 50fps기준 0.02
    // power = 25.715일때
    //power * 700 * 0.02 = 360.01도
    //즉 1프레임에 한바퀴 돌아버려서 안돌아가는 것 처럼 보임
    //쌌다bro

    void FixedUpdate()
    {
        float rotationAmount = power * 700f * Time.fixedDeltaTime * (counterclockwise ? -1f : 1f);

        foreach (Transform propeller in propellerTransforms)
        {
            if(propeller != null)
            {
                propeller.Rotate(0f, 0f, rotationAmount);
            }
        }
        
    }
}
