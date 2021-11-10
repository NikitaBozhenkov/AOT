using UnityEngine;

[CreateAssetMenu(fileName = "textsConfig", menuName = "textsConfig")]
public class TextsConfig : ScriptableObject
{
    [SerializeField] private TextAsset[] texts;

    public TextAsset[] Texts => texts;
}