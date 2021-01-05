# C4T Engie App
Created by:
* Tom England
* Ben Payne
* Liam Leonard
* Zayn Iftikhar
* Matthew Osborne


#Testing
##Overview
Tests have been written using the Xamarin.UITest framework that uses NUnit.  
This framework allows for the tests to interact with the application in the same 
way that a user would – by observing controls and tapping them appropriately.

##Structure
A “Page Object Architecture” has been followed where there is a class 
representing each page of the application.  Methods are available for each 
action a user may make – such as tapping a button or entering text into a 
textbox.  This reduces code duplication and allows for the tests to be easily 

updated if changes are made.
Tests have been split into categories (e.g. “Login”, “Navigation”) and these 
categories can be run when changes are made to relevant underlying code.
Alternatively, tests can be run individually or all the tests can be run one 
after the other.

##Running tests
1. Open “Test Explorer” window in visual studio (CTRL + E, T)
2. Ensure you have a suitable emulator set up or device connected.  Running the app on that device prior to testing ensures the correct version of the app is loaded there.
3. Select the test(s) you would like to run in the test explorer.  For example:
a. select “Tests(Android)” to run all Android test.
b. select an individual test to only run that test.
c. select “group by” and “traits” to sort by category.  Now you can select a category like “Navigation” to only run tests from that category
4. Select “run” in test explorer window (CTRL+R, T) to run selected test(s)

##Troubleshooting
Ensure Xamarin components are installed.  You can check in the 
“Individual Components” section under “Tools > Get Tools and Features…”.
A restart of visual studio is often required to make these changes apply.

