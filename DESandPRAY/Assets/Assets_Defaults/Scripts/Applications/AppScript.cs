using UnityEngine;
using TMPro;


public class AppScript : MonoBehaviour
{
    public DesktopManager desktopManager;
    public int Id;
    public TMP_Text TextIcon; 
    void Start()
    {
        desktopManager = GetComponent<DesktopManager>();
        TextIcon.text = Id.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
