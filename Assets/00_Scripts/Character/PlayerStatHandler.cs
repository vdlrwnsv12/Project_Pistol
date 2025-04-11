using DataDeclaration;

public class PlayerStatHandler
{
    private Player player;
    private CharacterStat stat;
    
    public CharacterStat Stat => stat;

    public PlayerStatHandler(Player player)
    {
        this.player = player;
        stat = player.Data.Stat;
    }

    //TODO: Test용 생성자
    public PlayerStatHandler(PlayerData playerData)
    {
        stat = playerData.Stat;
    }
    
    public void IncreaseStat(CharacterStat itemStat)
    {
        stat.rclValue += itemStat.rclValue;
        stat.hdlValue += itemStat.hdlValue;
        stat.stpValue += itemStat.stpValue;
        stat.spdValue += itemStat.spdValue;
    }
}
