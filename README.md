This solution was created bassed test assignement for one company:

<h1>Assignment</h1>
Create an application to download exchange rates from the CNB to enable currency conversion and simple export of current rates. 
The application can be conceived as a mobile application, a web service(REST API), or a desktop application. 

<h2>Functional Requirements</h2>

* **Download CNB Currencies**, Manually Invoked, from: http://www.cnb.cz/en/financial_trade/default_trh/default_course/denni_kurz.txt
* **Currency Conversion Interface** (Source currency and Amount Specified, Target currency) 
* **Simple export of current rates into a file** (better formatted than the TXT file provided by the CNB or possibly a completely different format, such as HTML, DOCX, XLSX, ..., **XML**) 

<h2>Nonfunctional Requirements</h2>

* Written in C # language 
* Developed in one of the technologies: WinRT, UWP, WPF, ASP.NET WebApi, ASP.NET MVC, **ASP.NET WebApi CORE**
* In the case of a mobile / desktop application, the frontend will be written in XAML 
* Rates will be stored in the internal database (just enough data structure in memory) to work 
* Stability, to handle all possible alternate flows and error states 
* Powerful and effective code 
* At least basic architecture to ensure clarity and eventual extensibility 
* Sources code, which, if appropriate, will be commented on 
* May contain unit tests (optional)

<h1>Solution</h1>
One ASP.NET WebApi CORE with three REST API endpoint:

* **PATH** on **/api/ExchangeRates** to force donwload new denni_kurz.txt
* **GET** **/api/ExchangeRates/download** with header **ACCEPT:application/xml** to get xml formatted response consisting of all rates
* **GET** **/api/ExchangeRates?ciloveMena=USD&zdrojovaMena=CZK&mnozstvi=1** to convert currency from source (cilovaMena) to target (cilovaMena) currency with specified amount(mnozstvi)

One sample client application which shows usage of all endpoints.

One unit test project to test the currency converting logic.



