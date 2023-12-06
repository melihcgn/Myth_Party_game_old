using System.Collections;
using UnityEngine;  
using System.Collections.Generic;

using playerNS;
using TMPro;
using UnityEngine.UI;
using System.Linq;


public class createPanels : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject playerPanelPrefab;
    public GameObject transferPanelPrefab;
    public Vector3 spawnPosition;
    public Transform canvas;
    public GameObject scriptAl;
    public GameObject openingpanel;
    private TextMeshProUGUI pNameText1, pNameText2, tNameText1, pRoleText1, pRoleText2, explanationText;
    public List<GameObject> panels = new List<GameObject>();
    public List<GameObject> playerPanels = new List<GameObject>();
    public GameObject votingPanelTo;
    public GameObject infoPanel ;
    public   GameObject morningPanel; 
    public GameObject circleButtonPrefab;

    // for clickable ptofile buttons
    public Color selectedColor = Color.yellow;  // Set selectedColor to yellow
    public Color defaultColor = Color.gray;

    private List<Button> buttons = new List<Button>();
    public List<Players> ps = new List<Players>();
    
    public Button selectedButton;
    public Button votingButton;
    private ActualPlay Aplay;
    public void Start()
    {
        GameObject gameManagerObject = GameObject.Find("scriptMoves2");

        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();

            if (gameManager != null)    
            {
                List<Players> players = gameManager.playerSit;
                ps = players;

                foreach (Players player in ps)
                {
                    Debug.Log("Player name: " + player.pname);
                }
            }
            else
            {
                Debug.LogError("GameManager component not found.");
            }
        }
        else
        {
            Debug.LogError("GameManager object not found.");
        }
    }



    public void SetupPlayerPanels(List<Players> playersList)
    {
        if (playerPanelPrefab == null)
        {
            Debug.LogError("Player panel prefab is null.");
            return;
        }


        foreach (var player in playersList)

        {
            if (playerPanelPrefab != null)
            {
                GameObject transferPlane = Instantiate(transferPanelPrefab, spawnPosition, Quaternion.identity, canvas);

                GameObject playerPlane = Instantiate(playerPanelPrefab, spawnPosition, Quaternion.identity, canvas);
                Transform names = playerPlane.transform.Find("Names");

                // Calculate the width of a single button
                float buttonWidth = circleButtonPrefab.GetComponent<RectTransform>().rect.width;
                // Set the maximum buttons per row
                int maxButtonsPerRow = 3;

                // Calculate the spacing between buttons to fit them in a row
                float spacing = buttonWidth * maxButtonsPerRow / 3 * 2;
                int pCount = playersList.Count;
                
                
                int dummy = 0 , deadCount = 0;
                if (player.role == "UMAY")
                {
                    List<Players> deadForUmays = new List<Players>();
                    if ( Aplay != null && Aplay.deadPlayers == null)
                    {
                        deadForUmays = Aplay.deadPlayers;
                    }
                    else{
                        deadCount = deadForUmays.Count;
                    }                    
                    
                    for (int k = 0; k < deadCount; k++)
                    {
                        // Calculate row and column indices for the button
                        int row = (k + dummy) / maxButtonsPerRow;
                        int col = (k + dummy) % maxButtonsPerRow;

                        // Calculate the position with appropriate spacing
                        Vector3 buttonPosition = new Vector3(
                            names.position.x + (col) * buttonWidth * 2 - spacing,
                            names.position.y - (row - 1) * buttonWidth * 2,
                            names.position.z);

                        // Instantiate the button at the calculated position
                        GameObject circleButtonInstance = Instantiate(circleButtonPrefab, buttonPosition, Quaternion.identity, names);
                        TextMeshProUGUI tmpText = circleButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
                        tmpText.text = deadForUmays[k].pname;
                        Button circleButton = circleButtonInstance.GetComponent<Button>();
                        circleButton.name = deadForUmays[k].pname + " button";
                        circleButton.onClick.AddListener(() => OnButtonClick(ref circleButton, deadForUmays));
                        //making clickable
                    }
                }
                else
                {
                    for (int i = 0; i < pCount; i++)
                    {
                        if (player.pname != playersList[i].pname)
                        {
                            // Calculate row and column indices for the button
                            int row = (i + dummy) / maxButtonsPerRow;
                            int col = (i + dummy) % maxButtonsPerRow;

                            // Calculate the position with appropriate spacing
                            Vector3 buttonPosition = new Vector3(
                                names.position.x + (col) * buttonWidth * 2 - spacing,
                                names.position.y - (row - 1) * buttonWidth * 2,
                                names.position.z);

                            // Instantiate the button at the calculated position
                            GameObject circleButtonInstance = Instantiate(circleButtonPrefab, buttonPosition, Quaternion.identity, names);
                            TextMeshProUGUI tmpText = circleButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
                            tmpText.text = playersList[i].pname;
                            Button circleButton = circleButtonInstance.GetComponent<Button>();
                            circleButton.onClick.AddListener(() => OnButtonClick(ref circleButton, playersList));
                            //making clickable
                        }
                        else
                        {
                            dummy--;
                        }

                    }
                }





                string playerName = player.pname;
                playerPlane.name = playerName;
                string roleName = player.role;
                playerPlane.SetActive(true);
                transferPlane.name = playerName + " transition";
                transferPlane.SetActive(true);
                playerPanels.Add(transferPlane);
                playerPanels.Add(playerPlane);
                panels.Add(transferPlane);
                panels.Add(playerPlane);
                transferPlane.SetActive(false);
                playerPlane.SetActive(false);
                Debug.Log("gorko2 " + panels.Count);


                if (playerPlane != null)
                {
                    pNameText1 = playerPlane.transform.Find("nameText").GetComponent<TextMeshProUGUI>();
                    pRoleText2 = playerPlane.transform.Find("roleText").GetComponent<TextMeshProUGUI>();
                    explanationText = playerPlane.transform.Find("roleText").Find("roleInfo").GetComponent<TextMeshProUGUI>();
                    Button passButton = playerPlane.transform.Find("passButton").GetComponent<Button>();
                    Button pickButton = playerPlane.transform.Find("pickButton").GetComponent<Button>();
                    panelManager pManager = GetComponent<panelManager>();
                    passButton.onClick.AddListener(pManager.passingPages);
                    pickButton.onClick.AddListener(() => changeStatus(player.role));
                    pickButton.onClick.AddListener(pManager.passingPages);
                    pNameText1.text = "Name: " + player.pname;
                    pRoleText2.text = "Role: " + player.role;
                    if (player.pname == playersList[playersList.Count - 1].pname)
                    {
                        ActualPlay Aplay = GetComponent<ActualPlay>();
                        if (Aplay != null)
                        {
                            pickButton.onClick.AddListener(Aplay.afterPlayerAction);
                            passButton.onClick.AddListener(Aplay.afterPlayerAction);
                        }
                        else
                        {
                            Debug.Log("WOKEGEEE!");
                        }

                    }
                    else
                    {
                        Debug.Log(player.pname + " daha olmadı " + playersList[playersList.Count - 1].pname);
                    }
                    tNameText1 = transferPlane.transform.Find("nameText").GetComponent<TextMeshProUGUI>();
                    tNameText1.text = player.pname;
                    Button seeButton = transferPlane.transform.Find("seeButton").GetComponent<Button>();
                    seeButton.onClick.AddListener(pManager.passingPages);
                    TextMeshProUGUI buttonText = pickButton.GetComponentInChildren<TextMeshProUGUI>();

                    Button inactiveButton = playerPlane.transform.Find("extraButton").GetComponent<Button>();
                    inactiveButton.interactable = false;

                    // specifying the interface of specific roles, exmple: adding a button 
                    if (roleName == "MERGEN")
                    {
                        explanationText.text = "Ok attığı kişinin rolü açığa çıkar.";
                        buttonText.text = "Reveal";
                        //MERGEN - Ok attığı kişinin rolü açığa çıkar.
                    }
                    else if (roleName == "Ulgen")
                    {
                        inactiveButton.interactable = true;

                        TextMeshProUGUI extraButtonText = inactiveButton.GetComponentInChildren<TextMeshProUGUI>();
                        extraButtonText.text = "Bless";
                        buttonText.text = "Strike";
                        explanationText.text = "Yıldırımını kullanarak başka oyuncuları öldürebilir ya da kutsayabilir.";
                        inactiveButton.onClick.AddListener(() => changeStatusDifferent(player.role, extraButtonText.text));
                        inactiveButton.onClick.AddListener(pManager.passingPages);
                    }
                    else if (roleName == "KIZAGAN")
                    {
                        explanationText.text = "Savaş Tanrısı Kızagan, halkı için seçtiğikişiyi mızarığyla öldürür.";
                        buttonText.text = "Shot";
                    }
                    else if (roleName == "UMAY")
                    {
                        explanationText.text = "4 tur boyunca birini diriltebilir, eğer diriltmezse kara umaya dönüşür ve birini öldürme niteliği kazanır.";
                        buttonText.text = "Retrieve";
                    }
                    else if (roleName == "BURKUT")
                    {
                        explanationText.text = "Bürküt, kanatlarıyla seçtiği kişinin evini koruma altına alır.";
                        buttonText.text = "Guard";
                    }
                    else if (roleName == "OD ANA")
                    {

                    }
                    else if (roleName == "AKBUGA")
                    {
                        explanationText.text = "Seçtiği kişinin bir geceliğine ölmemesini sağlar.";
                        buttonText.text = "Protect";
                    }
                    else if (roleName == "AI TOYON")
                    {
                        explanationText.text = "Ai Toyon, halkının lideridir ve istediği zaman kendini gösterip halkına önderlik eder(oyu 3 sayılır).";
                        buttonText.text = "Reveal";
                    }
                    else if (roleName == "AYZIT")
                    {
                        explanationText.text = "Güzelliğiyle seçtiği kişinin aksiyon yapmasını engeller.";

                        buttonText.text = "Charm";
                    }
                    else if (roleName == "ALAZ HAN")
                    {
                        explanationText.text = "Alaz Han, bir ateş yakarak o gece ateşin öfkesini evine gelenlere karşı püskürtür.";

                        buttonText.text = "Light the fire";
                    }
                    else if (roleName == "SIGUN GEYIK")
                    {
                        explanationText.text = "Çabukluğu, sinsiliği ve mükemmel gözlemciliğiyle seçtiği kiinin evini gözlemler.";

                        buttonText.text = "Lookout";
                    }
                    else if (roleName == "YILDIZ HAN")
                    {
                        explanationText.text = "Zekasıyla araştırdıkları kişilerin ruhunu görebilirler.";

                        buttonText.text = "Investigate";
                    }
                    else if (roleName == "TEPEGOZ")
                    {
                        explanationText.text = "Her gün birini yiyen aç bir canavardır, sadece gözünden öldürülebilir.";

                        buttonText.text = "Eat";
                    }
                    else if (roleName == "DEMIRKIYNAK")
                    {
                        explanationText.text = "Seçtiği kişiyi delirtir ve bir gün boyunca hiçbir eylemde bulunamamasına neden olur.";
                        buttonText.text = "Madden";
                    }
                    else if (roleName == "SU IYESI")
                    {
                        inactiveButton.interactable = true;
                        TextMeshProUGUI extraButtonText = inactiveButton.GetComponentInChildren<TextMeshProUGUI>();
                        extraButtonText.text = "Mark";
                        buttonText.text = "Flood";
                        inactiveButton.onClick.AddListener(() => changeStatusDifferent(player.role, extraButtonText.text));
                        inactiveButton.onClick.AddListener(pManager.passingPages);
                    }
                    else if (roleName == "KORMOS")
                    {
                        explanationText.text = "Unknown for now";
                        buttonText.text = "Kormos special";
                    }
                    else if (roleName == "ARCURA")
                    {
                        explanationText.text = "Unknown for now";
                        buttonText.text = "Control";
                    }
                    else if (roleName == "AZMIC")
                    {
                        explanationText.text = "Her gece birinin kontrolünü alır ve onun skillini istediği gibi kullanır sonra da kontrol ettiği kişiyi öldürür.";

                        buttonText.text = "Control2";
                    }
                    else if (roleName == "GULYABANI")
                    {
                        inactiveButton.interactable = true;
                        TextMeshProUGUI extraButtonText = inactiveButton.GetComponentInChildren<TextMeshProUGUI>();
                        extraButtonText.text = "Howl";
                        buttonText.text = "Kill";
                        inactiveButton.onClick.AddListener(() => changeStatusDifferent(player.role, extraButtonText.text));
                        inactiveButton.onClick.AddListener(pManager.passingPages);

                    }
                    else if (roleName == "KORTIGES")
                    {
                        explanationText.text = "Hastalık Cinidir. İnsanlara hastalık verirler. Türlü çeşitli hastalıklara neden olabilirler. Yaygın olarak insanların el ve ayaklarını tutmaz hale getirirler.";

                        buttonText.text = "Deactivate";
                    }
                    else
                    {
                        Debug.Log("OLMADIA Q ");
                    }

                    TextMeshProUGUI[] textComponents = playerPlane.GetComponentsInChildren<TextMeshProUGUI>();

                    foreach (TextMeshProUGUI textComponent in textComponents)
                    {
                        if (textComponent.gameObject.name == "nameText")
                        {
                            // Accessing the "nameText" TMP Text's text property for debugging
                            Debug.Log("TMP Text (nameText) value: " + textComponent.text);
                        }
                        else if (textComponent.gameObject.name == "roleName")
                        {
                            // Accessing the "roleName" TMP Text's text property for debugging
                            Debug.Log("TMP Text (roleName) value: " + textComponent.text);
                        }
                    }

                }
                else
                {
                    Debug.LogError("Player panel instantiation failed.");
                }
            }
            else
            {
                Debug.LogError("Player panel prefab is null.");
            }
        }

        

        if (infoPanel != null) panels.Add(infoPanel);
        if (morningPanel != null) panels.Add(morningPanel);

        //VOTING PANEL
        string voteName = "voting";
        string decideName = "deciding";
        GameObject votePlane = createOnePanel(voteName, playersList, panels);   
        votingPanelTo = votePlane;
        panels.Add(votePlane);
        votePlane.SetActive(false);            
            //panels.Add(decidePlane);
            Debug.Log("gorko33 " + panels.Count);



            


        }



    

    


    
   

    public void putOnDais()
    {
        if (selectedButton != null)
        {
            TextMeshProUGUI buttonText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
            Players playerCell = new Players();
            playerCell = ps.FirstOrDefault(players => players.pname == buttonText.text);
        }

    }
    public GameObject createOnePanel(string panelName , List<Players> playersList, List<GameObject> panels)
    {
        GameObject votePlane = Instantiate(playerPanelPrefab, spawnPosition, Quaternion.identity, canvas);
        Transform voteNames = votePlane.transform.Find("Names");

        // Calculate the width of a single button
        float votebuttonWidth = circleButtonPrefab.GetComponent<RectTransform>().rect.width;
        // Set the maximum buttons per row
        int maxVoteButtonsPerRow = 3;

        // Calculate the spacing between buttons to fit them in a row
        float voteSpacing = votebuttonWidth * maxVoteButtonsPerRow / 3 * 2;
        int votepCount = playersList.Count;

        int dummyVar = 0;
        if (panelName == "voting")
        {
            for (int i = 0; i < votepCount; i++)
                {


                    // Calculate row and column indices for the button
                    int row = (i + dummyVar) / maxVoteButtonsPerRow;
                    int col = (i + dummyVar) % maxVoteButtonsPerRow;

                    // Calculate the position with appropriate spacing
                    Vector3 buttonPosition = new Vector3(
                        voteNames.position.x + (col) * votebuttonWidth * 2 - voteSpacing,
                        voteNames.position.y - (row - 1) * votebuttonWidth * 2,
                        voteNames.position.z);

                    // Instantiate the button at the calculated position
                    GameObject circleButtonInstance = Instantiate(circleButtonPrefab, buttonPosition, Quaternion.identity, voteNames);
                    TextMeshProUGUI tmpText = circleButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
                    tmpText.text = playersList[i].pname;
                    Button circleButton = circleButtonInstance.GetComponent<Button>();
                    circleButton.onClick.AddListener(() => VoteOnButtonClick(ref circleButton, playersList));
                    
                    //making clickable


                }
                string playerName ="deneiyoz hoca";
                TextMeshProUGUI pNameText1 = votePlane.transform.Find("nameText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI pRoleText2 = votePlane.transform.Find("roleText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI explanationText = votePlane.transform.Find("roleText").Find("roleInfo").GetComponent<TextMeshProUGUI>();
                Button votepassButton = votePlane.transform.Find("passButton").GetComponent<Button>();
                Button votepickButton = votePlane.transform.Find("pickButton").GetComponent<Button>();
                panelManager pManagerFV = GetComponent<panelManager>();
                votepassButton.onClick.AddListener(pManagerFV.passingPages);
                votepickButton.onClick.AddListener(() =>  createTemporaryPanel( panelName,  playerName,  playersList , panels));
                votepickButton.name = "Put";
                pNameText1.text = "The chosen will be put on the dais";
                pRoleText2.text = "SOULS CAN PUT A BODY ON THE DAIS";
        }
        
        votePlane.name = panelName + " 1";
        return votePlane;
    }
    public void changeToStart()
    {
         GameObject closingpanel = GameObject.Find("introPanel");
         closingpanel.SetActive(false);
         openingpanel.SetActive(true);
        
         Debug.Log("yoyoyo number is: " + panels.Count);
     }

    public void changeStatus(string ExeRole)
    {
        if (selectedButton != null)
        {
            TextMeshProUGUI buttonText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log("sdfsdf=  " + ps[0].pname);
            Debug.Log("sdfsdf=  " + ps[0].pname + " DSFDS " + ExeRole);
            Players playerCell = new Players();
            playerCell = ps.FirstOrDefault(players => players.pname == buttonText.text);

            Players choosingCell = ps.FirstOrDefault(players => players.role == ExeRole);
            Debug.Log("resmin var şuan elimde: " + choosingCell.pname);
            if (choosingCell.role == "MERGEN")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.revealed = true;
                
                choosingCell.action = "reveal";
            }
            else if (choosingCell.role == "Ulgen")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.dead = true;
                choosingCell.action = "kill";
            }
            else if (choosingCell.role == "KIZAGAN")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.changeDead(true);
                choosingCell.action = "kill";
            }
            else if (choosingCell.role == "UMAY")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.changeDead(false);
                choosingCell.action = "kill";
            }
            else if (choosingCell.role == "BURKUT")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.protectd = true;
                choosingCell.action = "protect";
            }
            else if (choosingCell.role == "OD ANA")
            {
                //?
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.changeDead(true);
                choosingCell.action = "kill";
            }
            else if (choosingCell.role == "AKBUGA")
            {
                
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.rescued = true;
                choosingCell.action = "rescue";
            }
            else if (choosingCell.role == "AI TOYON")
            {
                choosingCell.revealed = true;   
                choosingCell.action = "reveal";
            }
            else if (choosingCell.role == "AYZIT")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.charmed = true;
                choosingCell.action = "charm";
            }
            else if (choosingCell.role == "ALAZ HAN")
            {
                choosingCell.rescued = true;
                choosingCell.action = "rescue";
            }
            else if (choosingCell.role == "SIGUN GEYIK")
            {
                //?
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.changeDead(true);
                choosingCell.action = "kill";
            }
            else if (choosingCell.role == "YILDIZ HAN")
            {
                //?
                playerCell.changeDead(true);
                choosingCell.action = "kill";
            }
            else if (choosingCell.role == "TEPEGOZ")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.changeDead(true);
                choosingCell.action = "kill";
            }
            else if (choosingCell.role == "DEMIRKIYNAK")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.charmed = true;
                choosingCell.action = "charm";
            }
            else if (choosingCell.role == "SU IYESI")
            {
                for (int i = 0; i < ps.Count; i++)
                {
                    Players markedPlayer = ps[i];
                    if (markedPlayer.markedSu == true)
                    {
                        ps[i].dead = true;
                        choosingCell.action = "kill";
                    }
                }
            }
            else if (choosingCell.role == "KORMOS")
            {
                //?
                playerCell.changeDead(true);
                choosingCell.action = "kill";
            }
            else if (choosingCell.role == "ARCURA")
            {
                //?
                playerCell.changeDead(true);
                choosingCell.action = "kill";
            }
            else if (choosingCell.role == "AZMIC")
            {
                //?
                playerCell.changeDead(true);
                choosingCell.action = "kill";
            }
            else if (choosingCell.role == "GULYABANI")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.changeDead(true);
                choosingCell.action = "kill";
            }
            else if (choosingCell.role == "KORTIGES")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.kortigesed = true;
                choosingCell.action = "kortiges";
            }
            else
            {
                Debug.Log("OLMADIA Q ");
            }
            Debug.Log("resmin mi var şuan elimde: " + playerCell.pname + " - " + playerCell.dead);
        }

        else
        {
            Debug.Log("SIKINTI VAAR");
        }

    }


    public void changeStatusDifferent(string ExeRole , string command)
    {
        if (selectedButton != null)
        {
            Debug.Log("ps count=  " + ps.Count + " DSFDS " + ExeRole);
            TextMeshProUGUI buttonText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log("sdfsdf=  " + ps[0].pname + " DSFDS " + ExeRole);
            Players playerCell = new Players();
            playerCell = ps.FirstOrDefault(players => players.pname == buttonText.text);

            Players choosingCell = ps.FirstOrDefault(players => players.role == ExeRole);
            Debug.Log("resmin var şuan elimde: " + choosingCell.pname);
            if (choosingCell.role == "Ulgen")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.rescued = true;
            }
            
            else if (choosingCell.role == "SU IYESI")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.markedSu = true;
            }
            
            else if (choosingCell.role == "GULYABANI")
            {
                choosingCell.visitedPlayer = playerCell.pname; 
                playerCell.changeDead(true);
            }
            else
            {
                Debug.Log("OLMADIA Q ");
            }
            Debug.Log("resmin mi var şuan elimde: " + playerCell.pname + " - " + playerCell.dead);
        }
        else
        {
            Debug.Log("SIKINTI VAAR");
        }

    }


    private void OnButtonClick(ref Button clickedButton, List<Players> ps)
    {
        selectedButton = clickedButton;
        TextMeshProUGUI buttonText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
        //Debug.Log("DUR BAKIMMM : " + buttonText.text);
    }

    private void VoteOnButtonClick(ref Button clickedButton, List<Players> ps)
    {
        votingButton = clickedButton;
        TextMeshProUGUI buttonText = votingButton.GetComponentInChildren<TextMeshProUGUI>();
        //Debug.Log("DUR BAKIMMM : " + buttonText.text);
    }
    private void createTemporaryPanel(string panelName, string playerName, List<Players> ps , List<GameObject> panels)
    {
        if (votingButton != null)
        {
            Aplay = GetComponent<ActualPlay>();
            TextMeshProUGUI buttonText = votingButton.GetComponentInChildren<TextMeshProUGUI>();
            GameObject tempPlane = Instantiate(playerPanelPrefab, spawnPosition, Quaternion.identity, canvas);
            Transform voteNames = tempPlane.transform.Find("Names");

            // Calculate the width of a single button
            float votebuttonWidth = circleButtonPrefab.GetComponent<RectTransform>().rect.width;
            // Set the maximum buttons per row
            int maxVoteButtonsPerRow = 3;

            // Calculate the spacing between buttons to fit them in a row
            float voteSpacing = votebuttonWidth * maxVoteButtonsPerRow / 3 * 2;
            int votepCount = ps.Count;
            
            int dummyVar = 0;
            TextMeshProUGUI pNameText1 = tempPlane.transform.Find("nameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI pRoleText2 = tempPlane.transform.Find("roleText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI explanationText = tempPlane.transform.Find("roleText").Find("roleInfo").GetComponent<TextMeshProUGUI>();
            Button killVoteButton = tempPlane.transform.Find("passButton").GetComponent<Button>();
            Button cancelVoteButton = tempPlane.transform.Find("pickButton").GetComponent<Button>();

            panelManager pManagerFV = GetComponent<panelManager>();
            TextMeshProUGUI killVoteText = killVoteButton.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI cancelVoteText = cancelVoteButton.GetComponentInChildren<TextMeshProUGUI>();
            killVoteText.text = "Execute";
            cancelVoteText.text = "Forgive";
            string killableName =  buttonText.text;
            pNameText1.text = "Do you want to execute " + killableName + "\n(It needs to " + (ps.Count / 2 +1) + " votes to execute)";
            pRoleText2.text = "EXECUTION";
            tempPlane.name = panelName;
            tempPlane.SetActive(true);
            cancelVoteButton.onClick.AddListener(() => tempPlane.SetActive(false));
            killVoteButton.onClick.AddListener(() => Aplay.killVotedPlayer(killableName) );
            killVoteButton.onClick.AddListener(() => tempPlane.SetActive(false));
            killVoteButton.onClick.AddListener(pManagerFV.passingPages);
            cancelVoteButton.onClick.AddListener(pManagerFV.passingPages);
            votingButton = null;
        }
        
        
        

    }
    private void killWithVote(string killedByVote,List<Players> ps , List<GameObject> panels)
        {
            
            Players playerCell = new Players();
            playerCell = ps.FirstOrDefault(players => players.pname == killedByVote);
            playerCell.changeDead(true);

            for (int m = 0; m < panels.Count; m++)
                {
                    if (panels[m].name == killedByVote)
                    {
                        panels.RemoveAt(m);
                        panels.RemoveAt(m-1);
                    }
                }
            
            
        }
        


}
