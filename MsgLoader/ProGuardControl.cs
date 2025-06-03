using DeathWish.ServerSockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeathWish
{
    public partial class ProGuardControl : Form
    {
        public ProGuardControl()
        {
            InitializeComponent();
        }
        private void ProGuardControl_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (var user in DeathWish.Database.Server.GamePoll.Values)
            {
                comboBox1.Items.Add(user.Player.Name);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (var user in DeathWish.Database.Server.GamePoll.Values)
            {
                comboBox1.Items.Add(user.Player.Name);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (var user in DeathWish.Database.Server.GamePoll.Values)
            {
                if (user.Player.Name.ToLower() == comboBox1.Text.ToLower())
                {
                    DeathWish.Database.SystemBannedAccount.AddBan(user.Player.UID, user.Player.Name, uint.Parse(textBox1.Text));
                    DeathWish.Database.SystemBannedAccount.Save();
                    user.Socket.Disconnect();
                    break;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string pro = "";
            foreach (var user in DeathWish.Database.Server.GamePoll.Values)
            {
                if (user.Player.Name.ToLower() == comboBox1.Text.ToLower())
                {
                    pro = user.TqSerial;
                    DeathWish.Database.SystemBanned.AddBan(user.TqSerial, uint.Parse(textBox1.Text));
                    DeathWish.Database.SystemBanned.Save();
                    break;
                }
            }
            foreach (var user in DeathWish.Database.Server.GamePoll.Values)
            {
                if (user.TqSerial.ToLower() == pro.ToLower())
                {
                    user.Socket.Disconnect();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            foreach (var user in DeathWish.Database.SystemBanned.BannedPoll.Values)
            {
                comboBox2.Items.Add(user.IP);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string mac = comboBox2.Text;
            DeathWish.Database.SystemBanned.BannedPoll.Remove(comboBox2.Text);
            DeathWish.Database.SystemBanned.Save();
            comboBox2.Text = "";
            button6_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var user in DeathWish.Database.Server.GamePoll.Values)
            {
                if (user.Player.Name.ToLower() == comboBox1.Text.ToLower())
                {
                    using (var Recycled = new RecycledPacket())
                    {
                        var cPacket = Recycled.GetStream();
                        string Message = "you dissconnect By GM  ";

                        user.Send(DeathWish.Game.MsgLoader.CheatPacket.SendClosePacket(cPacket, 40, Message));
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var user in DeathWish.Database.Server.GamePoll.Values)
            {
                if (user.Player.Name.ToLower() == comboBox1.Text.ToLower())
                {
                    Scanlist.Items.AddRange(user.OpenedProcesses.ToArray());

                    foreach (var foo in user.OpenedProcesses)
                    {
                        Scanlist.Items.Add(foo);
                    }
                    using (var Recycled = new RecycledPacket())
                    {
                        var cPacket = Recycled.GetStream();
                        user.Send(DeathWish.Game.MsgLoader.CheatPacket.SendProPacket(cPacket, TqShield.Types.SubType.ProcessesCheck));
                    }
                }
            }
        }
    }
}
