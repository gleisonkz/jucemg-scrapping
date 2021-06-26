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
        private async void Button1_Click(object sender, EventArgs e)
        {
            string date = currentDate.Value.AddDays(-1).ToString("yyyy-MM-dd");
            _ = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://jucemg.mg.gov.br/atos");            
            await page.EvaluateFunctionAsync<dynamic>("(value)=> document.querySelector('input[type=date]').value = value",date);

        }
    }
}
