using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Process.NET.Memory;

namespace Process.NET.Test
{
    [TestFixture]
    public class ChuckTest
    {
        public string GetHexStringFromByteArray(byte[] array)
        {
            if (array != null)
            {
                var temp = array.Select(x => x.ToString("X2"));
                string str = string.Join(" ", temp);
                return str;
            }

            return string.Empty;
        }

        [Test]
        public void StringToByteArray()
        {
            string str = "hello world";
            var array = Encoding.Unicode.GetBytes(str);
            var hexString = GetHexStringFromByteArray(array);
            Console.WriteLine(hexString);
            //68 00 65 00 6C 00 6C 00 6F 00 20 00 77 00 6F 00 72 00 6C 00 64 00
        }

        /// <summary>
        /// https://codingvision.net/security/c-read-write-another-process-memory
        /// </summary>
        [Test]
        public void NotePadTest()
        {
            var processName = "notepad";
            var processes = System.Diagnostics.Process.GetProcessesByName(processName);
            if (processes.Length == 0)
            {
                Console.WriteLine($"Can not find process by name {processName}");
                return;
            }
            ProcessSharp processSharp = new ProcessSharp(processes[0], MemoryType.Remote);

            //0x000000B9A6B78542
            IntPtr intPtr = new IntPtr(0x000000B9A6B78542);
            string str = "hello";
            var length = str.Length * 2; //'hello' takes *2 bytes because of Unicode 
            var byteArray = processSharp.Memory.Read(intPtr, length);
            var hexString = GetHexStringFromByteArray(byteArray);
            Console.WriteLine(hexString);
            var text = Encoding.Unicode.GetString(byteArray);
            Console.WriteLine(text);

            //0x00000263C09E3A8C
            IntPtr intPtr2 = new IntPtr(0x00000263C09E3A8C);
            string str2 = "world";
            var length2 = str2.Length * 2;
            var byteArray2 = processSharp.Memory.Read(intPtr2, length2);
            var hexString2 = GetHexStringFromByteArray(byteArray2);
            Console.WriteLine(hexString2);
            var text2 = Encoding.Unicode.GetString(byteArray2);
            Console.WriteLine(text2);
        }

    }
}
