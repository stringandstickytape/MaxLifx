using System;

namespace AuraSDKDotNet
{
    public abstract class AuraDevice
    {
        /// <summary>
        /// Number of color zones that can be controlled.
        /// </summary>
        public int LedCount { get => ledCount; }

        protected AuraSDK sdk;
        protected IntPtr handle;
        protected int ledCount;

        internal AuraDevice(AuraSDK sdk, IntPtr handle)
        {
            this.sdk = sdk;
            this.handle = handle;
        }

        /// <summary>
        /// Set the device's RGB mode (currently only default/automatic or software).
        /// </summary>
        /// <param name="mode"></param>
        public abstract void SetMode(DeviceMode mode);
        
        /// <summary>
        /// Set the device's colors. There must be the same number of colors as there are zones on the device.
        /// </summary>
        /// <param name="colors">Colors of the different zones</param>
        public abstract void SetColors(Color[] colors);

        /// <summary>
        /// Set the device's colors. There must be the same number of colors as there are zones on the device times 3 bytes.
        /// </summary>
        /// <param name="colors">Colors of the different zones as bytes in RBG order</param>
        public abstract void SetColors(byte[] colors);
    }
}
