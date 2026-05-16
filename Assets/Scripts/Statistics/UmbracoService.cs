using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UmbracoService : MonoBehaviour
{
    [Header("API Settings")]
    [SerializeField] private string umbracoApiUrl = "https://your-umbraco-site.com/api/leaderboard";

    public void SendRunData(string jsonPayload, System.Action onComplete)
    {
        StartCoroutine(PostDataCoroutine(jsonPayload, onComplete));
    }

    private IEnumerator PostDataCoroutine(string jsonPayload, System.Action onComplete)
    {

        UnityWebRequest request = new UnityWebRequest(umbracoApiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"[UmbracoService] Network Error: {request.error}");
        }
        else
        {
            Debug.Log($"[UmbracoService] Success! Server responded: {request.downloadHandler.text}");
        }

        request.Dispose();

        // Tell whoever called us that we are finished, regardless of success/failure
        onComplete?.Invoke();
    }
}