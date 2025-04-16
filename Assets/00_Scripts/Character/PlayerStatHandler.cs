using DataDeclaration;

public class PlayerStatHandler
{
    private CharacterStat stat;

    public CharacterStat Stat => stat;

    public PlayerStatHandler(Player player)
    {
        stat = new CharacterStat
        {
            HDL = player.Data.HDL,
            SPD = player.Data.SPD,
            RCL = player.Data.RCL,
            STP = player.Data.STP
        };
    }

    public void IncreaseStat(float rcl, float hdl, float stp, float spd)
    {
        stat.RCL += rcl;
        stat.HDL += hdl;
        stat.STP += stp;
        stat.SPD += spd;
    }
    
    //----------------------------------------------------------
    // 테스트 코드
    public PlayerStatHandler(CharacterSO data)
    {
        stat = new CharacterStat
        {
            HDL = data.HDL,
            SPD = data.SPD,
            RCL = data.RCL,
            STP = data.STP
        };
    }
}