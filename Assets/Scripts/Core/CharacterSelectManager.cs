using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class CharacterOption
    {
        public string characterId;   
        public Button characterButton;
    }

    [Header("کاراکترها")]
    public CharacterOption[] characters;

    [Header("دکمه‌های تایید (دو حالت)")]
    public GameObject antecabNo;   
    public Button antecab;         

    [Header("فلش نشانگر کاراکتر انتخابی")]
    public RectTransform craecter;
    public float craecterYOffset = 40f; 

    [Header("صدا")]
    public AudioClip selectSound;
    public AudioClip confirmSound;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    [Header("صحنه بعدی")]
    public string nextScene = "Level1_m1_R2";

    private const string SaveKey = "SelectedCharacterId";

   
    public static string SelectedCharacterId { get; private set; }

    private int selectedIndex = -1;

    void Start()
    {
        
        antecabNo.SetActive(true);
        antecab.gameObject.SetActive(false);
        craecter.gameObject.SetActive(false);

        for (int i = 0; i < characters.Length; i++)
        {
            int index = i; 
            characters[i].characterButton.onClick.AddListener(() => OnCharacterClicked(index));
        }

        antecab.onClick.AddListener(OnConfirmClicked);
    }

    private void OnCharacterClicked(int index)
    {
        if (selectSound != null)
            AudioManager.Instance.PlaySound(selectSound, sfxVolume);

        selectedIndex = index;

        
        RectTransform charRect = characters[index].characterButton.GetComponent<RectTransform>();
        craecter.gameObject.SetActive(true);
        craecter.position = new Vector3(charRect.position.x, charRect.position.y + charRect.rect.height / 2f + craecterYOffset, charRect.position.z);

        
        antecabNo.SetActive(false);
        antecab.gameObject.SetActive(true);

        
        SelectedCharacterId = characters[selectedIndex].characterId;
        PlayerPrefs.SetString(SaveKey, SelectedCharacterId);
        PlayerPrefs.Save();
    }

    private void OnConfirmClicked()
    {
        if (selectedIndex < 0) return;

        if (confirmSound != null)
            AudioManager.Instance.PlaySound(confirmSound, sfxVolume);

        SceneManager.LoadScene(nextScene);
    }
}