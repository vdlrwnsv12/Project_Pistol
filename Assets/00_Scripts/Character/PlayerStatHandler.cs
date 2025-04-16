using DataDeclaration;
using UnityEngine;

public class PlayerStatHandler
{
    private Player player;
    private CharacterStat stat;
    
    public CharacterStat Stat => stat;
    public float MovementSpeed => player.Data.SPD;

    public PlayerStatHandler(Player player)
    {
        this.player = player;
        stat = new CharacterStat
        {
            HDL = player.Data.HDL,
            SPD = player.Data.SPD,
            RCL = player.Data.RCL,
            STP = player.Data.STP
        };
        Debug.Log($"PlayerStatHandler Initialized: SPD = {stat.SPD}");
    }
    
    public void IncreaseStat(CharacterStat itemStat)
    {
        stat.RCL += itemStat.RCL;
        stat.HDL += itemStat.HDL;
        stat.STP += itemStat.STP;
        stat.SPD += itemStat.SPD;
        Debug.Log($"Stats Increased: SPD = {stat.SPD}");
    }
}
