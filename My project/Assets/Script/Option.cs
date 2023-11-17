using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Option : MonoBehaviour
{
    [SerializeField] private SetUI optionUi;
    [SerializeField] private Slider soundeffectslider;
    [SerializeField] private Slider bgmslider;
    private bool optionsWindow;
    private bool resetButton;
    private void Start()
    {
        resetButton = false;
        bgmslider.value = 1f;
        soundeffectslider.value = 1f;
        optionsWindow = false;
        optionUi.Off();
    }

    public void OnReSet()
    {
        resetButton = true;
    }
    public void OnClick()
    {
        optionUi.On();
        optionsWindow = true;
    }

    public void OnExitClick()
    {
        AudioMgr.Instance.VolumeUpdate(soundeffectslider.value, bgmslider.value);
        optionsWindow = false;
        optionUi.Off();

    }
    private void Update()
    {
        if (optionsWindow) AudioMgr.Instance.VolumeUpdate(soundeffectslider.value, bgmslider.value);
    }
}
