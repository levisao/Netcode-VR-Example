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

public class TestRelay : NetworkBehaviour {

    public static TestRelay instance; //Singleton

    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private TextMeshProUGUI codeTextVRScene;

    [SerializeField] private string sceneName = "World_School";

    private string joinCode = null;

    public string JoinCode => joinCode;  //property

    private bool isHost;

    private bool gameStarted = false;

    public bool GameStarted => gameStarted;

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

        SceneManager.sceneLoaded += OnSceneLoaded;  //subscribing to event that tells when a scene finished loading

    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) //ouvindo o evento de quando terminar de load uma cena ANTIGO JEITO
    {
        if (scene.name == sceneName)
        {
            gameStarted = true;
            if (isHost)
            {
                NetworkManager.Singleton.StartHost();
            }
            else
            {
                NetworkManager.Singleton.StartClient();
            }
        }
    }

    public override void OnNetworkSpawn()
    {

      /*  var status = NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single); // irá loadar a cena assim que a network spawn
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load {sceneName} " +
                  $"with a {nameof(SceneEventProgressStatus)}: {status}");
        }
      */
    }


    // Inicializando unity services
    // É async, pois manda um request par o unity services inicializar a api
    // e se n fosse asyn/await o jogo travaria até o jogo receber a resposta
    private async void  Start()                             
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {             // Inscrevendo no evento SignedIn para saber quando o player logará
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync(); // logando anonimamente// poderia ser com varias contas


    }

    private async void CreateRelay()
    {
        try
        { // Toda função de Relay tem exceção, tem q tratar para n travar o jogo
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3); //mas number of connectiso, except host


            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Join Code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls"); // udp, dtls. dtls é criptografado

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); // bem mais simples que o antigo. relayServerData terá todas as informações necessárias já

            codeText.text = joinCode;

            isHost = true;

        LoadScene(sceneName);
        //NetworkManager.Singleton.StartHost(); 
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }

    private static void LoadScene(string scene)
    {
        //Debug.Log("Scene name: " + scene);
        //NetworkManager.Singleton.SceneManager.LoadScene(scene, LoadSceneMode.Single);
        SceneManager.LoadScene(scene);
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


            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls"); // udp, dtls. dtls é criptografado

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); // bem mais simples que o antigo. relayServerData terá todas as informações necessárias já

            isHost = false;

            LoadScene(sceneName);
            //NetworkManager.Singleton.StartClient();
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
