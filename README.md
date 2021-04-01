# PortfolioManager

## Features

- View the list of Clients
- View Portfolio of individual client
    - High level summary of net worth, asset and liabilities
    - A graph showing the net asset growth over the last 10 days
    - A chart showing the net asset contribution from each asset entity
    - Profit and Loss calculation from equity
    - List of equity transactions made by the client
    - Download the profit and loss report as CSV file

## Architecture

- The backend is built using .net framework 4.8 MVC
- Thers is no authentication feature added to make the app simple
- The web layer is using Razor syntax for simplicity
- The app uses file based SQL server database and the app uses dapper as ORM
- nuget package manager is used for dependency library management
- The test runner choosen is NUnit
- Autofac is used for IOC
- Logging is done using Serilog

