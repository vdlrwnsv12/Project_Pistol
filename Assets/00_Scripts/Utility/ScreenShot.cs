#if UNITY_EDITOR
using UnityEngine;
using System.IO;


public class ObjectScreenshot : MonoBehaviour
{
    /// <summary>
    /// 찍을 카메라 등록
    /// 카메라의 cullingMask를 CaptureOnly로
    /// 이미지 크기 지정
    /// 찍을 오브젝트들 등록
    /// 찍을 오브젝트의 레이어를 CaptureOnly로 설정
    /// 조명 등록
    /// 숫자 키로 찍을 오브젝트 선택
    /// K눌러서 찍기(편한 키가 뭔지 모르겠습니다 아무 키나 넣은거예요)
    /// </summary>

    [Header("ScreenShot Cam")]
    public Camera captureCamera; // 전용 카메라
    private RenderTexture renderTexture;
    [Header("Output Size")]
    public int textureWidth = 512;
    public int textureHeight = 512;
    [Header("Target Object")]
    public GameObject[] targetObjects; //오브젝트들 등록
    public GameObject currentTarget; // 찍을 오브젝트
    public Light additionalLight;

    void Start()
    {
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(textureWidth, textureHeight, 24);
        }
        foreach(var obj in targetObjects)
        {
            obj.SetActive(false);
        }
    }


    void Update()
    {
        for(int i = 1; i <= 9; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                ActivateTarget(i - 1);
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)) ActivateTarget(0);
        if(Input.GetKeyDown(KeyCode.Alpha2)) ActivateTarget(1);
        if(Input.GetKeyDown(KeyCode.Alpha3)) ActivateTarget(2);
        if(Input.GetKeyDown(KeyCode.Alpha4)) ActivateTarget(3); 
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeObjectScreenshot(currentTarget);
        }
    }

    private void ActivateTarget(int index)
    {
        if(index < 0 || index >= targetObjects.Length)
        {
            return;
        }
        for(int i = 0; i < targetObjects.Length; i++)
        {
            targetObjects[i].SetActive(i == index);
        }
        currentTarget = targetObjects[index];
    }

    private void TakeObjectScreenshot(GameObject targetObject)
    {

        // 대상 오브젝트를 전용 레이어로 옮김
        int captureLayer = LayerMask.NameToLayer("CaptureOnly");
        int originalLayer = targetObject.layer;
        SetLayerRecursively(targetObject, captureLayer);

        Renderer targetRenderer = targetObject.GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            targetRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; // 그림자 끄기
            targetRenderer.receiveShadows = false; // 그림자 받지 않음
        }


        // 카메라 설정
        captureCamera.clearFlags = CameraClearFlags.SolidColor;
        captureCamera.backgroundColor = new Color(0, 0, 0, 0);
        captureCamera.cullingMask = 1 << captureLayer;
        captureCamera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;

        // 렌더링
        captureCamera.Render();

        // 텍스처 저장
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        byte[] pngData = tex.EncodeToPNG();
        string savePath = Path.Combine(Application.dataPath, $"Resources/Sprites/{targetObject.name}.png");
        File.WriteAllBytes(savePath, pngData);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();

        string assetPath = "Assets" + savePath.Replace(Application.dataPath, "");
        var importer = UnityEditor.AssetImporter.GetAtPath(assetPath) as UnityEditor.TextureImporter;
        if (importer != null)
        {
            importer.textureType = UnityEditor.TextureImporterType.Sprite;
            importer.alphaIsTransparency = true;
            importer.SaveAndReimport();
        }
#endif

        // 원상복구
        RenderTexture.active = null;
        captureCamera.targetTexture = null;
        SetLayerRecursively(targetObject, originalLayer);
        Destroy(tex);

        Debug.Log("오브젝트 스크린샷 저장됨: " + savePath);
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
#endif