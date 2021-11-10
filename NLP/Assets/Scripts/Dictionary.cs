using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class Dictionary : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform content;
    [SerializeField] private LoadTextController textController;
    [SerializeField] private Button updateButton;
    [SerializeField] private TMP_InputField addWordField;
    [SerializeField] private TMP_InputField findWord;
    [SerializeField] private TextMeshProUGUI findResult;


    private Dictionary<string, Pair<int, string>> dictionary;
    private List<Tile> tiles = new List<Tile>();

    public void Find(string word)
    {
        if (dictionary.ContainsKey(word))
            findResult.text = dictionary[word].First.ToString();
        else
            findResult.text = "No";
    }

    private void Start()
    {
        findWord.onEndEdit.AddListener((string str) => Find(str));
        dictionary =
            JsonConvert.DeserializeObject<Dictionary<string, Pair<int, string>>>(PlayerPrefs.GetString("Dict"));
        if (dictionary == null) dictionary = new Dictionary<string, Pair<int, string>>();
        SpawnTiles();
        updateButton.onClick.AddListener(() =>
        {
            ParseText(textController.CurrentText);
            SpawnTiles();
        });

        addWordField.onEndEdit.AddListener((string newValue) =>
        {
            if (!dictionary.ContainsKey(newValue))
            {
                dictionary.Add(newValue, new Pair<int, string>(0, "----"));
                SpawnTile(newValue, 0, "----");
            }

            addWordField.text = null;
        });
    }

    private void ParseText(string text)
    {
        var data = text.Split(' ', '\n');
        var d = JsonConvert.DeserializeObject<Dictionary<string, Pair<int, string>>>(
            PlayerPrefs.GetString(textController.CurrentTextNumber + "t"));
        if (d == null) d = new Dictionary<string, Pair<int, string>>();
        //print(d["for"].First);
        MinusDict(d, false);
        d.Clear();

        for (int i = 0; i < data.Length; ++i)
        {
            if (data[i].EndsWith("."))
            {
                data[i] = data[i].Remove(data[i].Length - 1);
            }

            data[i] = data[i].ToLower();

            if (d.ContainsKey(data[i]))
            {
                ++d[data[i]].First;
            }
            else
            {
                d[data[i]] = new Pair<int, string>(1, "----");
            }
        }

        MinusDict(d, true);
        var json = JsonConvert.SerializeObject(dictionary);
        PlayerPrefs.SetString("Dict", json);
        PlayerPrefs.SetString(textController.CurrentTextNumber + "t", JsonConvert.SerializeObject(d));
        RefreshTiles();
    }

    private void MinusDict(Dictionary<string, Pair<int, string>> d, bool plus)
    {
        foreach (var pair in d)
        {
            if (dictionary.ContainsKey(pair.Key))
            {
                if (plus)
                    dictionary[pair.Key].First += d[pair.Key].First;
                else
                {
                    dictionary[pair.Key].First -= d[pair.Key].First;
                    dictionary[pair.Key].First = Mathf.Max(dictionary[pair.Key].First, 0);
                }
            }
            else
            {
                if (plus)
                    dictionary[pair.Key] = new Pair<int, string>(pair.Value.First, pair.Value.Second);
            }
        }
    }

    private void RefreshTiles()
    {
        foreach (var tile in tiles)
        {
            tile.Frequency = dictionary[tile.Word].First;
        }
    }

    private void SpawnTiles()
    {
        foreach (var pair in dictionary)
        {
            SpawnTile(pair.Key, pair.Value.First, pair.Value.Second);
        }
    }

    private void SpawnTile(string word, int frequency, string tag)
    {
        var tile = Instantiate(tilePrefab, content);
        tile.Word = word;
        tile.Frequency = frequency;
        tile.Tag = tag;
        tile.OnDelete.AddListener(OnTileDelete);
        tile.OnEndEdit.AddListener(OnTileChangeValue);
        tiles.Add(tile);
    }

    private void OnTileChangeValue(string newKey, Tile tile)
    {
        tiles.ForEach(localTile =>
        {
            if (localTile.Word != newKey || localTile == tile) return;
            tile.Frequency += localTile.Frequency;
            Destroy(localTile.gameObject);
        });
    }

    private void OnTileDelete(string key)
    {
        dictionary.Remove(key);
    }
}