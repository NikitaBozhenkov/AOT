using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextStorage : MonoBehaviour
{
    [SerializeField] private TextsConfig textsConfig;

    public string GetText(int number)
    {
        if (number <= 0 || number >= textsConfig.Texts.Length) throw new ArgumentOutOfRangeException(nameof(number));
        number -= 1;
        

        return PlayerPrefs.GetString((number).ToString(), textsConfig.Texts[number].text);
    }

    public void SaveText(int number, string text)
    {
        number -= 1;
        PlayerPrefs.SetString(number.ToString(), text);
    }
}