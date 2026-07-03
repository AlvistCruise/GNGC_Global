using UnityEngine;
using TMPro;
public class WindowScript : MonoBehaviour
{
    public int Id;
    public TMP_Text WindowText;
    public GameObject CloseBtn; 

    void Start()
    {
        WindowText.text = "Window " + Id.ToString();  
    }

    void Update()
    {
        
    }

}
