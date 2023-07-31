# EmployeeManagementApp
A WPF MVVM application that connects to an Oracle DB server.

### Disclaimer
This is a project made for learning purposes.

# Setup Guide
This app requires that you have access to an Oracle database server

To setup the application, follow the steps below:
1. Fork or clone the repository
2. While on your Oracle server, execute the schema script found in DatabaseSetup folder of this project to create the database with sample data
3. Inside the project directory, navigate to ```App/Presentation/App.config```
4. Once you open the file you will find three settings named ```host```, ```port``` and ```sid```
5. Fill the values of these settings to match the host, port and sid of your OracleDB server
6. Build and run the source code via Visual Studio

# Project architecture
The project uses N-layer architecture due to it being a database-driven application.
The app has three main layers:
* Presentation
* Business Logic
* Data Access

## Presentation
The presentation layer contains all the views as well as their code-behinds. These
code-behinds contain mostly code tied to the UI (such as handling a button click, etc.)
with most of the actual app logic (i.e. manipulating and inserting data) being done inside
other application modules.

Besides that, Presentation layer also holds App.xaml.cs where services are registered
for Dependency Injection.

## Business Logic
The business logic layer is the main portion of the application, where most of the logic resides.

ViewModels represent the data on a page, such as a list of employees in our system, or the new employee
about to be created by the user. One viewmodel corresponds to one view.

Commands hold the behavior of an application - they control stuff such as inserting a new user or loading
departments from the database. These commands are fields inside a viewmodel, with one command corresponding to
one user action.

## Data Access
Data access are a set of classes that abstract the lower-level database frameworks to be more readable
at a higher level.

Dapper is used due to how easy it makes mapping database entities onto models in the project.

# Tests
The app uses XUnit for testing. Currently there are only unit tests.

# Contributing Guidelines
Contributors are always welcome, but since this project is made purely for learning purposes and thus doesn't
serve any real-life purpose, there is little reason to contribute to it. If you however have any advice regarding the
code, I would love to hear you out!

# Contributors
Patryk Dziurkowski  
Szymon Kruk
