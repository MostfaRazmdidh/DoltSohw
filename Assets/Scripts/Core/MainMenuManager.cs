using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioClip clickSound;
    [Range(0f, 1f)]
    public float clickVolume = 0.8f; // ولوم ساند افکت بین 0 تا 1

    public void OnStartButton()
    {
        AudioManager.Instance.PlaySound(clickSound, clickVolume);
        SceneManager.LoadScene("LevelSelect");
    }
}