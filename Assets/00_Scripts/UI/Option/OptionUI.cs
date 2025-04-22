using UnityEngine;
using UnityEngine.UI;

public class OptionUI : PopupUI
{
    [Header("Mouse Sensitivity")]
    public Text hipSensitivityText;
    public Text adsSensitivityText;
    public float hipSensitivity = 1f;
    public float adsSensitivity = 1f;

    [Header("Sound Sliders")]
    public Slider masterSlider;
    public Slider seSlider;
    public Slider bgmSlider;

    public Text masterValueText;
    public Text seValueText;
    public Text bgmValueText;

    protected override void Awake()
    {
        base.Awake();
        
        masterSlider.minValue = 0;
        masterSlider.maxValue = 100;
        seSlider.minValue = 0;
        seSlider.maxValue = 100;
        bgmSlider.minValue = 0;
        bgmSlider.maxValue = 100;

        // masterSlider.value = SoundManager.Instance.GetMasterVolume() * 100f;

        UpdateSensitivityTexts();

        // 사운드 슬라이더 값 변경 리스너 등록
        masterSlider.onValueChanged.AddListener((v) =>
        {
            masterValueText.text = v.ToString("0");
            SoundManager.Instance.SetMasterVolume(v / 100f);
        });

        seSlider.onValueChanged.AddListener((v) =>
        {
            seValueText.text = v.ToString("0");
            SoundManager.Instance.SetSFXVolume(v / 100f);
        });

        bgmSlider.onValueChanged.AddListener((v) =>
        {
            bgmValueText.text = v.ToString("0");
            SoundManager.Instance.SetMusicVolume(v / 100f);
        });
    }

    private void OnEnable()
    {
        // SoundManager의 값을 가져와서 슬라이더에 적용
        masterSlider.value = SoundManager.Instance.masterVol * 100f;
        seSlider.value = SoundManager.Instance.sfxVol * 100f;
        bgmSlider.value = SoundManager.Instance.backgroundMusicVol * 100f;

        // 슬라이더 값에 맞는 텍스트 갱신
        UpdateSoundTexts();

    }

    #region Sensitivity Control
    public void ChangeHipSensitivity(float delta)
    {
        hipSensitivity = Mathf.Clamp(hipSensitivity + delta, 0f, 9.9f);
        UpdateSensitivityTexts();
        // GameSettings.Instance.HipSensitivity = hipSensitivity;
    }

    public void ChangeADSSensitivity(float delta)
    {
        adsSensitivity = Mathf.Clamp(adsSensitivity + delta, 0f, 9.9f);
        UpdateSensitivityTexts();
        // GameSettings.Instance.ADSSensitivity = adsSensitivity;
    }

    private void UpdateSensitivityTexts()
    {
        hipSensitivityText.text = hipSensitivity.ToString("0.0");
        adsSensitivityText.text = adsSensitivity.ToString("0.0");
    }
    #endregion

    private void UpdateSoundTexts()
    {
        masterValueText.text = masterSlider.value.ToString("0");
        seValueText.text = seSlider.value.ToString("0");
        bgmValueText.text = bgmSlider.value.ToString("0");
    }

    public void OnClickClose()
    {
        UIManager.Instance.ClosePopUpUI();
    }
}
