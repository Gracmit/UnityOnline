using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
     [SerializeField] private Image _timerImage;

     private void Awake()
     {
          _timerImage.fillAmount = 1;
     }

     private void Update()
     {
          if (GameManager.Instance.IsGamePlaying())
          {
               _timerImage.fillAmount = GameManager.Instance.GetPlayingTimerNormalized();
          }
     }
}
