using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public List<Text> SelectText;
    public Transform SelectObject;
    private int selectIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectIndex--;
            if (selectIndex < 0)
            {
                selectIndex = SelectText.Count - 1;
            }
            SetSelectText(selectIndex);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectIndex++;
            if (selectIndex >= SelectText.Count)
            {
                selectIndex = 0;
            }
            SetSelectText(selectIndex);
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (selectIndex == 0)
            {
                gameObject.GetComponent<Animator>().Play("MoveScene");
            }
            else if (selectIndex == 1)
            {
                Application.Quit();
            }
        }
    }
    public void SetSelectText(int index)
    {      
        selectIndex = index;
        for (int i = 0; i < SelectText.Count; i++)
        {
            if (i == selectIndex)
            {
                SelectText[i].color = Color.black;
            }
            else
            {
                SelectText[i].color = Color.white;
            }
        }
        SelectObject.position = new Vector3(SelectObject.position.x, SelectText[selectIndex].transform.position.y, SelectObject.position.z);
        SelectObject.GetComponent<Animator>().Play("select");
    }
    public void NextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
