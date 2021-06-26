using PuppeteerSharp;
using System;
using System.Collections.Generic;
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
                Headless = false,
                DefaultViewport = null
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://jucemg.mg.gov.br/atos");
            await page.EvaluateFunctionAsync<dynamic>("(value)=> document.querySelector('input[type=date]').value = value", date);

            var companies = new List<string>();

            await ReplaceText(page, "#tp_processo_id", "EXTINÇÃO");
            await page.WaitForSelectorAsync("#pesquisa_ato");
            ClickAsync(page,"a#pesquisa_ato");
            //await page.ClickAsync("a#pesquisa_ato");
        }

        private static async Task ReplaceText(Page page, string selector, string replacementValue)
        {
            await page.WaitForSelectorAsync(selector).ConfigureAwait(false);
            await page.EvaluateExpressionAsync($"document.querySelector(\"{selector}\").value = \"{replacementValue}\"").ConfigureAwait(false);
        }

        private static async Task ClickAsync(Page page, string selector)
        {
            await page.WaitForSelectorAsync(selector).ConfigureAwait(false);
            await page.EvaluateExpressionAsync($"document.querySelector(\"{selector}\").click()").ConfigureAwait(false);
        }
    }
}
