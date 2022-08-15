using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject UIManager;
    public GameObject cameraPlayer;
    public Player player;
    public PlayerTakeDame playerTakeDame;


    public RectTransform heathBar;
    public GameObject energyPanel;
    public RectTransform energyBar;
    public Transform spawnPointOrigin;

    public bool isTeleport;

    //UI
    public GameObject intro, infoCharacter, notifiSkill4, notifiSkill5, GameOverPanel, PausePanel, settingPanel,settingSoundBtn, pauseBtn, winPanel;
    public FloatingTextManager floatingTextManager;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(cameraPlayer.gameObject);
            Destroy(player.gameObject);
            Destroy(playerTakeDame.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(UIManager.gameObject);
            Destroy(spawnPointOrigin.gameObject);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }


    //Floating  Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    public void OnHealthChange()
    {
        float ratio = (float)player.health / (float)player.maxHealth;
        heathBar.localScale = new Vector3(ratio, 1, 1);
    }


    //Button
    public void PlayGame()
    {
        settingSoundBtn.SetActive(true);
        pauseBtn.SetActive(true);

        Time.timeScale = 1f;

        intro.SetActive(false);
        infoCharacter.SetActive(true);

    }

    public void HomeBtn()
    {
        Time.timeScale = 1f;
        PlayerPrefs.DeleteAll();
        playerTakeDame.anim.SetBool("Dead", false);

        //Set UI default
        playerTakeDame.isUnlockSkill = false;
        playerTakeDame.isUnlockUlti = false;
        player.health = player.maxHealth;
        player._isAlive = true;
        OnHealthChange();

        intro.SetActive(true);
        energyPanel.SetActive(false);
        settingSoundBtn.SetActive(false);
        pauseBtn.SetActive(false);
        infoCharacter.SetActive(false);
        PausePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        winPanel.SetActive(false);

        SoundManager.Ins.PlayMusicBG(SoundManager.Ins.adventureSong);
        SceneManager.LoadScene("Main");
        player.transform.position = spawnPointOrigin.position;
    }
    public void ExitBtn()
    {
        Application.Quit();
    }
    public void ReplayGame()
    {
        isTeleport = true;
        Time.timeScale = 1f;
        player._isAlive = true;
        player.health = player.maxHealth;
        playerTakeDame.anim.SetBool("Dead", false);
        GameOverPanel.SetActive(false);
        OnHealthChange();

        if (!PlayerPrefs.HasKey("SaveState"))
        {
            player.transform.position = spawnPointOrigin.position;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        settingSoundBtn.SetActive(false);
        pauseBtn.SetActive(false);
        Time.timeScale = 0f;
        if (PausePanel)
            PausePanel.SetActive(true);
    }
    public void ContinueGame()
    {
        Time.timeScale = 1f;

        PausePanel.SetActive(false);
        settingSoundBtn.SetActive(true);
        pauseBtn.SetActive(true);
        notifiSkill4.SetActive(false);
        notifiSkill5.SetActive(false);
    }

    //setting Sound/Music
    public void ShowSettingPanel()
    {
        pauseBtn.SetActive(false);
        settingSoundBtn.SetActive(false);
        Time.timeScale = 0f;
        settingPanel.SetActive(true);
    }
    public void UpdateSoundSetting()
    {
        pauseBtn.SetActive(true);
        settingSoundBtn.SetActive(true);
        Time.timeScale = 1f;
        settingPanel.SetActive(false);
    }


    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    //Save state
    /*
     * INT health
     * INT isUnlockSkill
     * INT isUnlockUlti
     * INT isShowInfoChacracter
     */
    public void SaveState()
    {
        int unlockSkill = (playerTakeDame.isUnlockSkill) ? 1 : 0;
        int unlockUlti = (playerTakeDame.isUnlockSkill) ? 1 : 0;


        int isShowInfoCharacter = (infoCharacter.activeSelf) ? 1 : 0;

        string s = "";
        s += player.health.ToString() + "|";
        s += unlockSkill.ToString() + "|";
        s += unlockUlti.ToString() + "|";
        s += isShowInfoCharacter.ToString() + "|";

        PlayerPrefs.SetString("SaveState", s);
        Debug.Log("SaveState");
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        player.health = int.Parse(data[0]);
        playerTakeDame.isUnlockSkill = data[1].Equals("1") ? true : false;
        playerTakeDame.isUnlockUlti = data[2].Equals("1") ? true : false;
        infoCharacter.SetActive(data[3].Equals("1") ? true : false);

        OnHealthChange();
        SoundManager.Ins.PlayMusicBG(SoundManager.Ins.battleSong);

        if (isTeleport == true)
        {
            player.transform.position = GameObject.Find("SpawnPoint").transform.position;
            isTeleport = false;
        }

    }
}
