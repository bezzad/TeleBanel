using System;
using System.Drawing;

namespace TeleBanel.Test
{
    public static class Helper
    {
        public static byte[] ToByte(this Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
