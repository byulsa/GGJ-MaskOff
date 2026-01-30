using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public enum MaskWear
{
    None,
    Wear
}
public class cardskin : MonoBehaviour
{
    public MaskWear maskWear = MaskWear.None;
    public SpriteRenderer Eyes;
    public SpriteRenderer Face;
    public SpriteRenderer clothes;
    public SpriteRenderer Mask;
    public SpriteRenderer Hair;
    public GameObject BackHair;

    [Header("스킨리스트")]
    public List<Sprite> EyesList;
    public List<Sprite> FaceList;
    public List<Sprite> ClothesList;
    public List<Sprite> MaskList;
    public List<Sprite> HairList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Random.Range(0, 2) == 1)
        {
            maskWear = MaskWear.Wear;
            Mask.sprite = MaskList[Random.Range(0, MaskList.Count)];
        }
        else
        {
            maskWear = MaskWear.None;
            Mask.sprite = null;
        }
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
        Hair.sprite = HairList[Random.Range(0, HairList.Count)];
        if(Hair.sprite == HairList[3])
        {
            BackHair.SetActive(true);
        }
        else
        {
            BackHair.SetActive(true);
        }
        
    }
}
