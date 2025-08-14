using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueListHolder : MonoBehaviour
{
    public string trueMode;
    public List<string> trueList = new List<string>();
    public List<string> trueEmojis = new List<string>();
    public List<Sprite> trueSprites = new List<Sprite>();
    public List<AudioClip> trueVA = new List<AudioClip>();
    public int trueDifficulty;
    public bool trueEffectsOn;
    public bool voiceOn;
    public bool progressOn;

    public bool firstTime;
    public int trueModeInt;
    public bool beatLevel;
    public bool quitLevel;
    public int score;
    public int medalScore;

    public List<int> highScores = new List<int>();
    public List<int> medalsEarned = new List<int>();

    public void UpdateScores()
    {
        if(score > highScores[trueModeInt])
        {
            highScores[trueModeInt] = score;
        }
    }
}
