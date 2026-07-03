using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
                int clickedId = (clickedObj.GetComponent<AppScript>()).Id;
                Debug.Log($"App id: {clickedId}");
                for(int i = 0; i < windows.Count; i++)
                {
                    if(((windows[i]).GetComponent<WindowScript>()).Id == clickedId)
                    {
                        DirectToWindow(clickedId);
                        return;
                    }
                }
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

    public void DirectToWindow(int AppId)
    {
        
    }

    public void NewWindow(int AppId)
    {
        int n = windows.Count;
        int order = n * 3;
        Debug.Log($"Window total: {windows.Count}");
        Vector3 spawnPosition = new Vector3(0.5f*n, -0.5f*n, 0f);
        GameObject newWindow = Instantiate(window, spawnPosition, Quaternion.identity);

        Transform[] newWindowChildrens = newWindow.GetComponentsInChildren<Transform>();
        for(int i = 0; i < newWindowChildrens.Length; i++)
        {
            Debug.Log($"{i+1}. Child: {newWindowChildrens[i].gameObject.name}");
            Vector3 childPos = newWindowChildrens[i].position;
            newWindowChildrens[i].position = new Vector3(childPos.x, childPos.y, childPos.z - (1 + order));
        }

        // (newWindow.GetComponent<SpriteRenderer>()).sortingOrder = n;

        WindowScript windowScript = newWindow.GetComponent<WindowScript>();
        windowScript.Id = AppId;

        windows.Insert(0, newWindow);
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

//  demi kristus apaan ini anjg
    private void updateWindowLayer()
    {
        
    }
}
