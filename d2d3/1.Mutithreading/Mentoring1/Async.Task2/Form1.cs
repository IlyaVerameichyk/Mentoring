using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Async.Task2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            if (!Uri.IsWellFormedUriString(text, UriKind.Absolute))
            {
                MessageBox.Show("Incorrect uri");
                return;
            }
            var cts = new CancellationTokenSource();
            var entry = new DownloadEntry
            {
                CancellationTokenSource = cts,
                Text = text
            };
            listBox1.Items.Add(entry);
            textBox1.Clear();
            await DownloadResource(new Uri(text), cts.Token, () => listBox1.Items.Remove(entry));
        }


        private async Task DownloadResource(Uri uri, CancellationToken token, Action deleteListEntry)
        {
            try
            {
                var content = await new HttpClient().GetAsync(uri, token);
                var path = Path.Combine(uri.Host, Path.GetFileName(Path.GetTempFileName())); //TODO: replace temp file name
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await content.Content.CopyToAsync(stream);
                }
                deleteListEntry();
            }
            catch (TaskCanceledException ex)
            {
                //TODO: log
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var item = (DownloadEntry)listBox1.SelectedItem;
            if (item == null) { return; }
            var cts = item.CancellationTokenSource;
            cts.Cancel();
            listBox1.Items.Remove(item);
        }
    }
}
