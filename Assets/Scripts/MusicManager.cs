using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

    public AudioClip mainTheme;
    public AudioClip menuTheme;
    AudioClip lastPlayed;

    string sceneName;
    bool stoppedMusic = false;

    void Start()
    {
        sceneName = "Menu";
        lastPlayed = menuTheme;
        AudioManager.instance.PlayMusic(menuTheme, 2);
    }

    void OnLevelWasLoaded()
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != null && sceneName != null && ((sceneName.Equals("Game") && !newSceneName.Equals(sceneName)) || newSceneName.Equals("Game"))){
            sceneName = newSceneName;
            Invoke("PlayMusic", .2f);
        }
    }

    void PlayMusic()
    {
        AudioClip clipToPlay = null;
        if(sceneName.Equals("Game"))
            clipToPlay = mainTheme;
        else
            clipToPlay = menuTheme;

        if(clipToPlay != null && lastPlayed != clipToPlay){
            CancelInvoke();
            lastPlayed = clipToPlay;
            AudioManager.instance.PlayMusic(clipToPlay, 2);
            Invoke("PlayMusic", clipToPlay.length);
        }else{
            CancelInvoke();
            lastPlayed = clipToPlay;
            AudioManager.instance.PlayMusic(clipToPlay, 0);
            Invoke("PlayMusic", clipToPlay.length);
        }
    }
}
