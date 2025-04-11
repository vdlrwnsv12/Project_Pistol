using DataDeclaration;

public class PlayerStatHandler
{
    private Player player;
    private CharacterStat stat;

    public PlayerStatHandler(Player player)
    {
        this.player = player;
        stat = player.Data.Stat;
    }
}
