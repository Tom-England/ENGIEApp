# C4T Engie App
Created by:
* Tom England
* Ben Payne
* Liam Leonard
* Zayn Iftikhar
* Matthew Osborne

# Running the App
## Logging in
In order to log into the app, the user must enter their university login details
in the file DatabaseConnector.cs, located in the ENGIE_App folder. The user needs to
place their username in the bnumber variable and their password in the unipass variable

E.g.
```
String bnumber = "b1234567";
String unipass = "Hunter2";
```

## Generating QR codes
To generate a QR code the admin must have set a destination email address for the code to be sent to. This can be done on the admin options page.

## QR Scanning and Form Submission
QR codes are read using the ZXing.Net.Mobile.Forms package. The QR codes contain
a 3 letter text string of either 'ELT', 'EML' or 'GAS'. Once the QR code has been
succesfully scanned, the user will be displayed with the correct user input form.
Once the user has entered all neccesary fields and the submit button has been
pressed, the user input details will be saved into the relevant text fields in the
corresponding PDF document. The Syncfusion.Xamarin.PDF library and ISave.cs file
are used to fill in the PDF document. An email is then sent to the destination email,
that is set in the admin area.

# Testing
## Overview
Tests have been written using the Xamarin.UITest framework that uses NUnit.  
This framework allows for the tests to interact with the application in the same
way that a user would – by observing controls and tapping them appropriately.

## Structure
A “Page Object Architecture” has been followed where there is a class
representing each page of the application.  Methods are available for each
action a user may make – such as tapping a button or entering text into a
textbox.  This reduces code duplication and allows for the tests to be easily

updated if changes are made.
Tests have been split into categories (e.g. “Login”, “Navigation”) and these
categories can be run when changes are made to relevant underlying code.
Alternatively, tests can be run individually or all the tests can be run one
after the other.

## Running tests
1. Open “Test Explorer” window in visual studio (CTRL + E, T)
2. Ensure you have a suitable emulator set up or device connected.  Running the app on that device prior to testing ensures the correct version of the app is loaded there.
3. Select the test(s) you would like to run in the test explorer.  For example:

* select “Tests(Android)” to run all Android test.
* select an individual test to only run that test.
* select “group by” and “traits” to sort by category.  Now you can select a category like “Navigation” to only run tests from that category
4. Select “run” in test explorer window (CTRL+R, T) to run selected test(s)

## Troubleshooting
Ensure Xamarin components are installed.  You can check in the
“Individual Components” section under “Tools > Get Tools and Features…”.
A restart of visual studio is often required to make these changes apply.
