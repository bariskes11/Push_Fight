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
    private void Start()
    {
        spritesInChildren = this.GetComponentsInChildren<SpriteRenderer>();
        GenerateRandom();
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
    public void CreateQuestion()
    {
        int i = 0;
        foreach (var item in spritesInChildren)
        {
            item.sprite = soruSablonu.shapes_Normal[selectedIndexes.ElementAt(i)];
            i ++;
        }
    }

}
