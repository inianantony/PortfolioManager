# PortfolioManager

## Features

- View the list of Clients
- View Portfolio of individual client
    - High level summary of net worth, assets worth and liabilities
    - A graph showing the net asset growth over the last 10 days
    - A chart showing the net asset contribution from each equity
    - Equity's Profit and Loss calculation
    - List of equity transactions made by the client
    - Download the profit and loss report as CSV file

## Architecture

- The backend is built using `.net framework 4.8 MVC`
- The app is architectured using `Domain Centric Design` in mind
- Thers is `no authentication` feature added to make the app simple
- The web layer is using `Razor` syntax for simplicity
- The app uses `file based SQL server` database and the app uses `dapper` as ORM
- `nuget` package manager is used for dependency library management
- The test runner choosen is `NUnit`
- `Autofac` is used for IOC
- Logging is done using `Serilog`

## Project Structure

### PortfolioManager

- The static files are located in the `StaticFiles` folder
- `Models` folder gives home to all the data model 
- The services are present in `Services` folder
- The database connectivity realted scope resides in `Repository` folder
- The file database resides in `App_Data` folder
- The `App_Start` folder contains the application config classes
- The `Controllers` and `Views` follow the same convention based on the MVC concept
- `Cache` folders holds the cache of equity prices

### PortfolioManagerTests

- `Models` folder hosts the test of models
- The services tests are present in `Services` folder
- More tests will be added based on the similar structure of main project

## How to run the project?

- Use visual studio 2019 to open the app
- Restore the nuget packages by right clicking the solution file and choose the menu `Restore Nuget Packages`
- Build the solution
- Click `F5` to run the project
- The default browser opens with the app running in debugging mode backed up by IIS Express

## How to run the tests?

- Use reshrper to run the tests in Visual studio 
- Hit `Ctrl + U` + `Ctrl + L` to run all the tests in the solution
