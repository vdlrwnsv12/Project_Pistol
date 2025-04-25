using UnityEngine;

public class PlayerStatHandler
{
    private float rcl;
    private float hdl;
    private float stp;
    private float spd;

    private float finalRCL;

    public float RCL
    {
        get => rcl;
        private set => rcl = Mathf.Clamp(value, 0, 99);
    }
    public float HDL
    {
        get => hdl;
        private set => hdl = Mathf.Clamp(value, 0, 99);
    }
    public float STP
    {
        get => stp;
        private set => stp = Mathf.Clamp(value, 0, 99);
    }
    public float SPD
    {
        get => spd;
        private set => spd = Mathf.Clamp(value, 0, 99);
    }

    public PlayerStatHandler(Player player)
    {
        RCL = player.Data.RCL;
        HDL = player.Data.HDL;
        STP = player.Data.STP;
        SPD = player.Data.SPD;
    }

    public void IncreaseStat(float rclValue, float hdlValue, float stpValue, float spdValue)
    {
        RCL += rclValue;
        HDL += hdlValue;
        STP += stpValue;
        SPD += spdValue;
    }
}