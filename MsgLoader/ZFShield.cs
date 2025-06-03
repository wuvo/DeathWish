namespace TqShield
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using DeathWish;

    public static class _TqShield
    {
        public static List<String> BlockAppsList = new List<String>();
        public static String GameCryptographyKey = "A154mr20cod0IN2G";
        public static String arDx8File, arDx9File, arMTFile, arMEFile, arSRFile, arDLFile;
        public static String enDx8File, enDx9File, enMTFile, enMEFile, enSRFile, enDLFile;
        public static bool Validated(string language, String Dx8Conquer, String Dx9Conquer, String MagicType, String MagicEffect, String StrRes, String DLL8, String DLL9, out String FileChanged)
        {
            FileChanged = "";


            if (language == "en")
            {

                if (Dx8Conquer != enDx8File)
                {
                    FileChanged += "Conquer_8.exe ";
                }
                if (Dx9Conquer != enDx9File)
                {
                    FileChanged += "Conquer_9.exe ";
                }
                if (MagicType != enMTFile)
                {
                    FileChanged += "MagicType.dat ";
                }
                if (MagicEffect != enMEFile)
                {
                    FileChanged += "MagicEffect.ini ";
                }
                if (StrRes != enSRFile)
                {
                    FileChanged += "StrRes.ini ";
                }
                if (DLL8 != enDLFile)
                {
                    FileChanged += "NetShield_8.dll ";
                }
                if (DLL9 != enDLFile)
                {
                    FileChanged += "NetShield_9.dll ";
                }
            }
            else if (language == "ar")
            {
                if (Dx8Conquer != arDx8File)
                {
                    FileChanged += "Conquer_8.exe ";
                }
                if (Dx9Conquer != arDx9File)
                {
                    FileChanged += "Conquer_9.exe ";
                }
                if (MagicType != arMTFile)
                {
                    FileChanged += "MagicType.dat ";
                }
                if (MagicEffect != arMEFile)
                {
                    FileChanged += "MagicEffect.ini ";
                }
                if (StrRes != arSRFile)
                {
                    FileChanged += "StrRes.ini ";
                }
                if (DLL8 != arDLFile)
                {
                    FileChanged += "NetShield_8.dll ";
                }
                if (DLL9 != arDLFile)
                {
                    FileChanged += "NetShield_9.dll ";
                }
            }
            return FileChanged == "" ? true : false;
        }
        public static void LoadFilesAntiCheat()
        {

            LoadToList();

            // ar load files
            arDLFile = CalculateMD5(@"Files\ar\NetShield.dll");
            arDx8File = CalculateMD5(@"Files\ar\Conquer_8.exe");
            arDx9File = CalculateMD5(@"Files\ar\Conquer_9.exe");
            arSRFile = CalculateMD5(@"Files\ar\ini\StrRes.ini");
            arMTFile = CalculateMD5(@"Files\ar\ini\magictype.dat");
            arMEFile = CalculateMD5(@"Files\ar\ini\MagicEffect.ini");

            // en load files
            enDLFile = CalculateMD5(@"Files\en\NetShield.dll");
            enDx8File = CalculateMD5(@"Files\en\Conquer_8.exe");
            enDx9File = CalculateMD5(@"Files\en\Conquer_9.exe");
            enSRFile = CalculateMD5(@"Files\en\ini\StrRes.ini");
            enMTFile = CalculateMD5(@"Files\en\ini\magictype.dat");
            enMEFile = CalculateMD5(@"Files\en\ini\MagicEffect.ini");
            MyConsole.WriteLine("Loaded all file Anticheat", ConsoleColor.DarkYellow);

        }
        private static void LoadToList()
        {
            BlockAppsList.Clear();
            var Lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "//BannedAppList.txt");
            for (Int32 FL = 0; FL < Lines.Length; FL++)
            {
                var line = Lines[FL].Trim();
                if (!BlockAppsList.Contains(line.ToLower()))
                {
                    BlockAppsList.Add(line.ToLower());
                }
            }
        }
        public static string RemoveIllegalCharacters(this string str, bool path, bool file)
        {
            string myString = str;
            if (file)
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                    myString = myString.Replace(c, '_');
            if (path)
                foreach (char c in System.IO.Path.GetInvalidPathChars())
                    myString = myString.Replace(c, '_');
            return myString;
        }
        private static string CalculateMD5(string filename)
        {
            try
            {
                using (var THash = MD5.Create())
                {
                    using (var Stream = File.OpenRead(filename))
                    {
                        var Hash = THash.ComputeHash(Stream);
                        return BitConverter.ToString(Hash).Replace("-", "").ToLowerInvariant();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating MD5 for {filename}: {ex}");
                return string.Empty;
            }
        }
    }
}
