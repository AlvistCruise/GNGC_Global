using System.Collections.Generic;
using UnityEngine;

public class ExplorerScript : MonoBehaviour
{
    public GameObject[] fileList;
    public GameObject noFileText;    
    public GameObject redDotNotif;
    private List<int> idList;
    private const float startXpos = -7.8f; 
    private const float startYpos = 2.35f; 
    private const float intervalXGap = 8;
    private const float intervalYGap = -7;
    private float row;
    private int numItems;
    void Start()
    {
        idList = new List<int>();
        numItems = 0;
        row = 0;
        foreach(GameObject file in fileList)
        {
            file.SetActive(false);
        }
        noFileText.SetActive(true); 
        redDotNotif.SetActive(false);
    }

    public void AddFile(int id)
    {
        if(noFileText.activeSelf)noFileText.SetActive(false);
        if(!ValidateId(id))return;
        idList.Add(id);
        Debug.Log($"Added item to explorer: {id}");

        /*
            1(8, 0) 2(16, 0) 3(24, 0)
            
            4(8, 1) 5(16, 1) 6(24, 1)
            
            34 - (8*3) * 1 
        */
        
        foreach(GameObject file in fileList)
        {
            if((file.GetComponent<ProfileId>()).Id == id)
            {
                if(numItems >= 3)row = 1;
                file.transform.localPosition = new Vector3(
                    startXpos + intervalXGap * numItems - ((intervalXGap * 3) * row), 
                    startYpos + intervalYGap * row, 
                    0
                );
                // Debug.Log($"Location: x{startXpos + intervalXGap * numItems} y{startYpos + intervalYGap * row} z{0}");
                file.SetActive(true);
            }
        }
        numItems+=1;
        redDotNotif.SetActive(true);
    }


    private bool ValidateId(int id)
    {
        foreach (int _id in idList)
        {
            if(id == _id)return false;
        }
        return true;
    }
}
