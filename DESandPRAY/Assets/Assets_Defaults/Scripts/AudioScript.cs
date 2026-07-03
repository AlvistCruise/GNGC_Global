// using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI; // Wajib untuk mengakses komponen Slider

public class AudioScript : MonoBehaviour
{
    [Header("Pengaturan BGM")]
    public Slider bgmSlider;
    public AudioSource bgmSource; // Cuma 1 sumber suara

    [Header("Pengaturan SFX")]
    public Slider sfxSlider;
    public AudioSource[] sfxSources; // Menggunakan Array [] karena ada lebih dari 1 (ada 3)

    void Start()
    {
        // 1. Sinkronkan posisi Slider BGM dengan volume awal saat game dimulai
        if (bgmSource != null && bgmSlider != null)
        {
            bgmSlider.value = bgmSource.volume = 0.2f;

            // Menyambungkan Slider BGM ke fungsi pengubah volume
            bgmSlider.onValueChanged.AddListener(UbahVolumeBGM);
        }

        // 2. Sinkronkan posisi Slider SFX dengan volume awal (ambil patokan dari SFX pertama)
        if (sfxSources.Length > 0 && sfxSlider != null)
        { 
            sfxSlider.value = sfxSources[0].volume = 0.7f;

            // Menyambungkan Slider SFX ke fungsi pengubah volume
            sfxSlider.onValueChanged.AddListener(UbahVolumeSFX);
        }
    }

    // Fungsi ini dipanggil otomatis saat Slider BGM digeser
    private void UbahVolumeBGM(float nilaiVolume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = nilaiVolume;
        }
    }

    // Fungsi ini dipanggil otomatis saat Slider SFX digeser
    private void UbahVolumeSFX(float nilaiVolume)
    {
        // Karena ada 3 SFX, kita gunakan "for loop" untuk mengubah volume ketiganya sekaligus
        for (int i = 0; i < sfxSources.Length; i++)
        {
            if (sfxSources[i] != null)
            {
                sfxSources[i].volume = nilaiVolume;
            }
        }
    }
}
