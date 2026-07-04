using UnityEngine;

public class MeawCordScript : MonoBehaviour
{
    private int selectedProfile;
    private InputManager inputManager;


    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();
    }

    void Update()
    {
        if(inputManager.newClick)
        {
            GameObject clickedObj = inputManager.clickedObj;

            // Buka Aplikasi
            if(clickedObj.CompareTag("DiscordProfile"))
            {
                int clickedId = clickedObj.GetComponentInParent<ProfileId>().Id;
                Debug.Log($"Clicked dc Id: {clickedId}");
            }

        }
    }
}
