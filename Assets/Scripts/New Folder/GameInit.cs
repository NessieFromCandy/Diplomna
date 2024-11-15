using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInit : MonoBehaviour
{
    public TMPro.TMP_InputField usernameInput;

    private void Start()
    {
        if (PlayerPrefs.HasKey("username")) usernameInput.SetTextWithoutNotify(PlayerPrefs.GetString("username"));
    }

    public void PlayClicked()
    {
        if (string.IsNullOrWhiteSpace(usernameInput.text)) return;

        PlayerPrefs.SetString("username", usernameInput.text);
        SceneManager.LoadScene(1);
    }
}
