using DataDeclaration;

public class PlayerStatHandler
{
    private Player player;
    private CharacterStat stat;
    
    public CharacterStat Stat => stat;

    public PlayerStatHandler(Player player)
    {
        this.player = player;
        stat = new CharacterStat
        {
            HDL = this.player.Data.HDL,
            SPD = this.player.Data.SPD,
            RCL = this.player.Data.RCL,
            STP = this.player.Data.STP
        };
    }
    
    public void IncreaseStat(CharacterStat itemStat)
    {
        stat.RCL += itemStat.RCL;
        stat.HDL += itemStat.HDL;
        stat.STP += itemStat.STP;
        stat.SPD += itemStat.SPD;
    }
}
