using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class cardskin : MonoBehaviour
{
    public SpriteRenderer Eyes;
    public SpriteRenderer Face;
    public SpriteRenderer clothes;

    [Header("스킨리스트")]
    public List<Sprite> EyesList;
    public List<Sprite> FaceList;
    public List<Sprite> ClothesList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Ran();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Ran();
        }
    }
    void Ran()
    {
        Eyes.sprite = EyesList[Random.Range(0, EyesList.Count)];
        Face.sprite = FaceList[Random.Range(0, FaceList.Count)];
        clothes.sprite = ClothesList[Random.Range(0, ClothesList.Count)];
    }
}
