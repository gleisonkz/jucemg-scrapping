using CsvHelper;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
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

        private static NavigationOptions _navigationOptions = new NavigationOptions { WaitUntil = new WaitUntilNavigation[] { WaitUntilNavigation.Networkidle0 } };

        [Obsolete]
        private void Button1_Click(object sender, EventArgs e)
        {

            Thread threadFetch = new Thread(() =>
            {
                DisableButton();
                _ = FetchData();
            });

            threadFetch.Start();
        }

        private async Task FetchData()
        {
            Browser browser = await LaunchBrowserAsync(false);
            try
            {
                string date = currentDate.Value.AddDays(-1).ToString();

                var page = await browser.NewPageAsync();
                await page.GoToAsync("https://jucemg.mg.gov.br/atos");

                // await page.EvaluateFunctionAsync<dynamic>("(value)=> document.querySelector('input[type=date]').value = value", date);
                await page.TypeAsync("input[type=date]", date);

                var companies = new List<string>();


                await page.TypeAsync("#tp_processo_id", "EXTINÇÃO");

                await page.WaitForSelectorAsync("#pesquisa_ato");
                await ClickAsync(page, "a#pesquisa_ato");
                await page.WaitForSelectorAsync("#table-result-search-acts2");

                short quantity = await GetCompaniesQuantity(page);

                //var elements = await page.XPathAsync("//a[contains(text(), 'próxima')]");

                do
                {
                    UpdateProggressLabel(companies, quantity);
                    await GetCompaniesList(page, companies);
                    ClickHyperlinkWithText(page, "próxima");

                } while (await HasNext(page));

                if (await HasNext(page))
                {



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
                Reset();
            }
        }

        private static async Task<Browser> LaunchBrowserAsync(
            bool headless = true, ViewPortOptions viewPortOptions = null
            )
        {
            var downloadPath = @"C:\\PuppteerBrowser";
            var browserFetcherOptions = new BrowserFetcherOptions { Path = downloadPath };
            var browserFetcher = new BrowserFetcher(browserFetcherOptions);
            _ = await browserFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = headless,
                DefaultViewport = viewPortOptions
            });
            return browser;
        }

        private void UpdateProggressLabel(List<string> companies, short quantity)
        {
            Action<List<string>, short> Update = (c, q) => UpdateProggressLabel(c, q);

            if (currentCompanies.InvokeRequired)
            {
                currentCompanies.Invoke(Update, new object[] { companies, quantity });
                return;
            }

            currentCompanies.Text = companies.Count.ToString();
            progressBar.Value = companies.Count;
            totalCompanies.Text = quantity.ToString();
            progressBar.Maximum = quantity;
        }

        private void DisableButton()
        {
            var Disable = new Action(() => DisableButton());

            if (button1.InvokeRequired)
            {
                button1.Invoke(Disable);
                return;
            }

            button1.Enabled = false;
        }

        private void Reset()
        {
            var ResetAction  = new Action(() => Reset());

            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(ResetAction);
                return;
            }
            
            progressBar.Maximum = 0;
            progressBar.Value = 0;
            button1.Enabled = true;
        }


        private async Task<bool> HasNext(Page page)
        {
            var elements = await page.XPathAsync("//a[contains(text(), 'próxima')]");
            return elements.Length > 0;
        }

        private async Task<short> GetCompaniesQuantity(Page page)
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

        private async Task GetCompaniesList(Page page, List<string> rows)
        {
            string js = @" () => {
              const rows = [...document.querySelectorAll('#table-result-search-acts2 tbody tr')];
              return rows.map((row) =>
                [...row.children].map((tableData) => tableData.textContent)
              );
            }";

            var companiesRows = await page.EvaluateFunctionAsync<dynamic>(js);

            foreach (var company in companiesRows)
            {
                string line = "";
                foreach (var column in company)
                {
                    line += column.Value + ",";
                }
                rows.Add(line);
            }
        }

        private static void ExportToCsv(List<string> rows)
        {
            var exportFilePath = Path.Combine(
                       Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                       DateTime.Now.Date.ToString("yyyy_MM_dd"),
                       ".csv");

            File.WriteAllLines(exportFilePath, rows, Encoding.UTF8);
            MessageBox.Show($"Arquivo exportado");
        }

        private async Task ClickAsync(Page page, string selector)
        {
            await page.WaitForSelectorAsync(selector).ConfigureAwait(false);
            await page.EvaluateExpressionAsync($"document.querySelector(\"{selector}\").click()").ConfigureAwait(false);
        }

        private async Task ClickHyperlinkWithText(Page page, string hyperlinkText)
        {
            var aElementsWithRestful = await page.XPathAsync($"//a[contains(text(), '{hyperlinkText}')]");
            if (aElementsWithRestful.Length < 1)
                throw new Exception($"A hyperlink with text: {hyperlinkText} was not found");

            var navigationTask = page.WaitForNavigationAsync(_navigationOptions);
            var clickTask = aElementsWithRestful[0].ClickAsync();
            await Task.WhenAll(navigationTask, clickTask);
        }

        private async Task ClickElementWithXPathAndWaitForXPath(
            Page page,
            string clickOnXpathExpression,
            string waitForXpathExpression)
        {
            var aElementsWithRestful = await page.XPathAsync(clickOnXpathExpression);
            if (aElementsWithRestful.Length < 1)
                throw new Exception($"A element with expression: {clickOnXpathExpression} was not found");

            var navigationTask = page.WaitForXPathAsync(waitForXpathExpression);
            var clickTask = aElementsWithRestful[0].ClickAsync();
            await Task.WhenAll(navigationTask, clickTask);
        }
    }
}
