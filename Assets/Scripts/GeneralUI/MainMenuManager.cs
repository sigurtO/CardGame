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
        // 1. Check for the Umbraco Name
        // (If playing via WebGL on Umbraco, you will eventually inject the name here via a JS bridge.
        // For now, we will simulate it with a simple PlayerPrefs check or a blank string).

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

    // Hook this to the Start button on the LOGGED IN panel
    public void OnLoggedInStartClicked()
    {
        // Name is already saved, just load the game!
        LoadGame();
    }

    // Hook this to the Start button on the GUEST panel
    public void OnGuestStartClicked()
    {
        // 1. Grab whatever they typed into the box
        string typedName = guestNameInput.text;

        // 2. Fallback if they left it completely blank
        if (string.IsNullOrWhiteSpace(typedName))
        {
            typedName = "Anonymous_Hero";
        }

        // 3. Lock in the name for the run analytics!
        PlayerPrefs.SetString("CurrentPlayerName", typedName);
        PlayerPrefs.Save();

        // 4. Load the game
        LoadGame();
    }

    private void LoadGame()
    {
        Debug.Log($"[MainMenu] Starting run for: {PlayerPrefs.GetString("CurrentPlayerName")}");

        // Load your Map Scene! (Make sure it's exactly the name of your scene)
        SceneManager.LoadScene("MapScene");
    }

    // --- THE UMBRACO BRIDGE (For later) ---
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
       // return "Architect_Tester"; // test logged in UI
        return "";                    // Test guest UI

#endif
    }
}