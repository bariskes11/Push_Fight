using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SoruSablonu",menuName ="ScriptableObjects/SoruSablonuOlustur",order =1)]
public class QuestionScriptable : ScriptableObject
{
    public Sprite[] shapes_dotted;
    public Sprite[] shapes_Normal;
    public string[] Tags;
    public Color[] shape_Colors;
}
