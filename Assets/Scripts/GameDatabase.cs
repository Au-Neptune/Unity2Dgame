public static class GameDatabase
{
    public static float PlayerHP = 200;
    public static float BossHP = 400;
    public static bool BowActive = false;

    public static int operationMode = 0; //0 WASD , 1 ARROW, 2 JoyStick
    public static int loadingNo = -1; //-1 New Game, 0 最新, 1 第二新, 2 第三新;
    public static bool win = false;
}