using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private TMP_InputField wordField;
    [SerializeField] private TextMeshProUGUI frequencyText;
    [SerializeField] private TextMeshProUGUI tagText;
    [SerializeField] private Button editButton;

    private string word;
    private int frequency;
    private string tag;

    #region Properties

    public string Word {
        get => word;
        set {
            word = value;
            wordField.text = word;
        }
    }

    public int Frequency {
        get => frequency;
        set {
            frequency = value;
            frequencyText.text = frequency.ToString();
        }
    }

    public string Tag {
        get => tag;
        set {
            tag = value;
            tagText.text = tag;
        }
    }

    #endregion


    public UnityEvent<string> OnDelete;
    public UnityEvent<string, Tile> OnEndEdit;

    public void Setup(string word, string frequency, string tag)
    {
        this.word = word;
        this.frequency = int.Parse(frequency);
        this.tag = tag;

        wordField.text = word;
        frequencyText.text = frequency;
        tagText.text = tag;
    }

    private void Start()
    {
        wordField.onEndEdit.AddListener(newValue => {
            wordField.interactable = false;
            editButton.interactable = true;
            word = newValue;
            OnEndEdit?.Invoke(newValue, this);
        });

        editButton.onClick.AddListener(() => {
            wordField.interactable = true;
            editButton.interactable = false;
        });
    }

    public void Delete()
    {
        OnDelete?.Invoke(wordField.text);
        Destroy(gameObject);
    }
}