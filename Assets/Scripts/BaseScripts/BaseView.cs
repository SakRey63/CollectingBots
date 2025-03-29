using System;
using TMPro;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetText(int number)
    {
        _text.SetText(Convert.ToString(number));
    }
}