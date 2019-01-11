using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace demo_console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Url = "http://www.google.com";
                System.Threading.Thread.Sleep(5000);
                Console.WriteLine(driver.Title);
                IWebElement query = driver.FindElement(By.TagName("body"));
                Console.WriteLine(query.Text);
                var strs = query.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                var innerHtml = query.GetAttribute("innerHTML");
                driver.Close();

                //driver.Url = "http://www.google.com";
                //System.Threading.Thread.Sleep(5000);
                //Console.WriteLine(driver.Title);                
                //var lstParent = driver.FindElements(By.XPath("//div[@class='w-box w-box-blue']"));
                //foreach (var itemParent in lstParent)
                //{
                //    var item = itemParent.FindElement(By.XPath("//code[@class='cs undefined']"));
                //    if (item != null)
                //    {
                //        Console.WriteLine(item.Text);
                //    }
                //}                
                //driver.Close();
            }
        }
    }
}
