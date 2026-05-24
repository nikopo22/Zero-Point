using Microsoft.Xna.Framework.Content;
using ZeroPoint.Levels;

namespace ZeroPoint.Managers;

public class LevelManager
{
    public Level1 CurrentLevel { get; private set; }
    private ContentManager contentManager;
    private int currentIndex = 1;

    public LevelManager(ContentManager contentManager)
    {
        this.contentManager = contentManager;
        LoadLevel();
    }

    private void LoadLevel()
    {
        // 1 — простой кодовый уровень, 2 — medium, 3 — hard
        if (currentIndex == 1)
        {
            CurrentLevel = new Level1();
            // Применяем заранее сгенерированный кодовый уровень (easy)
            PrebuiltLevels.ApplyEasyLevel(CurrentLevel);
        }
        else if (currentIndex == 2)
        {
            CurrentLevel = new Level1();
            PrebuiltLevels.ApplyMediumLevel(CurrentLevel);
        }
        else if (currentIndex == 3)
        {
            CurrentLevel = new Level1();
            PrebuiltLevels.ApplyHardLevel(CurrentLevel);
        }
        else
        {
            // При превышении — возвращаемся к первому уровню
            currentIndex = 1;
            LoadLevel();
        }
    }

    public int CurrentLevelIndex => currentIndex;

    public void NextLevel()
    {
        currentIndex++;
        LoadLevel();
    }

    public void ReloadLevel()
    {
        LoadLevel();
    }
}
