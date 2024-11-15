using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelManager : SaveableMonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject finishUI;
    public RectTransform scoreboardContent;
    public ScoreboardItem scoreboardItem;

    private float elapsedTime;
    private LevelData _data = new LevelData();
    private bool _finished;

    private void Start()
    {
        SavingSystem.instance.Load();
    }

    private void Update()
    {
        if (_finished) return;

        elapsedTime += Time.deltaTime;

        timerText.text = FormatTime(elapsedTime);
    }

    [ContextMenu("Finish Level")]
    public void FinishLevel()
    {
        AddTimeToData();
        SavingSystem.instance.Save();
        _finished = true;

        finishUI.SetActive(true);
        foreach (var timer in _data.timers)
        {
            var item = Instantiate(scoreboardItem, scoreboardContent);
            item.username.text = timer.username;
            item.score.text = FormatTime(timer.timer);
        }
    }

    private void AddTimeToData()
    {
        _data.timers.Add(new LevelTimer() { username = PlayerPrefs.GetString("username"), timer = elapsedTime});
        _data.timers = _data.timers.OrderBy(a => a.timer).ThenBy(a => a.username).ToList();
    }

    public override object CaptureState() => _data;

    public override void RestoreState(object state)
    {
        var castedState = state as LevelData;

        if (castedState == null)
        {
            return;
        }

        _data = castedState;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public class LevelData
    {
        public List<LevelTimer> timers = new List<LevelTimer>();
    }

    public class LevelTimer
    {
        public string username;
        public float timer;
    }
}
