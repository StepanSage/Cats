using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LocalizeManager;

public class DialogSettings : Dialog
{

  [Header("Sound Slider")]
  [SerializeField]
  private Slider _soundSlider;
  [SerializeField]
  private TextMeshProUGUI _soundSliderMin;
  [SerializeField]
  private TextMeshProUGUI _soundSliderMax;
  [SerializeField]
  private GameObject _soundSliderIconOn;
  [SerializeField]
  private GameObject _soundSliderIconOff;

  [Space(10f)]
  [Header("Music Slider")]
  [SerializeField]
  private Slider _musicSlider;
  [SerializeField]
  private TextMeshProUGUI _musicSliderMin;
  [SerializeField]
  private TextMeshProUGUI _musicSliderMax;
  [SerializeField]
  private GameObject _musicSliderIconOn;
  [SerializeField]
  private GameObject _musicSliderIconOff;

  [Space(20f)]
  [SerializeField]
  private LanguageElement _languagePrefab;
  [SerializeField]
  private Transform _languagesContent;

  private List<LanguageElement> _languageItems = new List<LanguageElement>();

  // ===================================================================================================
  public void OnEnable()
  {
    _soundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
    _musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
  }

  // ===================================================================================================
  public void OnDisable()
  {
    _soundSlider.onValueChanged.RemoveListener(OnSoundSliderValueChanged);
    _musicSlider.onValueChanged.RemoveListener(OnMusicSliderValueChanged);
  }

  // ===================================================================================================
  public void Activate()
  {
    Redraw();
    Open();
  }

  // ===================================================================================================
  public void Redraw()
  {
    _soundSliderMax.text = _soundSlider.maxValue.ToString();
    _musicSliderMax.text = _musicSlider.maxValue.ToString();

    var sound = Global.Instance.SoundService.GetVolumeAudio();
    var music = Global.Instance.SoundService.GetVolumeMusic();

    _soundSlider.value = Mathf.CeilToInt(_soundSlider.maxValue * sound);
    _musicSlider.value = Mathf.CeilToInt(_musicSlider.maxValue * music);


    List<ELanguage> languages = new List<ELanguage> 
    { 
      ELanguage.RU, 
      ELanguage.EN, 
      ELanguage.DE, 
      ELanguage.FR
    };

    foreach (var prod in _languageItems)
    {
      prod.gameObject.SetActive(false);
    }

    int cacheEC = _languageItems.Count;
    int needCount = languages.Count();

    if (cacheEC < needCount)
    {
      for (int i = 0; i < (needCount - cacheEC); i++)
      {
        var elem = Instantiate(_languagePrefab, _languagesContent);
        _languageItems.Add(elem);
      }
    }

    int ij = 0;
    foreach (var language in languages)
    {
      _languageItems[ij].Init(language, OnButtonChangeLanguage);
      ij++;
    }
  }

  // ===================================================================================================
  private void OnSoundSliderValueChanged(float value)
  {
    _soundSliderMin.text = value.ToString();

    _soundSliderIconOn.SetActive(value > 0);
    _soundSliderIconOff.SetActive(value <= 0);

    Global.Instance.SoundService.AddVolumeAudio(value/ _soundSlider.maxValue);
    Global.Instance.SoundService.PlayOne("slider_value_change");
  }

  // ===================================================================================================
  private void OnMusicSliderValueChanged(float value)
  {
    _musicSliderMin.text = value.ToString();

    _musicSliderIconOn.SetActive(value > 0);
    _musicSliderIconOff.SetActive(value <= 0);

    Global.Instance.SoundService.AddVolumeMusic(value / _musicSlider.maxValue);
    Global.Instance.SoundService.PlayOne("slider_value_change");
  }

  // ===================================================================================================
  public void OnButtonChangeLanguage(ELanguage language)
  {
    if (Global.Instance.PlayerData.Language.Equals(language))
      return;

    LocalizeManager.Instance.SetLanguage(language);
  }
}