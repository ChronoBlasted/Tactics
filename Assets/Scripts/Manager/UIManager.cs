using BaseTemplate.Behaviours;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] Canvas _mainCanvas;

    [SerializeField] MenuView _menuView;
    [SerializeField] GameView _gameView;
    [SerializeField] EndView _endView;

    View _currentView;
    public Canvas MainCanvas { get => _mainCanvas; }

    public void Init()
    {
        GameManager.Instance.OnGameStateChanged += HandleStateChange;

        InitView();

        ChangeView(_gameView);
    }

    public void InitView()
    {
        _menuView.Init();
        _gameView.Init();
        _endView.Init();
    }

    public void ChangeView(View newPanel)
    {
        if (newPanel == _currentView) return;

        if (_currentView != null)
        {
            CloseView(_currentView);
        }

        _currentView = newPanel;
        _currentView.gameObject.SetActive(true);

        _currentView.OpenView();

    }

    void CloseView(View newPanel)
    {
        newPanel.CloseView();
    }

    #region GameState

    void HandleStateChange(GAMESTATE newState)
    {
        switch (newState)
        {
            case GAMESTATE.MENU:
                HandleMenu();
                break;
            case GAMESTATE.GAME:
                HandleGame();
                break;
            case GAMESTATE.END:
                HandleEnd();
                break;
            default:
                break;
        }
    }

    void HandleMenu()
    {
        ChangeView(_menuView);
    }
    void HandleGame()
    {
        ChangeView(_gameView);
    }
    void HandleEnd()
    {
        ChangeView(_endView);
    }

    #endregion
}
