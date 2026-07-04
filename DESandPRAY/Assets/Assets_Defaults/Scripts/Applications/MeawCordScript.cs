using System.Collections.Generic;
using UnityEngine;

public class MeawCordScript : MonoBehaviour
{
    [Header("Chat Panels Settings")]
    // Masukkan GameObject Chat dari Hierarchy ke array ini.
    // Urutannya WAJIB sama dengan ID di ProfileId!
    // Contoh: Element 0 = AlviciaRoxiesChat, Element 1 = AlanDezainChat, dst.
    public GameObject[] chatPanels;
    public List<int> acceptedId;
    private InputManager inputManager;
    public ExplorerScript explorerScript;
    public ArtInspectorScript artInspectorScript;
    void Awake()
    {
        acceptedId = new List<int>();
        chatPanels[0].SetActive(true);
        // Pastikan semua chat panel di-assign di Inspector
        if (chatPanels.Length == 0)
        {
            Debug.LogWarning("Chat panels belum di-assign di Inspector!");
        }
    }

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();

        // Saat Meawcord pertama kali dibuka, sembunyikan semua chat panel
        // (Atau kamu bisa set chatPanels[0] jadi true jika ingin ada chat default yang terbuka)
        // chatPanels[0].SetActive(true);
        
        foreach (GameObject chat in chatPanels)
        {
            if (chat != null) chat.SetActive(false);
        }
    }

    void Update()
    {
        if(inputManager.newClick)
        {
            GameObject clickedObj = inputManager.clickedObj;

            // Buka isi Chat berdasarkan Profil yang diklik
            if(clickedObj.CompareTag("DiscordProfile"))
            {
                int clickedId = clickedObj.GetComponentInParent<ProfileId>().Id;
                Debug.Log($"Membuka isi chat untuk ID: {clickedId}");
                
                OpenChat(clickedId);
            }

            if (clickedObj.CompareTag("DownloadCV"))
            {
                int clickedId = clickedObj.GetComponentInParent<ProfileId>().Id;
                explorerScript.AddFile(clickedId);
                artInspectorScript.AddFile(clickedId);
            }

            
            if (clickedObj.CompareTag("MeawcordReject"))
            {
                int clickedId = clickedObj.GetComponentInParent<ProfileId>().Id;
            }
            
            if (clickedObj.CompareTag("MeawcordAccept") && acceptedId.Count < 2)
            {
                int clickedId = clickedObj.GetComponentInParent<ProfileId>().Id;
                if (ValidateId(clickedId))
                {
                    clickedObj.SetActive(false);
                    acceptedId.Add(clickedId);
                }
            }
        }
    }
    
    public void OpenChat(int chatId)
    {
        // Loop semua chat panel: yang ID-nya sama di-True, yang lain di-False
        for(int i = 0; i < chatPanels.Length; i++)
        {
            if (chatPanels[i] != null)
            {
                if (i == chatId)
                {
                    chatPanels[i].SetActive(true); // Tampilkan chat yang dipilih
                }
                else
                {
                    chatPanels[i].SetActive(false); // Sembunyikan chat yang lain
                }
            }
        }
    }

    private bool ValidateId(int id)
    {
        foreach (int _id in acceptedId)
        {
            if(id == _id)return false;
        }
        return true;
    }
}