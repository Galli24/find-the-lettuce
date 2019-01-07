using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerVariables : MonoBehaviour
{

    public static PlayerVariables instance;

    bool playerCanMove = true;
    bool paused = false;
    bool cutscene = false;
    bool win = false;

    Vector3 checkpoint = new Vector3(-55.624f, 0.7f, -1f);

    public GameObject timerObject;
    public GameObject playerPrefab;
    public GameObject pauseMenu;
    public GameObject winMenu;

    Text timerText;
    public Text winText;

    float timer = 0f;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            timerText = timerObject.GetComponent<Text>();
        }
        Instantiate(playerPrefab, checkpoint, Quaternion.identity);
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        setPlayerCanMove(false);
        Time.timeScale = 0f;
    }

    void UnPause()
    {
        pauseMenu.SetActive(false);
        if (!cutscene)
            setPlayerCanMove(true);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void restartFromCheckpoint()
    {
        if ((!PlayerVariables.instance.getPlayerCanMove() && !paused) || cutscene)
            return;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Destroy(playerObject);
            Instantiate(playerPrefab, checkpoint, Quaternion.identity);
            UnPause();
        }
    }

    public void playerDied()
    {
        Instantiate(playerPrefab, checkpoint, Quaternion.identity); 
    }

    public void setPlayerCanMove(bool value)
    {
        playerCanMove = value;
    }

    public bool getPaused()
    {
        return (paused);
    }

    public void setPaused(bool value)
    {
        paused = value;
        if (paused == true)
            Pause();
        else
            UnPause();
    }

    public bool getCutscene()
    {
        return (cutscene);
    }

    public void setCutscene(bool value)
    {
        cutscene = value;
    }

    public bool getPlayerCanMove()
    {
        return (playerCanMove);
    }

    public void setCheckpoint(Vector3 value)
    {
        checkpoint = value;
    }

    public Vector3 getCheckpoint()
    {
        return (checkpoint);
    }

    void Update()
    {
        if (!paused && !cutscene && !win)
            timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerText.text = "Time: " + niceTime;
    }

    public void Win()
    {
        win = true;

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        winText.text = "Well done! Your time was " + niceTime;
        winMenu.SetActive(true);
    }
}
