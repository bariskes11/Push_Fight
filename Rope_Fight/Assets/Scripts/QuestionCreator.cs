using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionCreator : MonoBehaviour
{
    public QuestionScriptable soruSablonu;
    private SpriteRenderer[] spritesInChildren;
    private List<int> selectedIndexes = new List<int>();
    private Sprite[] needed_Doted_Sprites;
    private void Start()
    {
        spritesInChildren = this.GetComponentsInChildren<SpriteRenderer>().Where(x => x.name.Contains("item")).OrderBy(x => x.name).ToArray();
        GenerateRandom();
        foreach (var item in spritesInChildren)
        {
            item.enabled = false;
        }
    }
    void GenerateRandom()
    {
        for (int j = 0; j < spritesInChildren.Length; j++)
        {
            int Rand = Random.Range(0, spritesInChildren.Length);
            while (selectedIndexes.Contains(Rand))
            {
                Rand = Random.Range(0, spritesInChildren.Length);
            }
            selectedIndexes.Add(Rand);
        }
    }
    public int[] CreateQuestion()
    {
        int i = 0;
        int[] r = new int[spritesInChildren.Length];
        needed_Doted_Sprites = new Sprite[spritesInChildren.Length];
        foreach (var item in spritesInChildren)
        {
            item.enabled = true;
            item.sprite = soruSablonu.shapes_Normal[selectedIndexes.ElementAt(i)];
            item.tag = soruSablonu.Tags[selectedIndexes.ElementAt(i)];
            r[i] = selectedIndexes.ElementAt(i);
            i++;
        }
        return r;
    }
    public void CloseQuestion()
    {
        needed_Doted_Sprites = new Sprite[spritesInChildren.Length];
        foreach (var item in spritesInChildren)
        {
            item.enabled = false;
        }
    }

}
