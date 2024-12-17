using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    private string serverURL = "<API ENDPOINT>";
    private string bearerToken = "<TOKEN>";

    // Function to send POST request to server
    public IEnumerator SendInventoryStatusRequest(int itemId, string eventType)
    {
        // Prepare data to send in the request
        var jsonData = new
        {
            item_id = itemId,
            @event = eventType
        };

        // Convert the data into JSON format
        string json = JsonUtility.ToJson(jsonData);

        // Create the request
        UnityWebRequest request = UnityWebRequest.PostWwwForm(serverURL, json);

        // Set the request headers (Authorization and Content-Type)
        request.SetRequestHeader("Authorization", "Bearer " + bearerToken);
        request.SetRequestHeader("Content-Type", "application/json");

        // Set the body of the POST request
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // Wait for the server response
        yield return request.SendWebRequest();

        // Check if the request was successful
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request sent successfully! Response: " + request.downloadHandler.text);

            // You can parse the response if necessary
            var response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);
            Debug.Log("Data submitted: " + response.data_submitted);
        }
        else
        {
            Debug.LogError("Error sending request: " + request.error);
        }
    }

    // Server response structure
    [System.Serializable]
    public class ServerResponse
    {
        public string response;
        public string status;
        public string data_submitted;
    }
}
