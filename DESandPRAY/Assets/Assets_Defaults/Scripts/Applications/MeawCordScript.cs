using UnityEngine;

public class MeawCordScript : MonoBehaviour
{
    [Header("Chat Panels Settings")]
    // Masukkan GameObject Chat dari Hierarchy ke array ini.
    // Urutannya WAJIB sama dengan ID di ProfileId!
    // Contoh: Element 0 = AlviciaRoxiesChat, Element 1 = AlanDezainChat, dst.
    public GameObject[] chatPanels;

    private InputManager inputManager;
    void Awake()
    {
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
        }
    }

    // Fungsi ini bekerja dengan sistem yang sama persis dengan OpenWindow
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
}