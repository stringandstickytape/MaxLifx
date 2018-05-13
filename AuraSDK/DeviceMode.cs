namespace AuraSDKDotNet
{
    public enum DeviceMode : int
    {
        /// <summary>
        /// Default mode. The device does whatever it wants.
        /// </summary>
        EC = 0,

        /// <summary>
        /// Software control mode. Device LEDs are controlled exclusively via software.
        /// </summary>
        Software = 1
    }
}
