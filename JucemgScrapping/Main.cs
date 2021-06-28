﻿using PuppeteerSharp;
using System;
using System.Collections.Generic;
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
            currentDate.Value = DateTime.Today.AddDays(-1);
        }

        private NavigationOptions _navigationOptions = new NavigationOptions { WaitUntil = new WaitUntilNavigation[] { WaitUntilNavigation.Networkidle0 } };

        private void Button1_Click(object sender, EventArgs e)
        {

            Thread threadFetch = new Thread(() =>
            {
                DisableButton();
                FetchData();
            });

            threadFetch.Start();
        }

        private async Task FetchData()
        {
            Browser browser = await LaunchBrowserAsync();
            try
            {
                var companies = new List<string>();
                string selectedDate = currentDate.Value.ToString();

                Page page = await browser.NewPageAsync();
                // await page.SetUserAgentAsync("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                SetCurrentOperation("Navegando para a página...");
                await page.GoToAsync("https://jucemg.mg.gov.br/atos");

                await page.TypeAsync("input[type=date]", selectedDate);
                await ReplaceText(page, "#tp_processo_id", "EXTINÇÃO");

                await page.WaitForSelectorAsync("#pesquisa_ato");
                await ClickAsync(page, "a#pesquisa_ato");

                await page.WaitForSelectorAsync("#table-result-search-acts2");

                short quantityToFetch = await GetCompaniesQuantity(page);

                var hasNextPage = false;
                do
                {
                    await GetCompaniesList(page, companies);
                    hasNextPage = await HasNextPage(page);
                    SetCurrentOperation("Extraindo dados...");
                    if (hasNextPage)
                    {
                        await ClickHyperlinkWithText(page, "próxima");
                    }
                    UpdateProggressLabel(companies, quantityToFetch);

                } while (hasNextPage);

                if (quantityToFetch != companies.Count)
                    throw new Exception("A quantidade extraída não foi compatível com a esperada");

                ExportToCsv(companies);
                MessageBox.Show($"Foram exportados {companies.Count} registros");                
                Reset();
                await browser.CloseAsync();
            }
            catch (Exception ex)
            {
                await browser.CloseAsync();
                MessageBox.Show($"Algo deu errado! {ex.Message}");
                Reset();
            }
        }

        private async Task<Browser> LaunchBrowserAsync(
            bool headless = true, ViewPortOptions viewPortOptions = null
            )
        {
            SetCurrentOperation("Iniciando o navegador...");
            var downloadPath = @"C:\PuppteerBrowser";
            var browserFetcherOptions = new BrowserFetcherOptions { Path = downloadPath };
            var browserFetcher = new BrowserFetcher(browserFetcherOptions);
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = headless,
                DefaultViewport = viewPortOptions,
                ExecutablePath = @"C:\PuppteerBrowser\Win64-848005\chrome-win\chrome.exe"
            });
            return browser;
        }

        private async Task<Browser> LaunchUserBrowserAsync(bool headless = true, ViewPortOptions viewPortOptions = null)
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = headless,
                ExecutablePath = @"%ProgramFiles%\Google\Chrome\Application\chrome.exe"
            });
            return browser;
        }

        private async Task ReplaceText(Page page, string selector, string replacementValue)
        {
            await page.WaitForSelectorAsync(selector).ConfigureAwait(false);
            await page.EvaluateExpressionAsync($"document.querySelector(\"{selector}\").value = \"{replacementValue}\"").ConfigureAwait(false);
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
            totalCompanies.Text = quantity.ToString();
            progressBar.Value = companies.Count;
            progressBar.Maximum = quantity;
        }

        private void SetCurrentOperation(string message)
        {
            Action<string> Update = (msg) => SetCurrentOperation(msg);

            if (currentOperation.InvokeRequired)
            {
                currentOperation.Invoke(Update, message);
                return;
            }

            currentOperation.Text = message;
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
            var ResetAction = new Action(() => Reset());

            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(ResetAction);
                return;
            }

            totalCompanies.ResetText();
            label1.ResetText();
            currentCompanies.ResetText();
            progressBar.Value = 0;
            currentOperation.ResetText();
            button1.Enabled = true;
        }

        private async Task<bool> HasNextPage(Page page)
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

        private void ExportToCsv(List<string> rows)
        {
            var exportPath = $@"{Environment.CurrentDirectory}\\exportacao-jucemg-{DateTime.Now:dd-MM-yyyy-HH_mm_ss_}.csv";
            File.WriteAllLines(exportPath, rows, Encoding.UTF8);
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
    }
}
