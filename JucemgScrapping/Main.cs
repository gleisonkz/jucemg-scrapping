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
            _ = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                DefaultViewport = null
            });
            try
            {
                string date = currentDate.Value.AddDays(-1).ToString("yyyy-MM-dd");
                var page = await browser.NewPageAsync();
                await page.GoToAsync("https://jucemg.mg.gov.br/atos");
                await page.EvaluateFunctionAsync<dynamic>("(value)=> document.querySelector('input[type=date]').value = value", date);

                var companies = new List<string>();

                //await ReplaceText(page, "#tp_processo_id", "EXTINÇÃO");
                await page.TypeAsync("#tp_processo_id", "EXTINÇÃO");

                await page.WaitForSelectorAsync("#pesquisa_ato");
                await ClickAsync(page, "a#pesquisa_ato");
                await page.WaitForSelectorAsync("#table-result-search-acts2");

                short quantity = await GetCompaniesQuantity(page);
                
                //var elements = await page.XPathAsync("//a[contains(text(), 'próxima')]");
                
                if (await HasNext(page))
                {
                    UpdateProggressLabel(companies, quantity);
                    dynamic text = await GetCompaniesText(page);


                    //var nextButton = elements[0];
                    //await nextButton.ClickAsync();
                    //var jsHandle = await element.GetPropertyAsync("value");
                    //var text = await jsHandle.JsonValueAsync<string>();
                };

                MessageBox.Show($"{quantity}");
                await browser.CloseAsync();
            }
            catch (Exception ex)
            {
                await browser.CloseAsync();
                MessageBox.Show($"Algo deu errado! {ex.Message}");
            }




        }

        private void UpdateProggressLabel(List<string> companies, short quantity)
        {
            currentCompanies.Text = companies.Count.ToString();
            totalCompanies.Text = quantity.ToString();
        }

        private static async Task<bool> HasNext(Page page)
        {
            var elements = await page.XPathAsync("//a[contains(text(), 'próxima')]");
            return elements.Length > 0;
        }

        private static async Task<short> GetCompaniesQuantity(Page page)
        {
            string js = @" () => {
                const $table = document.querySelector('#table-result-search-acts2');
                const $childNodes = [...$table.parentElement.childNodes];
                const $textNodesWithValues = $childNodes
                  .filter((c) => c.nodeType === Node.TEXT_NODE)
                  .filter((c) => c.textContent.trim());
                const text = $textNodesWithValues[0].textContent.trim();
                const [_, quantity] = text.split(' ').map(Number).filter(Boolean);
                return quantity;
            }";
            var someObject = await page.EvaluateFunctionAsync<dynamic>(js);
            return (short)someObject.Value;
        }

        private static async Task<dynamic> GetCompaniesText(Page page)
        {
            string js = @" () => {
              const rows = [...document.querySelectorAll('#table-result-search-acts2 tbody tr')];
              return rows.map((row) =>
                [...row.children].map((tableData) => tableData.textContent)
              );
            }";
            var someObject = await page.EvaluateFunctionAsync<dynamic>(js);
            return someObject.Value;
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
