using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class TestRelay : MonoBehaviour {

    public static TestRelay instance; //Singleton

    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private TextMeshProUGUI codeTextVRScene;

    [SerializeField] private int sceneIndex = 1;

    private string joinCode = null;

    public string JoinCode => joinCode;  //property

    private bool isHost;


    private void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    private void OnEnable()
    {
        if (createButton != null)
        {
            createButton.onClick.AddListener(CreateRelay);
        }
        if (joinButton != null)
        {
            joinButton.onClick.AddListener(JoinRelay);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void JoinGameClient()
    {

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single).completed += (operation) =>     //O c�digo abaixo s� vai executar quando a scene loadar toda
        {
            NetworkManager.Singleton.StartClient(); // Em vez de clicar no bot�o client, chmar� por aqui
        };
    }
    public void StartGameHost()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single).completed += (operation) =>     //O c�digo abaixo s� vai executar quando a scene loadar toda //IMPORTANTE//
        {
            NetworkManager.Singleton.StartHost(); // Em vez de clicar no bot�o host, chmar� por aqui

        };

        
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) //ouvindo o evento de quando terminar de load uma cena
    {
        Debug.Log("CHEGOU AQUI 1111");
        if (scene.buildIndex == 1)
        {
            Debug.Log("CHEGOU AQUI 2222");
            if (isHost)
            {
                NetworkManager.Singleton.StartHost();
            }
            else
            {
                Debug.Log("CHEGOU AQUI 3333");
                NetworkManager.Singleton.StartClient();
            }
        }
    }


    // Inicializando unity services
    // � async, pois manda um request par o unity services inicializar a api
    // e se n fosse asyn/await o jogo travaria at� o jogo receber a resposta
    private async void  Start()                             
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {             // Inscrevendo no evento SignedIn para saber quando o player logar�
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync(); // logando anonimamente// poderia ser com varias contas


    }

    private async void CreateRelay()
    {
        try
        { // Toda fun��o de Relay tem exce��o, tem q tratar para n travar o jogo
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3); //mas number of connectiso, except host


            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Join Code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls"); // udp, dtls. dtls � criptografado

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); // bem mais simples que o antigo. relayServerData ter� todas as informa��es necess�rias j�

            codeText.text = joinCode;

            isHost = true;

            LoadScene(sceneIndex);
            //StartGameHost();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }

    private static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public async void JoinRelay() // Joinando o server com o joinCode gerado
    {
        try
        {
            if (inputField != null)
            {
                joinCode = inputField.text;
                Debug.Log("Join Relay with " + joinCode);
            }

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);


            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls"); // udp, dtls. dtls � criptografado

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); // bem mais simples que o antigo. relayServerData ter� todas as informa��es necess�rias j�

            isHost = false;

            LoadScene(sceneIndex);
            //JoinGameClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
