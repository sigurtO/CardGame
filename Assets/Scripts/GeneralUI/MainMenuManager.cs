using TMPro; // Required for TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI States (Panels)")]
    [SerializeField] private GameObject loggedInPanel;
    [SerializeField] private GameObject guestPanel;

    [Header("Logged In UI Elements")]
    [SerializeField] private TextMeshProUGUI welcomeText;

    [Header("Guest UI Elements")]
    [SerializeField] private TMP_InputField guestNameInput;

    private void Start()
    {
 
        string umbracoProvidedName = GetNameFromUmbraco();

        // 2. Route to the correct UI Panel
        if (!string.IsNullOrEmpty(umbracoProvidedName))
        {
            SetupLoggedInState(umbracoProvidedName);
        }
        else
        {
            SetupGuestState();
        }
    }

    // --- STATE ROUTING ---

    private void SetupLoggedInState(string accountName)
    {
        guestPanel.SetActive(false);
        loggedInPanel.SetActive(true);

        // Update the UI
        if (welcomeText != null)
        {
            welcomeText.text = $"Hello, {accountName}!";
        }

        // Lock in the name for the run analytics!
        PlayerPrefs.SetString("CurrentPlayerName", accountName);
        PlayerPrefs.Save();
    }

    private void SetupGuestState()
    {
        loggedInPanel.SetActive(false);
        guestPanel.SetActive(true);

        // Optional: Pre-fill the input box if they played as a guest previously
        if (guestNameInput != null)
        {
            guestNameInput.text = PlayerPrefs.GetString("CurrentPlayerName", "Guest");
        }
    }

    // --- BUTTON CLICKS ---

    public void OnLoggedInStartClicked()
    {
        // Name is already saved, just load the game!
        LoadGame();
    }

    public void OnGuestStartClicked()
    {
        string typedName = guestNameInput.text;

        if (string.IsNullOrWhiteSpace(typedName)) //if blank name
        {
            typedName = "Anonymous_Hero";
        }

        PlayerPrefs.SetString("CurrentPlayerName", typedName);
        PlayerPrefs.Save();

        LoadGame();
    }

    private void LoadGame()
    {
        Debug.Log($"[MainMenu] Starting run for: {PlayerPrefs.GetString("CurrentPlayerName")}");

        SceneManager.LoadScene("MapScene");
    }

    [DllImport("__Internal")]
    private static extern string GetUmbracoPlayerNameJS();

    private string GetNameFromUmbraco()
    {

#if UNITY_WEBGL && !UNITY_EDITOR //make sure we arent in the editor
            
            // If we are actually built and running on the website, ask JavaScript!
            Debug.Log("[Umbraco Bridge] Reaching out to browser for player name...");
            return GetUmbracoPlayerNameJS();
            
#else

        // if in the Unity Editor simulate a fake login
        Debug.Log("[Umbraco Bridge] Editor Mode detected. Skipping JS bridge.");
        return "";                    // Test guest UI

#endif
    }
}