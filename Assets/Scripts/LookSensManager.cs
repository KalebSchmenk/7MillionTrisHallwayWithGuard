using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LookSensManager : MonoBehaviour
{

    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _valueText;

    public static float _sliderValue = 0.25f;


    private void Awake()
    {
        _slider.value = _sliderValue;
    }

    private void Update()
    {
        _valueText.text = "Current value: " + (_sliderValue * 10).ToString("#.##");
    }

    public void SetSliderValue(float inVal)
    {
        _sliderValue = inVal;
       
        

        _sliderValue = Mathf.Clamp(_sliderValue, 0.01f, 1.0f);
    }


}
