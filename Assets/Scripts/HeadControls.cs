using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HeadControls : MonoBehaviour
{
    public GameObject trueListHolder;
    public int gameModeInt;
    public int difficulty;

    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private AudioSource voicePlayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material emojiMaterial;

    [SerializeField] private AudioClip correctSFX;
    [SerializeField] private AudioClip incorrectSFX;
    
    private int currentLetter;
    private string currentWord;
    private int randomChild;

    [SerializeField] private float stepLength;
    private List<Vector3> trailPositions = new List<Vector3>();
    private List<Quaternion> trailRotations = new List<Quaternion>();

    public GameObject progressUI;
    public GameObject scoreUI;
    public GameObject letterStreakUI;
    public GameObject wordStreakUI;
    public List<GameObject> strikeUI = new List<GameObject>();

    public GameObject gameplayEffect;
    public GameObject UIEffect;
    public GameObject stopwatchHand;
    [SerializeField] private string[] compliments;

    private int tempScore;
    private int totalScore;
    private int medalLetterStreak;
    private int medalWordStreak;
    private int medalScore;
    private int letterStreak;
    private int wordStreak;
    private bool wordStreakMaintained;
    private int strikes;
    
    void Awake()
    {
        trueListHolder = GameObject.Find("TrueListHolder");
        //logging the game mode for future reference
        gameModeInt = trueListHolder.GetComponent<TrueListHolder>().trueModeInt;
        difficulty = trueListHolder.GetComponent<TrueListHolder>().trueDifficulty;
        stopwatchHand.GetComponent<StopwatchHand>().difficulty = difficulty;
        //setting up the strikes system
        for (int x = 3; x > 4 - difficulty; x--)
        {
            strikeUI[x-1].SetActive(false);
        }
    }

    void Start()
    {
        //not sure if there's a way to do this but right now I just set in manually for simplicity's sake
        //emojiMaterial = GetComponent<TextMeshPro>().material;

        //making the trail list
        for (int i = 0; i < 3; i++)
        {
            trailPositions.Add(transform.GetChild(i + 3).transform.position);
            trailRotations.Add(transform.GetChild(i + 3).transform.rotation);
        }

        //Setting up the sprites for the alphabet levels
        spriteRenderer = transform.GetChild(6).GetComponent<SpriteRenderer>();
        if (gameModeInt < 2)
        {
            spriteRenderer.sprite = trueListHolder.GetComponent<TrueListHolder>().trueSprites[Random.Range(0, 3)];
            transform.GetChild(6).gameObject.SetActive(true);            
            GetComponent<TextMeshPro>().enabled = false;
        }
        strikes = 4-difficulty;
        letterStreak = 0;
        wordStreak = 0;
        medalWordStreak = -1;
        wordStreakMaintained = false;
        //letterStreakUI.GetComponent<TextMeshProUGUI>().text = "Letter Streak: " + letterStreak;
        //wordStreakUI.GetComponent<TextMeshProUGUI>().text = "Word Streak: " + wordStreak;
        scoreUI.GetComponent<TextMeshProUGUI>().text = "Score: " + totalScore;
        //choosing the word and the sprite to go with it
        NewVocab();
    }

    void NewVocab()
    {
        if(wordStreakMaintained == true)
        {
            wordStreak = wordStreak + 1;
            wordStreakUI.GetComponent<TextMeshProUGUI>().text = "Word Streak: " + wordStreak;
        }
        medalWordStreak = medalWordStreak + 1;
        //resetting the word streak
        wordStreakMaintained = true;
        //resetting the strikes
        strikes = 4 - difficulty;
        for (int x = 0; x < 4 - difficulty; x++)
        {
            strikeUI[x].SetActive(true);
        }
        currentLetter = 0;
        progressUI.GetComponent<TextMeshProUGUI>().text = "";
        int randomVocab = Random.Range(0, trueListHolder.GetComponent<TrueListHolder>().trueList.Count);
        currentWord = trueListHolder.GetComponent<TrueListHolder>().trueList[randomVocab];
        GetComponent<TextMeshPro>().text = trueListHolder.GetComponent<TrueListHolder>().trueEmojis[randomVocab];
        //THIS IS TO TEST OUT THE VOICE ACTING SYSTEM - RIGHT NOW IT ONLY WORKS WITH THE ANIMAL LEVEL
        if(gameModeInt == 2)
        {
            voicePlayer.clip = trueListHolder.GetComponent<TrueListHolder>().trueVA[randomVocab];
            voicePlayer.Play();
            trueListHolder.GetComponent<TrueListHolder>().trueVA.RemoveAt(randomVocab);
        }
        //removing that item from the list
        trueListHolder.GetComponent<TrueListHolder>().trueList.RemoveAt(randomVocab);
        trueListHolder.GetComponent<TrueListHolder>().trueEmojis.RemoveAt(randomVocab);
        ShuffleLetters();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) == true || Input.GetKeyDown(KeyCode.A) == true)
        {
            ArrowPressed(-1, 0, 90);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) == true || Input.GetKeyDown(KeyCode.D) == true)
        {
            ArrowPressed(1, 0, 270);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) == true || Input.GetKeyDown(KeyCode.W) == true)
        {
            ArrowPressed(0, 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) == true || Input.GetKeyDown(KeyCode.S) == true)
        {
            ArrowPressed(0, -1, 180);
        }

        //DYNAMIC SCORE ADDITION
        if (tempScore < totalScore)
        {
            int tempDiff = Mathf.FloorToInt(Mathf.Log10(totalScore - tempScore));
            tempScore = tempScore + Mathf.FloorToInt(Mathf.Pow(5, tempDiff));
            scoreUI.GetComponent<TextMeshProUGUI>().text = "Score:\n" + tempScore;
        }
    }

    void ArrowPressed(int xMove, int yMove, int direction)
    {
        //Vector3 rotatedStep = Quaternion.AngleAxis(direction, Vector3.up) * step;

        //Checking if the player tried to go back the way they came.
        if (Mathf.Abs(transform.eulerAngles.z - direction) == 180)
        {
            if (gameModeInt < 2)
            {
                spriteRenderer.sprite = trueListHolder.GetComponent<TrueListHolder>().trueSprites[4];
            }
            StartCoroutine(WrongWay(xMove, yMove));
            return;
        }
        //MOVEMENT CODE FOR IF THE PLAYER MOVED IN A VALID DIRECTION
        else
        {
            StopCoroutine(WrongWay(xMove, yMove));
            emojiMaterial.color = new Color(1, 1, 1);
            spriteRenderer.color = new Color(1, 1, 1);
            transform.position = transform.position + new Vector3(xMove * stepLength, yMove * stepLength, 0);
        }
        //IF THE CORRECT LETTER WAS CHOSEN
        if (transform.eulerAngles.z - direction == 90 * randomChild || transform.eulerAngles.z - direction == -270 * randomChild)
        {
            progressUI.GetComponent<TextMeshProUGUI>().text = progressUI.GetComponent<TextMeshProUGUI>().text + currentWord[currentLetter].ToString();
            currentLetter = currentLetter + 1;
            sfxPlayer.clip = correctSFX;
            sfxPlayer.Play();

            //CHANGING THE SPRITE FOR THE ALPHABET LEVELS
            if (gameModeInt < 2)
            {
                spriteRenderer.sprite = trueListHolder.GetComponent<TrueListHolder>().trueSprites[Random.Range(0, 3)];
            }

            //INCREASE THE SCORE
            int timeBonus = Mathf.RoundToInt((stopwatchHand.transform.eulerAngles.z / 36) * Mathf.Pow(10, difficulty));
            int letterStreakBonus = letterStreak * Mathf.RoundToInt(Mathf.Pow(10, difficulty-1));
            int medalTime = 135 + (45 * difficulty);
            int medalBonus = Mathf.RoundToInt((medalTime / 36) * Mathf.Pow(10, difficulty));
            int medalLetterStreakBonus = medalLetterStreak * Mathf.RoundToInt(Mathf.Pow(10, difficulty - 1));
            totalScore = totalScore + timeBonus + letterStreakBonus;
            medalScore = medalScore + medalBonus + medalLetterStreakBonus;
            //INCREASE THE LETTER STREAK
            letterStreak = letterStreak + 1;
            medalLetterStreak = medalLetterStreak + 1;
            //letterStreakUI.GetComponent<TextMeshProUGUI>().text = "Letter Streak: " + letterStreak;

            //TIME BONUS EFFECT
            NewEffect(true, new Vector2(stopwatchHand.transform.position.x + 100, stopwatchHand.transform.position.y), 0, new Vector2(1, 1), "Time Bonus +" + timeBonus);
            //LETTER STREAK EFFECT
            NewEffect(true, new Vector2(890, 17), 0, new Vector2(0.5f, 0.5f), "Letter Streak: " + letterStreak);
            //DIAGONAL COMPLIMENT EFFECT
            if (gameModeInt == 0)
            {
                NewEffect(false, transform.position, direction + Random.Range(-45, 45), new Vector2(1, 1), compliments[currentLetter-1]);
            }
            else
            {
                NewEffect(false, transform.position, direction + Random.Range(-45, 45), new Vector2(1, 1), compliments[Random.Range(0, compliments.Length)]);
            }
        }
        //IF THE WRONG LETTER WAS CHOSEN
        else
        {
            //CHANGING THE SPRITE FOR THE ALPHABET LEVELS
            if (gameModeInt < 2)
            {
                spriteRenderer.sprite = trueListHolder.GetComponent<TrueListHolder>().trueSprites[4];
            }

            //REMOVE A STRIKE AND RESET THE LETTER STREAK & WORD STREAK
            strikeUI[strikes-1].SetActive(false);
            letterStreak = 0;
            letterStreakUI.GetComponent<TextMeshProUGUI>().text = "Letter Streak: " + letterStreak;
            wordStreak = 0;
            wordStreakMaintained = false;
            wordStreakUI.GetComponent<TextMeshProUGUI>().text = "Word Streak: " + wordStreak;
            strikes = strikes - 1;
            //IF THAT WAS THE FINAL STRIKE
            if (strikes <= 0)
            {
                trueListHolder.GetComponent<TrueListHolder>().beatLevel = false;
                QuitLevel(false);
                return;
            }
            StartCoroutine(WrongWay(0, 0));
        }
        //rotating the head
        transform.eulerAngles = new Vector3(0, 0, direction);
        //changing the trail list and getting rid of the last item
        trailPositions.Insert(0, transform.GetChild(3).transform.position);
        trailRotations.Insert(0, transform.GetChild(3).transform.rotation);
        trailPositions.RemoveAt(trailPositions.Count - 1);
        trailRotations.RemoveAt(trailRotations.Count - 1);
        //this could maybe be moved to another loop to make it more efficient? anyway it's also for changing the trail
        for (int i = 1; i < 3; i++)
        {
            transform.GetChild(i + 3).transform.position = trailPositions[i];
            transform.GetChild(i + 3).transform.rotation = trailRotations[i];
        }
        ShuffleLetters();
    }

    void ShuffleLetters()
    {
        //reset the stopwatch
        stopwatchHand.transform.localEulerAngles = new Vector3(0, 0, 359.9f);
        //copying the word to a temp list
        List<char> letters = new List<char>();
        //splitting up the current word into a list of characters
        for (int a = 0; a < currentWord.Length; a++)
        {
            letters.Add(currentWord[a]);
        }
        //WIN CONDITION - IF THE CURRENT LETTER WAS THE LAST IN THE LIST
        if (currentLetter >= letters.Count)
        {
            if(trueListHolder.GetComponent<TrueListHolder>().trueList.Count == 0)
            {
                trueListHolder.GetComponent<TrueListHolder>().beatLevel = true;
                trueListHolder.GetComponent<TrueListHolder>().medalScore = medalScore;
                QuitLevel(false);
                return;
            }
            else
            {   
                //ADDING WORD STREAK BONUS
                totalScore = totalScore + Mathf.RoundToInt(wordStreak * Mathf.Pow(10, difficulty + 1));
                medalScore = medalScore + Mathf.RoundToInt(medalWordStreak * Mathf.Pow(10, difficulty + 1));
                NewVocab();
                return;
            }
            
        }
        //temporary variable with the correct letter
        string correctLetterString = letters[currentLetter].ToString();
        //getting rid of the correct letter in the alphabet list
        letters.RemoveAt(currentLetter);
        for (int i = 0; i < 3; i++)
        {
            //making sure the letters rotate correctly
            transform.GetChild(i).transform.eulerAngles = new Vector3(0, 0, 0);
            transform.GetChild(i).GetComponent<LetterMovement>().wobble = 0;
            string randomLetterString = letters[Random.Range(0, letters.Count)].ToString();
            transform.GetChild(i).GetComponent<TextMeshPro>().text = randomLetterString;
            //redoing the loop if the random letter is a double letter that's the same as the correct letter
            if(randomLetterString == correctLetterString)
            {
                //letters.RemoveAt(i); this should work but there were issues so I'm not sure and it's not necessary it would just make the code more efficient
                i = i - 1;
            }
            //removing any double letters that aren't the correct letter
            for (int j = 0; j < letters.Count; j++)
            {
                if (letters[j].ToString() == randomLetterString)
                {
                    letters.RemoveAt(j);
                    //to account for the removed item. Otherwise it would skip.
                    j = j - 1;
                }
            }
        }
        //changing one of the letters again to be the correct one. The Random.Range is effectively (0,3) for choosing a child, but it's (-1,2) to check for direction later.
        randomChild = Random.Range(-1, 2);
        transform.GetChild(randomChild+1).GetComponent<TextMeshPro>().text = correctLetterString;
    }

    private IEnumerator WrongWay(int xMove, int yMove)
    {
        sfxPlayer.clip = incorrectSFX;
        sfxPlayer.Play();
        emojiMaterial.color = new Color(1, 0, 0);
        spriteRenderer.color = new Color(1, 0, 0);
        transform.position = transform.position - new Vector3(xMove * -stepLength * 0.25f, yMove * -stepLength * 0.25f, 0);

        yield return new WaitForSeconds(0.25f);

        emojiMaterial.color = new Color(1, 1, 1);
        spriteRenderer.color = new Color(1, 1, 1);
        transform.position = transform.position + new Vector3(xMove * -stepLength * 0.25f, yMove * -stepLength * 0.25f, 0);
    }

    private void NewEffect(bool UI, Vector2 effectPosition, int effectDirection, Vector2 effectScale, string text)
    {
        GameObject tempEffect;
        if(UI == true)
        {
            tempEffect = Instantiate(UIEffect);
            tempEffect.GetComponent<TextMeshProUGUI>().text = text;
            //setting the canvas as the parent so that the UI effect shows up
            tempEffect.transform.SetParent(UIEffect.transform.parent, false);
        }
        else
        {
            tempEffect = Instantiate(gameplayEffect);
            tempEffect.GetComponent<TextMeshPro>().text = text;
        }
        tempEffect.SetActive(true);
        tempEffect.transform.position = effectPosition;
        tempEffect.transform.localEulerAngles = new Vector3(0, 0, effectDirection);
        tempEffect.transform.localScale = effectScale;
    }

    public void QuitLevel(bool backButton)
    {
        emojiMaterial.color = new Color(1, 1, 1);
        trueListHolder.GetComponent<TrueListHolder>().quitLevel = backButton;
        trueListHolder.GetComponent<TrueListHolder>().score = totalScore;
        trueListHolder.GetComponent<TrueListHolder>().UpdateScores();
        SceneManager.LoadScene(0);
    }
}
