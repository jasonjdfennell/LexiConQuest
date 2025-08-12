using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HighManager : MonoBehaviour
{
    public GameObject trueListHolder;

    public GameObject currentLevelText;
    public GameObject currentDifficultyText;

    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject difficultyMenu;
    public GameObject winScreen;
    public GameObject winScreenMedal;
    public GameObject loseScreen;
    public List<GameObject> levelButtons;
    public List<Sprite> medalSprites;

    public GameObject winScore;
    public GameObject loseScore;

    public List<string> modeNames = new List<string>();
    public List<string> difficultyOptions = new List<string>();

    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private AudioClip titleOST;
    [SerializeField] private AudioClip victoryOST;
    [SerializeField] private AudioClip failureOST;
    //[SerializeField] private AudioClip[] titleVoices;
    //[SerializeField] private AudioClip menuSFX;

    //I don't know if there's a way to do this where the size of the array is dependent on the number of modes in modeNames. Maybe it'd be better to have a List of Lists instead of an Array of Lists.
    public List<string>[] vocabListsArray = new List<string>[0];
    public List<string>[] vocabEmojisArray = new List<string>[0];
    public List<Sprite>[] SpritesArray = new List<Sprite>[2];

    public List<string> alphabetList = new List<string>();
    public List<string> alphabetEmojis = new List<string>();
    public List<Sprite> alphabetSprites = new List<Sprite>();
    public List<string> backwardsAlphabetList = new List<string>();
    public List<string> backwardsAlphabetEmojis = new List<string>();
    public List<Sprite> backwardsAlphabetSprites = new List<Sprite>();
    public List<string> animalList = new List<string>();
    public List<string> animalEmojis = new List<string>();
    public List<AudioClip> animalVA = new List<AudioClip>();
    public List<string> foodList = new List<string>();
    public List<string> foodEmojis = new List<string>();
    public List<string> fruitList = new List<string>();
    public List<string> fruitEmojis = new List<string>();
    public List<string> vegetableList = new List<string>();
    public List<string> vegetableEmojis = new List<string>();
    public List<string> colorList = new List<string>();
    public List<string> colorEmojis = new List<string>();
    public List<string> sportList = new List<string>();
    public List<string> sportEmojis = new List<string>();
    public List<string> clothingList = new List<string>();
    public List<string> clothingEmojis = new List<string>();
    public List<string> instrumentList = new List<string>();
    public List<string> instrumentEmojis = new List<string>();
    public List<string> objectList = new List<string>();
    public List<string> objectEmojis = new List<string>();

    void Start()
    {
        DontDestroyOnLoad(trueListHolder);
        //This automatically changes the size of the array. It's just one less thing that I have to change manually but unfortunately it's still not a fully automatic process.
        //REMEMBER TO INCREASE THE SIZE OF FIRSTLISTHOLDER'S HIGH SCORES AND MEDALS EARNED LISTS
        vocabListsArray = new List<string>[modeNames.Count];
        vocabEmojisArray = new List<string>[modeNames.Count];
        vocabListsArray[0] = alphabetList;
        vocabListsArray[1] = backwardsAlphabetList;
        vocabListsArray[2] = animalList;
        vocabListsArray[3] = foodList;
        vocabListsArray[4] = fruitList;
        vocabListsArray[5] = vegetableList;
        vocabListsArray[6] = colorList;
        vocabListsArray[7] = sportList;
        vocabListsArray[8] = clothingList;
        vocabListsArray[9] = instrumentList;
        vocabListsArray[10] = objectList;
        vocabEmojisArray[0] = alphabetEmojis;
        vocabEmojisArray[1] = backwardsAlphabetEmojis;
        vocabEmojisArray[2] = animalEmojis;
        vocabEmojisArray[3] = foodEmojis;
        vocabEmojisArray[4] = fruitEmojis;
        vocabEmojisArray[5] = vegetableEmojis;
        vocabEmojisArray[6] = colorEmojis;
        vocabEmojisArray[7] = sportEmojis;
        vocabEmojisArray[8] = clothingEmojis;
        vocabEmojisArray[9] = instrumentEmojis;
        vocabEmojisArray[10] = objectEmojis;
        SpritesArray[0] = alphabetSprites;
        SpritesArray[1] = backwardsAlphabetSprites;
        //REMEMBER TO ADD ONE TO THE HIGH SCORE LIST IN TRUELISTHOLDER.

        //This is how I save the player's preferences.
        GameObject returningListHolder = GameObject.Find("TrueListHolder");
        if (returningListHolder != null)
        {
            //copying data from the old list holder to a fresh one
            trueListHolder.GetComponent<TrueListHolder>().trueDifficulty = returningListHolder.GetComponent<TrueListHolder>().trueDifficulty;
            trueListHolder.GetComponent<TrueListHolder>().highScores = returningListHolder.GetComponent<TrueListHolder>().highScores;
            trueListHolder.GetComponent<TrueListHolder>().medalsEarned = returningListHolder.GetComponent<TrueListHolder>().medalsEarned;
            ChangeMode(returningListHolder.GetComponent<TrueListHolder>().trueModeInt);
            ChangeDifficulty(returningListHolder.GetComponent<TrueListHolder>().trueDifficulty);
            //going straight to the main menu if the player quit the level instead of beating it or failing it
            if(returningListHolder.GetComponent<TrueListHolder>().quitLevel == true)
            {
                
                MainMenu();
            }
            else
            {
                //activating either the win screen or lose screen
                if (returningListHolder.GetComponent<TrueListHolder>().beatLevel == true)
                {
                    LevelComplete(returningListHolder.GetComponent<TrueListHolder>().score, returningListHolder.GetComponent<TrueListHolder>().medalScore);
                }
                else
                {
                    GameOver(returningListHolder.GetComponent<TrueListHolder>().score);
                }
            }
            Destroy(returningListHolder);
        }
        //FIRST TIME THE GAME IS OPENED
        else
        {
            //sfxPlayer.clip = titleVoices[Random.Range(0, titleVoices.Length)];
            MainMenu();
        }
        musicPlayer.Play();
        //SET UP THE HIGH SCORES AND MEDALS IN THE LEVEL SELECT MENU
        List<int> highScores = trueListHolder.GetComponent<TrueListHolder>().highScores;
        List<int> medalsEarned = trueListHolder.GetComponent<TrueListHolder>().medalsEarned;
        for (int i = 0; i < levelButtons.Count; i++)
        {
            //High Score Child
            levelButtons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "High Score: " + highScores[i];
            //Medal Child
            if (medalsEarned[i] > 0)
            {
                levelButtons[i].transform.GetChild(3).gameObject.SetActive(true);
                levelButtons[i].transform.GetChild(3).GetComponent<Image>().sprite = medalSprites[medalsEarned[i]];
            }
        }
    }

    public void Play()
    {
        trueListHolder.name = "TrueListHolder";
        trueListHolder.GetComponent<TrueListHolder>().firstTime = false;

        sfxPlayer.Play();
        SceneManager.LoadScene(1);
    }

    public void ChangeMode(int modeIndex)
    {
        Debug.Log(modeNames[modeIndex]);
        trueListHolder.GetComponent<TrueListHolder>().trueMode = modeNames[modeIndex];
        trueListHolder.GetComponent<TrueListHolder>().trueModeInt = modeIndex;
        trueListHolder.GetComponent<TrueListHolder>().trueList = vocabListsArray[modeIndex];
        trueListHolder.GetComponent<TrueListHolder>().trueEmojis = vocabEmojisArray[modeIndex];
        if(modeIndex<2)
        {
            trueListHolder.GetComponent<TrueListHolder>().trueSprites = SpritesArray[modeIndex];
        }
        //changing the UI
        currentLevelText.GetComponent<TextMeshProUGUI>().text = modeNames[modeIndex];
        sfxPlayer.Play();
        MainMenu();
    }

    public void ChangeDifficulty(int difficulty)
    {
        trueListHolder.GetComponent<TrueListHolder>().trueDifficulty = difficulty;
        //changing the UI
        currentDifficultyText.GetComponent<TextMeshProUGUI>().text = difficultyOptions[difficulty-1];
        sfxPlayer.Play();
        MainMenu();
    }

    public void MainMenu()
    {
        levelMenu.SetActive(false);
        difficultyMenu.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        mainMenu.SetActive(true);
        sfxPlayer.Play(); 
        if(musicPlayer.clip != titleOST)
        {
            musicPlayer.clip = titleOST;
            musicPlayer.Play();
        }
    }

    public void LevelSelect()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(true);
        for (int i = 0; i < levelButtons.Count; i++)
        {
            List<string> emojiSelect = vocabEmojisArray[i];
            levelButtons[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = emojiSelect[Random.Range(0, emojiSelect.Count)];
        }
        sfxPlayer.Play();
    }

    public void DifficultySelect()
    {
        mainMenu.SetActive(false);
        difficultyMenu.SetActive(true);
        sfxPlayer.Play();
    }

    private void LevelComplete(int score, int medalScore)
    {
        if (score > medalScore)
        {
            //updating medal array
            if(trueListHolder.GetComponent<TrueListHolder>().medalsEarned[trueListHolder.GetComponent<TrueListHolder>().trueModeInt] < trueListHolder.GetComponent<TrueListHolder>().trueDifficulty)
            {
                trueListHolder.GetComponent<TrueListHolder>().medalsEarned[trueListHolder.GetComponent<TrueListHolder>().trueModeInt] = trueListHolder.GetComponent<TrueListHolder>().trueDifficulty;
            }
            winScore.GetComponent<TextMeshProUGUI>().text = "Score: " + score +
                "\nHigh Score: " + trueListHolder.GetComponent<TrueListHolder>().highScores[trueListHolder.GetComponent<TrueListHolder>().trueModeInt] +
                "\nYou Win A Medal!";
        }
        else
        {
            winScore.GetComponent<TextMeshProUGUI>().text = "Score: " + score +
                "\nHigh Score: " + trueListHolder.GetComponent<TrueListHolder>().highScores[trueListHolder.GetComponent<TrueListHolder>().trueModeInt] +
                "\nScore " + (medalScore - score) + " More For A Medal!";
        }
        //updating the medal sprite
        winScreenMedal.GetComponent<Image>().sprite = medalSprites[trueListHolder.GetComponent<TrueListHolder>().trueDifficulty];
        mainMenu.SetActive(false);
        winScreen.SetActive(true);
        musicPlayer.clip = victoryOST;
    }

    private void GameOver(int score)
    {
        loseScore.GetComponent<TextMeshProUGUI>().text = "Score: " + score + 
            "\nHigh Score: " + trueListHolder.GetComponent<TrueListHolder>().highScores[trueListHolder.GetComponent<TrueListHolder>().trueModeInt];
        mainMenu.SetActive(false);
        loseScreen.SetActive(true);
        musicPlayer.clip = failureOST;
    }

    public void QuitGame()
    {
        sfxPlayer.Play();
        Application.Quit();
    }
}
