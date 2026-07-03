using System.Collections.Generic;
using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    public GameObject[] apps;
    public List<GameObject> windows;
    public GameObject window;
    public InputManager inputManager;

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();

        apps = GameObject.FindGameObjectsWithTag("App");
        windows = new List<GameObject>();
        for(int i = 0; i < apps.Length; i++)
        {
            Debug.Log($"Apps Found: {apps[i]}");
        }
    }

    void Update()
    {
        if(inputManager.newClick)
        {
            // Debug.Log($"Clicked Obj: {inputManager.clickedObj.name}");
            GameObject clickedObj = inputManager.clickedObj;

            if(clickedObj.tag == "App")
            {
                int clickedId = (clickedObj.GetComponentInParent<AppScript>()).Id;
                NewWindow(clickedId);
            }

            if(clickedObj.tag == "DragWindow")
            {
                int clickedWindowId = (clickedObj.GetComponentInParent<WindowScript>()).Id;
                Debug.Log($"Drag window id: {clickedWindowId}");
                
            }

            if(clickedObj.tag == "CloseWindow")
            {
                int clickedWindowId = (clickedObj.GetComponentInParent<WindowScript>()).Id;
                Debug.Log($"Clossed App id: {clickedWindowId}");
                killWindow(clickedWindowId);
            }
        }
    }

    public void OpenWindow(int AppId)
    {
        for(int i = 0; i < windows.Count; i++)
        {
            if(((windows[i]).GetComponent<WindowScript>()).Id == AppId){
                windows[i].SetActive(true);
            } else
            {
                windows[i].SetActive(false);
            }
        }
    }

    public void MinimizeWindow()
    {

    }

    public void NewWindow(int AppId)
    {
        Debug.Log($"Clicked app: {AppId}");

        if(CheckOpenedWindow(AppId) == true)
        {
            OpenWindow(AppId);
            return;
        }
        Debug.Log($"Window total: {windows.Count}");
        Vector3 spawnPosition = new Vector3(0.5f, -0.5f, 0f);
        GameObject newWindow = Instantiate(window, spawnPosition, Quaternion.identity);

        WindowScript windowScript = newWindow.GetComponent<WindowScript>();
        windowScript.Id = AppId;

        windows.Add(newWindow);
        OpenWindow(AppId);
    }

    public void killWindow(int AppId)
    {
        for(int i = 0; i < windows.Count; i++)
        {
            if(((windows[i]).GetComponent<WindowScript>()).Id == AppId){
                Destroy(windows[i]);
                windows.Remove(windows[i]);
                return;
            }
        }
    }

    private bool CheckOpenedWindow(int AppId)
    {
        for(int i = 0; i < windows.Count; i++)
        {
            if(((windows[i]).GetComponent<WindowScript>()).Id == AppId)
            {
                return true;
            }
        }
        return false;
    }

    public void Shutdown()
    {
        
    }
}
