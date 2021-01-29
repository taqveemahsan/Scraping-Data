using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Scraping_Data.Models;
namespace Scraping_Data.Controllers
{
    public class HomeController : Controller
    {
        public IWebDriver driver;
        ScrapeTestEntities model = new ScrapeTestEntities();
        public ActionResult Index()
        {
            //string Hello = "Hi how are you My name is Taqi and i am a software Developer at Eazisols";
            //string[] array = Hello.Split(new char[] { ' ' }, 2);
            //var WordsArray = Hello.Split();
            //var num = WordsArray.Length;
            //string Items = WordsArray[0] + ' ' + WordsArray[1] + ' ' + WordsArray[2] + ' ' + WordsArray[3];
            return View();
        }
        
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index1(string Search_Value,string Search_sold= "")
        {
            ItemsName items = new ItemsName();
            List<string> NameProducts = new List<string>();
            List<string> sellerName = new List<string>();
            IList<IWebElement> namesList;
            IList<IWebElement> ItemSList;
            IList<IWebElement> location;
            IList<IWebElement> SellerList;
            IList<IWebElement> soldList;
            IWebDriver driver = new ChromeDriver(@"c:/");
            driver.Url = "https://www.aliexpress.com/";
            driver.Manage().Window.Minimize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.Id("search-key")).SendKeys(Search_Value + Keys.Enter);
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0,1080)");
            namesList = driver.FindElements(By.ClassName("item-title"));
            soldList = driver.FindElements(By.XPath("//*[@id='root']/div/div/div[2]/div[2]/div/div[2]/ul/div/li/div/div/div/div/div/span/a"));
            //if (soldList == null) { }
            //*[@id="root"]/div/div/div[2]/div[2]/div/div[2]/ul/div[2]/li[1]/div/div[2]/div/div[7]/div/span/a
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0,1080)");
            var numb = 1;
            var listNumber = 1;
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0,1080)");
            for (int i = 0; i < namesList.Count; i++)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                var curr = ((IJavaScriptExecutor)driver).ExecuteScript("return window.pageYOffset;").ToString();
                var curr1 = Int32.Parse(curr);
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(" + curr1 + "," + (curr1 + 900) + ")");
                Console.WriteLine(namesList[i].Text);
                namesList = driver.FindElements(By.ClassName("item-title"));
                soldList = driver.FindElements(By.XPath("//*[@id='root']/div/div/div[2]/div[2]/div/div[2]/ul/div/li/div/div/div/div/div/span/a"));
            }
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0,1080)");
            for (int s=0;s<soldList.Count;s++)
            {
                var curr = ((IJavaScriptExecutor)driver).ExecuteScript("return window.pageYOffset;").ToString();
                var curr1 = Int32.Parse(curr);
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(" + curr1 + "," + (curr1 + 900) + ")");
                //Console.WriteLine(namesList[i].Text);
                //soldList = driver.FindElements(By.XPath("sale-value-link"));
                var soldValue = soldList[s].Text;
                var soldValue1 = soldValue.ToString();

                //Find Number from string
                string b = string.Empty;
                var intValueSold = 1.00;

                for (int i = 0; i < soldValue1.Length; i++)
                {
                    if (Char.IsDigit(soldValue1[i]))
                        b += soldValue1[i];
                }

                if (b.Length > 0)
                    intValueSold = Int32.Parse(b);
                
                var ValueFromInput = Int32.Parse(Search_sold);
                
                if(intValueSold>= ValueFromInput)
                {
                    if (listNumber > 4)
                    {
                        numb++;
                        listNumber = 1;

                    }
                    IWebElement ValueName = driver.FindElement(By.XPath("//*[@id='root']/div/div/div[2]/div[2]/div/div[2]/ul/div[" + numb + "]/li[" + listNumber + "]/div/div[2]/div/div[1]/a"));
                    numb = (s / 4) + 1;
                    
                    var listLength = namesList.Count;
                    //namesList.Append(ValueName);
                    NameProducts.Add(ValueName.Text);
                    items.ItemsName1 = ValueName.Text;
                    listNumber++;
                    
                }
            }

            model.ItemsNames.Add(items);
            model.SaveChanges();
            

            //*[@id="root"]/div/div/div[2]/div[2]/div/div[2]/ul/div[1]/li[1]/div/div[2]/div/div[1]/a
            //*[@id="root"]/div/div/div[2]/div[2]/div/div[2]/ul/div[1]/li[2]/div/div[2]/div/div[1]/a
            //*[@id="root"]/div/div/div[2]/div[2]/div/div[2]/ul/div[2]/li[1]/div/div[2]/div/div[1]/a
            //*[@id="root"]/div/div/div[2]/div[2]/div/div[2]/ul/div[3]/li[1]/div/div[2]/div/div[1]/a
            //*[@id="root"]/div/div/div[2]/div[2]/div/div[2]/ul/div[15]/li[1]/div/div[2]/div/div[1]/a

            

            var total = namesList.Count;
            var totalSolids = soldList.Count;
            var listCount = NameProducts.Count;
            IWebDriver driver1=new ChromeDriver(@"c:/");
            driver1.Url = "https://www.ebay.com/";
            driver1.Manage().Window.Minimize();
            driver1.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            
            for (int i = 0; i < NameProducts.Count; i++)
            {
                //var name = namesList[i].Text;
                var name = NameProducts[i].ToString();
                var WordsArray = name.Split();
                string Items = WordsArray[0] + ' ' + WordsArray[1]+ ' ' + WordsArray[2] + ' ' + WordsArray[3];
                //IEnumerable<string> words = name.Split(new char[] { ' ' }, 2).Take(5);
                driver1.FindElement(By.Id("gh-ac")).SendKeys(Items + Keys.Enter);
                location = driver1.FindElements(By.ClassName("s-item__location"));
                for(int j=0;j<location.Count;j++)
                {
                    var curr = ((IJavaScriptExecutor)driver).ExecuteScript("return window.pageYOffset;").ToString();
                    var curr1 = Int32.Parse(curr);
                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(" + curr1 + "," + (curr1 + 900) + ")");
                    Console.WriteLine(location[j].Text);
                    location = driver1.FindElements(By.ClassName("s-item__location"));
                    
                }
                var locationItem = location.Count;
                for(int k=0;k<location.Count;k++)
                {
                    var curr = ((IJavaScriptExecutor)driver).ExecuteScript("return window.pageYOffset;").ToString();
                    var curr1 = Int32.Parse(curr);
                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(" + curr1 + "," + (curr1 + 900) + ")");
                    //Console.WriteLine(location[k].Text);
                    location = driver1.FindElements(By.ClassName("s-item__location"));
                    var locationName = location[k].Text;
                    locationName.ToString();
                    var num = k + 1;
                    if (locationName=="From China" || locationName == "De China")
                    {
                        try
                        {
                            driver1.FindElement(By.XPath("//*[@id='srp-river-results']/ul/li["+ num + "]/div/div[2]/a")).Click();
                            driver1.FindElement(By.ClassName("mbg-nw")).Click();
                            //*[@id="RightSummaryPanel"]/div[3]/div/div/div/div[1]/div[1]/a
                            var locationOfSeller = driver1.FindElement(By.ClassName("mem_loc"));
                            var TextSeller = locationOfSeller.Text;
                            var locationOfSellers = TextSeller.ToString();
                            if (locationOfSellers != "China")
                            {
                                IWebElement SellerName1= driver1.FindElement(By.ClassName("mbg-id"));
                                sellerName.Add(SellerName1.Text);
                                
                            }
                            driver1.Navigate().Back();
                            driver1.Navigate().Back();
                        }
                        catch(Exception e)
                        {
                            throw e;
                        }

                    }

                }
                 
                driver1.Url = "https://www.ebay.com/";
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public static async void scrapData()
        {
            var url = "https://www.ebay.com/sch/i.html?_from=R40&_trksid=p2499334.m570.l1313&_nkw=samsung&_sacat=15032";
            var HttpClient = new HttpClient();

            var html = await HttpClient.GetStringAsync(url);

            var HtmlDocument = new HtmlDocument();
            HtmlDocument.LoadHtml(html);

            var productHtml = HtmlDocument.DocumentNode.Descendants("ul")
                .Where(node => node.GetAttributeValue("class", "").Equals("srp-results srp-list clearfix")).ToList();

            var productListItems = productHtml[0].Descendants("li").Where(node => node.GetAttributeValue("class", "").Contains("s-item")).ToList();
        }

        public void Open_Browse()
        {
            //var driverService = ChromeDriverService.CreateDefaultService();
            //driverService.HideCommandPromptWindow = true;
            //driver = new ChromeDriver(driverService);

            try
            {
                driver.Navigate().GoToUrl("https://www.aliexpress.com/?spm=a2g0o.productlist.1000002.1.26ed399f1kvMa2");
            }
            catch
            {
                throw;
            }
        }
        public void find_Data()
        {
            var Message = "";
            //driver.Navigate().GoToUrl("https://www.aliexpress.com/?spm=a2g0o.productlist.1000002.1.26ed399f1kvMa2");
            ////driver.Url("https://www.aliexpress.com/?spm=a2g0o.productlist.1000002.1.26ed399f1kvMa2");
            IList<IWebElement> searchElements = driver.FindElements(By.TagName("tbody"));
            foreach(IWebElement i in searchElements)
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                var text = i.GetAttribute("innerHtml");
                htmlDocument.LoadHtml(text);
                var inputs = htmlDocument.DocumentNode.Descendants("ul").ToList();
                foreach(var items in inputs)
                {
                    HtmlDocument htmlDocument1 = new HtmlDocument();
                    htmlDocument1.LoadHtml(items.InnerHtml);
                    var lis = htmlDocument1.DocumentNode.Descendants("li").ToList();
                    if(lis.Count !=0 )
                    {
                        Message = lis[0].InnerText;
                    }
                }
            }
        }
        
        public async System.Threading.Tasks.Task<ActionResult> ScrapingExample()
        {
            var location = "";
            var sold = "";
            var NameString = "";
            ScrapEbay scrapEbay = new ScrapEbay();
            var url = "https://www.aliexpress.com/?spm=a2g0o.productlist.1000002.1.46603afbphaLi5";
            //Url for seller Page
            var urlForSeller = "https://www.ebay.com/usr/bigmoosewireless?_trksid=p2047675.l2559";

            var HttpClient = new HttpClient();

            var html = await HttpClient.GetStringAsync(url);
            //Load Html for seller Page
            var htmlSellerAccount = await HttpClient.GetStringAsync(urlForSeller);
            var htmldocumentforSeller = new HtmlDocument();
            htmldocumentforSeller.LoadHtml(htmlSellerAccount);

            //Load HTML for category id
            var HtmlDocument = new HtmlDocument();
            HtmlDocument.LoadHtml(html);

            //Loaction of seller
            var locationofSeller = htmldocumentforSeller.DocumentNode.Descendants("div")
                                 .Where(node => node.GetAttributeValue("id", "").Equals("member_info")).ToList();

            //var locationValue = locationofSeller[0].Descendants("span").Where(node => node.GetAttributeValue("class", "").Contains("mem_loc")).ToString();
            var locationValue1 = locationofSeller[0].SelectSingleNode(".//span[@class='mem_loc']").InnerText;
            var SellerLocation = locationValue1.ToString();
            //var sellerLocation = "";

            //List of Products From Ebay
            var productHtml = HtmlDocument.DocumentNode.Descendants("ul")
                .Where(node => node.GetAttributeValue("class", "").Equals("srp-results srp-list clearfix")).ToList();

            var productListItems = productHtml[0].Descendants("li").Where(node => node.GetAttributeValue("class", "").Contains("s-item")).ToList();
            //foreach (var items in locationValue)
            //{
            //    sellerLocation=items.Descendants("span")
            //        .Where(node => node.GetAttributeValue("class", "").Equals("mem_loc")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t');
            //}   


            foreach (var productslist in productListItems)
            {
                productslist.GetAttributeValue("data-view", "");

                scrapEbay.Name = productslist.Descendants("a")
                        .Where(node => node.GetAttributeValue("class", "").Equals("s-item__link")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t');

                NameString = productslist.Descendants("a")
                        .Where(node => node.GetAttributeValue("class", "").Equals("s-item__link")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t');

                scrapEbay.price = productslist.Descendants("span")
                        .Where(node => node.GetAttributeValue("class", "").Equals("s-item__price")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t');

                location = productslist.Descendants("span")
                         .Where(node => node.GetAttributeValue("class", "").Equals("s-item__location s-item__itemLocation")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t');
                //sold = productslist.Descendants("span")
                //        .Where(node => node.GetAttributeValue("class", "").Equals("s-item__hotness s-item__itemHotness")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t');
                location.ToString();
                string[] array = NameString.Split(new char[] { ' ' }, 2);

                //Find Number from string
                string b = string.Empty;
                var val = 1.00;

                for (int i = 0; i < sold.Length; i++)
                {
                    if (Char.IsDigit(sold[i]))
                        b += sold[i];
                }

                if (b.Length > 0)
                    val = float.Parse(b);
                var DummyValue = 800;

                if (DummyValue > 500)
                {
                    if (location == "From China")
                    {
                        if (SellerLocation != "China")
                        {
                            model.ScrapEbays.Add(scrapEbay);
                            model.SaveChanges();
                        }
                    }
                }
            }
            return View(productListItems);
        }

        
    }
}