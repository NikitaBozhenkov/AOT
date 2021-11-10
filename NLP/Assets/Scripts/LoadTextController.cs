using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadTextController : MonoBehaviour
{
    [SerializeField] private TextStorage textStorage;
    [SerializeField] private Button loadTextButton;
    [SerializeField] private Button saveTextButton;
    [SerializeField] private TMP_InputField chooseText;
    [SerializeField] private TMP_InputField text;

    public string CurrentText => text.text;

    public int CurrentTextNumber { get; private set; }

    public UnityEvent<Dictionary<string, Pair<int, string>>> OnParseFinish;

    private void Start()
    {
        loadTextButton.onClick.AddListener(() =>
        {
            if (!int.TryParse(chooseText.text, out var number)) return;

            CurrentTextNumber = number;
            text.text = textStorage.GetText(number);
        });

        saveTextButton.onClick.AddListener(() =>
        {
            textStorage.SaveText(CurrentTextNumber, text.text);
        });
    }

}