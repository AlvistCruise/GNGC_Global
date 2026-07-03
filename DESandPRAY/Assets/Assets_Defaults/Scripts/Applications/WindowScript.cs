using UnityEngine;
using TMPro;
public class WindowScript : MonoBehaviour
{
    string[] IdName =  {"Explorer", "Email", "Meawcord", "ArtInspection"};
    public int Id;
    public TMP_Text WindowText;

    void Start()
    {
        WindowText.text = IdName[Id];  
    }

    void Update()
    {
        
    }

}
