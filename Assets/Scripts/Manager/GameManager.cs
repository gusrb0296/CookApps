using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    [SerializeField] Button btn_GameStart;

    public GameBoard gameBoard;

    public Cell cell_EnemyPos;

    

    //private void Start()
    //{
    //    btn_GameStart.onClick.AddListener();
    //}

    //#region Timer
    //public void TimerOn()
    //{
    //    _time = 61;
    //    image_Timer.gameObject.SetActive(true);
    //    StartCoroutine(StartTimer());
    //}

    //public void TimerOff()
    //{
    //    image_Timer.gameObject.SetActive(false);
    //    StopCoroutine(StartTimer());
    //}

    //IEnumerator StartTimer()
    //{
    //    _curTime = _time;
    //    while (_curTime > 0)
    //    {
    //        _curTime -= Time.deltaTime;
    //        image_TimerGauge.fillAmount = _curTime / _time;
    //        _minute = (int)_curTime / 60;
    //        _second = (int)_curTime % 60;
    //        _textTime.text = $"{_minute.ToString("00")} : {_second.ToString("00")}";
    //        yield return null;

    //        if (_curTime <= 0)
    //        {
    //            Manager.Game.Player.PlayerIsDead();
    //            _curTime = 0;
    //            image_Timer.gameObject.SetActive(false);
    //            yield break;
    //        }
    //    }
    //}

    //#endregion
}
