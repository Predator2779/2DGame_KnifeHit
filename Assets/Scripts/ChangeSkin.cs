using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSkin : MonoBehaviour
{
    private Slider _slider;
    private int _skinIndex = 0;

    public Image _skin;
    public Sprite[] _otherSkins;

    /// <summary>
    /// Start.
    /// </summary>
    void Start()
    {
        _slider = gameObject.GetComponent<Slider>();
        _slider.maxValue = _otherSkins.Length;
    }

    /// <summary>
    /// Update.
    /// </summary>
    void Update()
    {
        Change();
    }

    private void Change()
    {
        _skinIndex = (int)_slider.value - 1;
        _skin.sprite = _otherSkins[_skinIndex];
    }
}
