using System;
using System.IO;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

namespace DiscordAvatar
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private HttpClient client;

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bot BOT_TOKEN_HERE");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void IDField_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(idField.Text, @"^\d{18}"))
            {
                label1.ForeColor = Color.Red;
                label1.Text = "ID should contain only digits!";
                getAvatarButton.Enabled = false;
                return;
            }

            label1.ForeColor = Color.Green;
            label1.Text = "Valid ID!";
            getAvatarButton.Enabled = true;
        }

        private async void GetAvatarButton_Click(object sender, EventArgs e)
        {
            string content = client.GetStringAsync($"https://discord.com/api/v6/users/{idField.Text}").Result;
            DiscordUser userObject = JsonConvert.DeserializeObject<DiscordUser>(content);

            label2.Text = $"Avatar: {userObject.Username}#{userObject.Discriminator}";
            pictureBox1.LoadAsync($"https://cdn.discordapp.com/avatars/{idField.Text}/{userObject.AvatarHash}.png?size=1024");

            await DownloadAvatarLocally(userObject);
        }

        private async Task DownloadAvatarLocally(DiscordUser userObject)
        {
            var fs = new FileStream($"{userObject.AvatarHash}.png", FileMode.Create, FileAccess.Write);
            var imgStream = await client.GetStreamAsync($"https://cdn.discordapp.com/avatars/{idField.Text}/{userObject.AvatarHash}.png?size=1024");
            await imgStream.CopyToAsync(fs);

            imgStream.Close();
            fs.Close();
        }
    }
}
