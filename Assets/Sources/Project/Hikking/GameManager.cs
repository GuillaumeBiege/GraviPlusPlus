using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void VoidDelegate();

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject m_rGoal = default;


    public bool IsDemoComplete = false;
    public event VoidDelegate ONDemoCompleted;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteDemo()
    {
        ONDemoCompleted?.Invoke();

        IsDemoComplete = true;
    }

    public void RetryDemo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
