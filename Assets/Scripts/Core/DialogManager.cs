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

    [Header("تنظیمات تایپ")]
    public float typingSpeed = 0.05f;
    public AudioClip typingSound;
    [Range(0f, 1f)]
    public float typingVolume = 0.5f;

    [Header("صدای اخبار")]
    public AudioClip newsAmbience;
    [Range(0f, 1f)]
    public float newsVolume = 0.2f; // کمتر از تایپ

    [Header("انیمیشن خانم مجری")]
    public GameObject[] mouthObjects; // 3 تا GameObject از Hierarchy
    public float mouthSwitchSpeed = 0.15f;

    [Header("صحنه بعدی")]
    public string nextScene = "Level1_m2";
    public float clickDelay = 5f;

    private bool canClick = false;
    private AudioSource typingAudioSource;
    private AudioSource newsAudioSource;
    private Coroutine mouthCoroutine;

    void Start()
    {
        typingAudioSource = gameObject.AddComponent<AudioSource>();
        newsAudioSource = gameObject.AddComponent<AudioSource>();

        dialogBox.SetActive(false);

        if (AudioManager.Instance != null)
            AudioManager.Instance.StopMusic();

        // شروع صدای اخبار به صورت لوپ
        if (newsAmbience != null)
        {
            newsAudioSource.clip = newsAmbience;
            newsAudioSource.loop = true;
            newsAudioSource.volume = newsVolume;
            newsAudioSource.Play();
        }

        dialogBox.SetActive(true);
        StartCoroutine(TypeText());
        mouthCoroutine = StartCoroutine(AnimateMouth());

        StartCoroutine(EnableClickAfterDelay());
    }

    void Update()
    {
        if (canClick && Mouse.current.leftButton.wasPressedThisFrame)
        {
            StopAllAudio();
            SceneManager.LoadScene(nextScene);
        }
    }

    private IEnumerator TypeText()
    {
        dialogText.text = "";

        if (typingSound != null)
            typingAudioSource.PlayOneShot(typingSound, typingVolume);

        for (int i = 1; i <= dialog.Length; i++)
        {
            dialogText.text = dialog.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private IEnumerator AnimateMouth()
    {
        if (mouthObjects == null || mouthObjects.Length == 0) yield break;

        int index = 0;

        
        foreach (var obj in mouthObjects)
            obj.SetActive(false);

        while (true)
        {
            
            for (int i = 0; i < mouthObjects.Length; i++)
                mouthObjects[i].SetActive(i == index);

            index = (index + 1) % mouthObjects.Length;
            yield return new WaitForSeconds(mouthSwitchSpeed);
        }
    }

    private IEnumerator EnableClickAfterDelay()
    {
        yield return new WaitForSeconds(clickDelay);
        canClick = true;
    }

    private void StopAllAudio()
    {
        if (mouthCoroutine != null)
            StopCoroutine(mouthCoroutine);

        typingAudioSource.Stop();
        newsAudioSource.Stop();
    }
}