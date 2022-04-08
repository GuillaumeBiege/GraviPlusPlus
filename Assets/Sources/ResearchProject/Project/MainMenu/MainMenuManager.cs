using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public enum TypeMainMenu
    {
        MAIN,
        DEMO,
        EXPO
    }


    //Reference GameObjects
    [SerializeField] GameObject m_rMainMenu = default;
    [SerializeField] GameObject m_rDemoMenu = default;
    [SerializeField] GameObject m_rExpoMenu = default;

    //Main menu
    [Space(10)]
    [SerializeField] Button m_rGotoDemoButton = default;
    [SerializeField] Button m_rGotoExpoButton = default;
    [SerializeField] Button m_rQuitGameButton = default;

    //Menu Demo scene
    [Space(10)]
    [SerializeField] List<Button> m_lDemoSceneButtonList = default;
    [SerializeField] Button m_rDemoQuitGameButton = default;


    //Menu Expo scene
    [Space(10)]
    [SerializeField] List<Button> m_lExpoSceneButtonList = default;
    [SerializeField] Button m_rExpoQuitGameButton = default;

    //Variables
    [Space(10)]
    [SerializeField] TypeMainMenu MainMenuState = TypeMainMenu.MAIN;


    // Start is called before the first frame update
    void Start()
    {
        ChangeMainMenu(TypeMainMenu.MAIN);

        m_rDemoMenu.GetComponentsInChildren<Button>(true, m_lDemoSceneButtonList);
        if (m_lDemoSceneButtonList.Contains(m_rDemoQuitGameButton))
        {
            m_lDemoSceneButtonList.Remove(m_rDemoQuitGameButton);
        }

        m_rExpoMenu.GetComponentsInChildren<Button>(true, m_lExpoSceneButtonList);
        if (m_lExpoSceneButtonList.Contains(m_rExpoQuitGameButton))
        {
            m_lExpoSceneButtonList.Remove(m_rExpoQuitGameButton);
        }

        m_rGotoDemoButton.onClick.AddListener(GoToDemoMenu);
        m_rGotoExpoButton.onClick.AddListener(GoToExpoMenu);
        m_rQuitGameButton.onClick.AddListener(QuitGame);

        m_rDemoQuitGameButton.onClick.AddListener(GoBackToMainMenu);
        m_rExpoQuitGameButton.onClick.AddListener(GoBackToMainMenu);

    }

    private void OnDestroy()
    {
        m_rGotoDemoButton.onClick.RemoveAllListeners();
        m_rGotoExpoButton.onClick.RemoveAllListeners();
        m_rQuitGameButton.onClick.RemoveAllListeners();

        m_rDemoQuitGameButton.onClick.RemoveAllListeners();
        m_rExpoQuitGameButton.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region MainMenu Function
    public void GoToDemoMenu()
    {
        ChangeMainMenu(TypeMainMenu.DEMO);
    }
    public void GoToExpoMenu()
    {
        ChangeMainMenu(TypeMainMenu.EXPO);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion


    public void GoToSpecificScene(int _i)
    {
        SceneManager.LoadScene(_i);
    }

    public void GoBackToMainMenu()
    {
        ChangeMainMenu(TypeMainMenu.MAIN);
    }



    public void ChangeMainMenu(TypeMainMenu _e)
    {
        m_rMainMenu.SetActive(false);
        m_rDemoMenu.SetActive(false);
        m_rExpoMenu.SetActive(false);
        switch (_e)
        {
            case TypeMainMenu.MAIN:
                m_rMainMenu.SetActive(true);
                break;
            case TypeMainMenu.DEMO:
                m_rDemoMenu.SetActive(true);
                break;
            case TypeMainMenu.EXPO:
                m_rExpoMenu.SetActive(true);
                break;
            default:
                break;
        }

        MainMenuState = _e;
    }
}
