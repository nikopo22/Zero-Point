namespace ZeroPoint.Utils;

public static class Constants
{
    // размеры экрана
    public const int SCREEN_WIDTH = 1280;
    public const int SCREEN_HEIGHT = 720;

    // физика игрока
    public const float PLAYER_SPEED = 400f;      
    public const float PLAYER_JUMP_FORCE = -650f;  
    public const float GRAVITY = 1800f;            

    // размеры робота (подогнаны под визуальный масштаб и кроп)
    public const int PLAYER_WIDTH = 64;
    public const int PLAYER_HEIGHT = 52;

    // размеры платформ
    public const int PLATFORM_WIDTH = 80;
    public const int PLATFORM_HEIGHT = 20;

    // размеры шипов
    public const int SPIKE_WIDTH = 32;
    public const int SPIKE_HEIGHT = 32;

    // радиус сканирования скрытых платформ
    public const float SCAN_RADIUS = 300f;
    
    // размеры уровня
    public const int LEVEL_WIDTH = 2000;
    public const int LEVEL_HEIGHT = 1000;
}
