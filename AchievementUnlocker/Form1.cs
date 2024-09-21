using JRPCPlusPlus;
using System.Diagnostics;
using XDevkit;

namespace AchievementUnlocker
{
    public partial class Form1 : Form
    {
        IXboxConsole console;
        bool connected = false;

        private uint LocateInstruction()
        {
            byte[] targetSequence = { 0x60, 0x84, 0x00, 0x08 };
            byte[] bytes = console.GetMemory(0x82000000, 0x3000000);

            for (int i = 0; i <= bytes.Length - targetSequence.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < targetSequence.Length; j++)
                {
                    if (bytes[i + j] != targetSequence[j])
                    {
                        match = false;
                        break;
                    }
                }
                uint address = 0x82000000 + (uint)i;
                if (match)
                {
                    uint addr2 = address - 0x8;
                    byte[] testBytes = console.GetMemory(addr2, 4);
                    byte[] target2 = { 0x38, 0xE0, 0x00, 0x08 };
                    if (testBytes.Length == target2.Length && testBytes.SequenceEqual(target2))
                    {
                        return address;
                    }
                }
            }
            return 0;
        }

        private void XUserWriteAchievements(uint achievementCallAddr, uint achievementIdPtr, uint xoverlappedPtr, int achievementCount)
        {
            console.WriteUInt32(xoverlappedPtr, 0);

            for (int i = 0; i < achievementCount; i++)
            {
                console.WriteUInt64(achievementIdPtr, (ulong)i);
                console.CallVoid(achievementCallAddr, 1, achievementIdPtr, xoverlappedPtr);

                while (console.ReadUInt32(xoverlappedPtr) != 0) Thread.Sleep(30);
            }
            Thread.Sleep(250);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!console.Connect(out console))
                    MessageBox.Show("Failed to connect!");
                else
                    connected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (connected)
                {
                    uint callAddr = LocateInstruction() - 0x24;
                    if (callAddr != 0)
                        XUserWriteAchievements(callAddr, 0x91D0D9E0, 0x91D0D9E8, 350); // i set the count high because the id's are not always in sequence. better chance to get all of them
                    else
                        MessageBox.Show("Failed to find the function address!");
                }
                else
                    MessageBox.Show("Not connected to console!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://github.com/XeCrippy/AchievementUnlocker";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}
