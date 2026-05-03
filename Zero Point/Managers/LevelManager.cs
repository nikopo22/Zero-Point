using ZeroPoint.Levels;

namespace ZeroPoint.Managers;

public class LevelManager
{
    public Level1 CurrentLevel { get; private set; }
    public int CurrentLevelNumber { get; private set; }

    public LevelManager()
    {
        CurrentLevelNumber = 1;
        LoadLevel(1);
    }

    public void LoadLevel(int levelNumber)
    {
        switch (levelNumber)
        {
            case 1:
                CurrentLevel = new Level1();
                break;

            default:
                CurrentLevel = new Level1();
                break;
        }

        CurrentLevelNumber = levelNumber;
        CurrentLevel.LevelCompleted = false;
    }

    public void NextLevel()
    {
        if (CurrentLevelNumber < 3) 
        {
            LoadLevel(CurrentLevelNumber + 1);
        }
        // else - экран победы
    }
}
