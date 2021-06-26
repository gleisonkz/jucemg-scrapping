using PuppeteerSharp;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JucemgScrapping
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        [Obsolete]
        private async void button1_Click(object sender, EventArgs e)
        {
            string date = currentDate.Value.ToString("yyyy-MM-dd");
            _ = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync("http://www.google.com");

        }
    }
}
