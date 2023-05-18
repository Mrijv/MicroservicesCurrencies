Welcome! Enjoy:

1. Setup a database running migrations - I've choosed SQLServer.
2. First execution of the app will take more time as it seeds the database with the newly retrieved data from Exchange Rates Data API (it has 250 requests/month and 177 is left for your tests). Please check out my implementation in Seed folder and in Program.cs using{} block.
3. On execution of the app the catching handler starts - please check out Caching folder (answer to first bonus question). It removes data which is older than 30 minutes - interval is set to 30minutes but it can be changed so I additionally implement this scenario: when a user makes a trade and a currency exchange rate is older than 30min then it calls third-party API and updats the database (catch here) too. Check out MakeTrade() method of TradeService.cs.
4. Integrating rate provider you can check out under https://localhost:7020/swagger/index.html and its implementations in Controllers folder.
5. If sth go wrong check if third-party API's servers are up. If yes, then check out Log folder and read logs comments.
Althought I'm open to have a call and make small presentation if you encounter some obstacles.
6. Logging - I used Serilog and you can change configuration just by modifying appsettings.json.
7. Unit tests - I've written only 2 tests but I had chosen the most sofisticated part of codes where I had to mock HttpClient . To do so, I used RichardSzalay.MockHttp nuget packege. In general I use xUnit and Shouldly package for Assetions.

I hope You will like my code and I'm looking forward to your feedback and hopefully next step of the interview.
Best regards
Maria Nawrocka


