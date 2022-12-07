using System.IO;
using System.Text;

namespace SimAI.Core.Extensions {
    public static class ByteExtensions {
        public static string ConvertToString(this byte[] bytes) {
            if (bytes == null) {
                return string.Empty;
            }

            using var stream = new MemoryStream(bytes);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}
