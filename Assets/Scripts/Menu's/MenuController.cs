using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using ScreenFx;
using UnityEngine.Audio;

public class MenuController : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject mainMenu;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 0.5f;
    [Space(10)]
    [SerializeField] AudioMixer volumeMixer = null;

    [Header("Gameplay Settings")]

    [Header("Graphics Settings")]
    public Volume volume;
    public ColorAdjustments colorAdjustments;

    [Space(10)]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;

    [Space(10)]
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    
    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    [Header("Confirmation Prompt")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Resolutions Dropdowns")]
    public Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private void Awake()
    {
        this.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(true);

        Time.timeScale = 1f;
    }

    private void Start()
    {
        //This entire code, checks the resolution of the screen and gives options depending on what is found
        //DEPENDING on the screen size on start and places the resolution number into the [i] box for the dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    //Sets the resolutions based on what is found when checking the monitor screen
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    //Takes the value in the slider and shows it to the player in text
    public void SetVolume(float volume)
    {
        //AudioListener.volume = Mathf.RoundToInt(volume);
        //When making a mixer, keep it a float aswell as use Mathf.Log10 because
        //Mixer's are non-linear
        volumeMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        volumeTextValue.text = (volume*100).ToString("0");
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    //Controls and sets the brightness
    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    //Makes the application Fullscreen
    public void SetFullScreen(bool isFullscreen)
    {
        _isFullScreen = isFullscreen;
    }

    //Sets the quality on the players preferences
    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    //Applies the graphics set in the functions and saves it in the strings 
    //"masterBrightness","masterQuality","masterFullscreen"
    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments)) 
        { colorAdjustments.postExposure.value = _brightnessLevel; }

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        //_isFullScreen checks with a bool to see if it is fullscreen or not
        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        StartCoroutine(ConfirmationBox());
    }

    public IEnumerator ConfirmationBox()
    {
        //Shows the player when options have been saved
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    /// <summary>
    /// Resets the settings via the string e.g "Graphics"
    /// Current MenuType strings available are "Graphics" & "Audio
    /// </summary>
    /// <param name="MenuType"></param>
    
    //Also applies the settings after reset
    public void ResetButton(string MenuType) 
    {
        if (MenuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            //Resets the resolution to the screen's current resolution via width and height
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }

        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
        }
    }
}
