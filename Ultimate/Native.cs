using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Ultimate
{
    public unsafe class Native
    {
        //[DllImport("kernel32.dll")]
        //public static extern IntPtr GetStdHandle(uint nStdHandle);

        //[DllImport("kernel32.dll")]
        //public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);

        //[DllImport("msvcrt.dll")]
        //public static extern unsafe void* memcpy(void* dest, void* src, uint size);

        //[DllImport("user32.dll")]
        //public static extern int MessageBox(int h, string m, string c, int type);

        [DllImport("winmm.dll")]
        public static extern uint timeGetTime();

        [DllImport("libeay32.dll")]
        public extern static void BF_set_key(IntPtr _key, int len, byte[] data);

        [DllImport("libeay32.dll")]
        public extern static void BF_ecb_encrypt(byte[] in_, byte[] out_, IntPtr schedule, int enc);

        [DllImport("libeay32.dll")]
        public extern static void BF_cbc_encrypt(byte[] in_, byte[] out_, int length, IntPtr schedule, byte[] ivec, int enc);

        [DllImport("libeay32.dll")]
        public extern static void BF_cfb64_encrypt(byte[] in_, byte[] out_, int length, IntPtr schedule, byte[] ivec, ref int num, int enc);

        [DllImport("libeay32.dll")]
        public extern static void BF_ofb64_encrypt(byte[] in_, byte[] out_, int length, IntPtr schedule, byte[] ivec, out int num);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();
    }
}
