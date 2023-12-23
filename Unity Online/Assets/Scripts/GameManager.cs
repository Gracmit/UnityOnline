using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnStateChanged;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        Playing,
        GameOver
    }

    private State _state;
    private float _stateTimer = 1f;
    private static GameManager _instance;
    private float _playingTimerMax = 10f;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
        _state = State.WaitingToStart;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                _stateTimer -= Time.deltaTime;
                if (_stateTimer < 0)
                {
                    _state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    _stateTimer = 3f;
                }

                break;
            case State.CountdownToStart:
                _stateTimer -= Time.deltaTime;
                if (_stateTimer < 0)
                {
                    _state = State.Playing;
                    _stateTimer = _playingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;
            case State.Playing:
                _stateTimer -= Time.deltaTime;
                if (_stateTimer < 0)
                {
                    _state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying() => _state == State.Playing;

    public bool IsCountdownToStartActive() => _state == State.CountdownToStart;

    public bool IsGameOver() => _state == State.GameOver;

    public float GetStateTimer() => _stateTimer;

    public float GetPlayingTimerNormalized() => 1 - (_stateTimer / _playingTimerMax);
}