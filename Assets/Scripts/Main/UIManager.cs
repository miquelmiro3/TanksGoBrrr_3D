using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField ipField;
    public InputField usernameField;
    public Text msgText;

    public Texture2D cursor;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        Cursor.SetCursor(cursor, new Vector2(8f, 8f), CursorMode.ForceSoftware);

        if (ClientInfo.errorMsg != null)
        {
            MsgError(ClientInfo.errorMsg);
            ClientInfo.errorMsg = null;
        }
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        ipField.interactable = false;
        usernameField.interactable = false; //NEW

        /*Client.instance.ip = ipField.text; //NEW
        Debug.Log($"Connecting to {Client.instance.ip}...");

        Client.instance.ConnectToServer();*/

        //COMPROVACIONES DE IP VALIDA / USERNAME VALIDO
        //public static bool TryParse (string? ipString, out System.Net.IPAddress? address);

        ClientInfo.ip = ipField.text;
        ClientInfo.username = usernameField.text;

        Debug.Log($"Connecting to {ClientInfo.ip}...");

        SceneManager.LoadScene("Client");
    }

    public void HostAndConnectToServer()
    {
        // TO DO: host i play
        ClientInfo.ip = "127.0.0.1";
        ClientInfo.username = usernameField.text;

        Debug.Log($"Connecting to {ClientInfo.ip}...");

        SceneManager.LoadScene("Client");

        Debug.Log($"Starting host...");

        SceneManager.LoadScene("ClientHost", LoadSceneMode.Additive);
    }

    public void HostServer()
    {
        //COMPROVACIONES DE USERNAME VALIDO

        ClientInfo.ip = "127.0.0.1";
        ClientInfo.username = usernameField.text;

        Debug.Log($"Starting host...");

        SceneManager.LoadScene("ClientHost");
    }

    public void MsgError(string msg) //NEW
    {
        startMenu.SetActive(true);
        ipField.interactable = true;
        usernameField.interactable = true;
        msgText.text = msg;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
