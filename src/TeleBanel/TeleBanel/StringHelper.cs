namespace TeleBanel
{
    public static class StringHelper
    {
        public static string GetNetMessage(this string msg)
        {
            if (msg == null) return null;

            msg = msg.ToLower().Replace("/", "");
            var skipFirstLen = 0;
            foreach (var c in msg)
            {
                if (!char.IsLetterOrDigit(c))
                    skipFirstLen++;
                else break;
            }

            return msg.Substring(skipFirstLen);
        }
    }
}