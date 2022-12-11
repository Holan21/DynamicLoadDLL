using System;
using System.Reflection;

namespace DynamicLoad
{
    public class Program
    {
        private static string _path = @"lib";
        public static void Main(string[] args)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_path);
            var _fullPath = dirInfo.FullName;

            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);

            string[] files = Directory.GetFiles(_fullPath + @"\" , "*.dll");

            foreach (string file in files)
            {
                var loadDLL = Assembly.LoadFile(@$"{file}");
                string[] strings = file.Split(@"\");
                Console.WriteLine("Invoke From:" + strings[strings.Length -1]);
 
                foreach (Type type in loadDLL.GetExportedTypes())
                {
                    var c = Activator.CreateInstance(type);

                    var writeInConsole = type.GetMethod("WriteString");
                    var getString = type.GetMethod("GetString");

                    Console.Write("   Result method " + writeInConsole.Name + ":");
                    writeInConsole.Invoke(c , new object[] { "Random Message"});

                    Console.Write("   Result method " + getString.Name + ":");
                    Console.WriteLine(getString.Invoke(c, null));
                }
            }
            Console.ReadKey();
        }
    }
}