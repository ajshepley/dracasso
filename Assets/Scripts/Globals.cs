public static class Globals
{

  public enum PlayerDirection
  {
    LEFT,
    RIGHT
  }

  public enum PlayerAction
  {
    WALKING,
    JUMPING,
    BITING,
    IDLE
  }

  public class SpriteKeys
  {
    public const string LOW_BLOOD = "low";
    public const string MID_BLOOD = "mid";
    public const string HIGH_BLOOD = "high";
  }

  public class Tags
  {
    // Must correspond to Tags in unityeditor
    public const string GROUND = "platform_ground";
    public const string PLAYER = "Player";
  }
}
