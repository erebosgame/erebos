enum MenuState
{
    MainMenu,
    Paused,
    Dead,
    Playing
}

class MenuLogic
{
    public static MenuState State { get { return state; } }
    static MenuState state;

    public static void Update(MenuState newState)
    {
        state = newState;
        switch(state)
        {
            case MenuState.MainMenu:
                MainUI.SetActive(false);
                BossUI.SetActive(false);
                DeathUI.SetActive(false);
                PauseMenu.SetActive(false);
                TitleScreen.SetActive(true);
                break;
            case MenuState.Paused:
                PauseMenu.SetActive(true);
                break;
            case MenuState.Dead:
                MainUI.SetActive(false);
                BossUI.SetActive(false);
                PauseMenu.SetActive(false);
                TitleScreen.SetActive(false);
                DeathUI.SetActive(true);
                break;
            case MenuState.Playing:
                PauseMenu.SetActive(false);
                TitleScreen.SetActive(false);
                DeathUI.SetActive(false);
                MainUI.SetActive(true);
                break;
        }
    }
}