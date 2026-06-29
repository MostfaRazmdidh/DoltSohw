using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using RTLTMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogBox;
    public RTLTextMeshPro dialogText;

    [Header("متن دیالوگ")]
    [TextArea(3, 6)]
    public string dialog = "با نزدیک شدن انتخابات، نامزدهای تایید شده آماده می‌شوند...";

    [Header("تنظیمات")]
    public float typingSpeed = 0.05f;
    public AudioClip typingSound;
    [Range(0f, 1f)]
    public float typingVolume = 0.5f;

    [Header("صحنه بعدی")]
    public string nextScene = "Level1_m2";

    private bool dialogShown = false;
    private bool typingDone = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        dialogBox.SetActive(false);

        // موزیک اصلی رو خاموش کن
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopMusic();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!dialogShown)
            {
                // کلیک اول - نشون بده دیالوگ رو
                dialogShown = true;
                dialogBox.SetActive(true);
                StartCoroutine(TypeText());
            }
            else if (typingDone)
            {
                // کلیک دوم - برو صحنه بعدی
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    private IEnumerator TypeText()
    {
        dialogText.text = "";

        // یک بار صدا پخش میشه
        if (typingSound != null)
            audioSource.PlayOneShot(typingSound, typingVolume);

        foreach (char c in dialog)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingDone = true;
    }
}