# GradeCalculatorProjects
A collection of projects for grade calculation by Hudson Eicke

## Table of contents
- [<ins>Features</ins>](#features)
- [<ins>Description</ins>](#description)
- [<ins>How to install</ins>](#how-to-install)
- [<ins>Wiki</ins>](#wiki)
- [<ins>Projects</ins>](#projects)
    - [<ins>Grade Calculator Library</ins>](#grade-calculator-library)
    - [<ins>Grade Calculator App</ins>](#grade-calculator-app)
    - [<ins>Grade Calculator Terminal</ins>](#grade-calculator-terminal)
- [<ins>License</ins>](#license)

## Features
- Grade Calculator Library
    - Course temlate class for template building
    - Course calculator class for letter grade calculation
    - Can calculate required scores given any number of scores
    - Handles dropped assignments
    - Returns calcuations in nicely formatted class
- Grade Calculator App
    - Simple to use UI
    - Cross platform use
    - Template builder
    - Course calculator
    - Full error checking
- Grade Calaculator Terminal
    - Terminal based version
    - Template builder
    - Letter grade template builder for quicker template building
    - Course calculator
    - Full error checking

## Description
Grade Calculator Projects a collection of projects by Hudson Eicke. The projects contain a library for calculating scores required for letter grades, a terminal based application built on top of the library, and a GUI based application built in Avalonia UI using the library. The library is built to handle calculating required scores for all letter grades given any number of scores. Both the GUI and terminal based apps allow the user to build templates for classes that can be later loaded and to calculate scores on those templates.

## How to install

## Wiki
Link to wiki: [<ins>Wiki</ins>](https://github.com/HudsonEicke/GradeCalculatorProjects/wiki)

## Projects
A brief overview of the projects in the repo

### Grade Calculator Library
The grade calculator library is a custom library that contains both a course template builder and a course calculator. The course template builder uses data given to the template to build a template file that can be used by the calculator to setup a class to calculate letter grades off of. The course calculator uses a course template file to build the calculator then through function calls changes scores and keeps track of true grade(True grade is the overall percentage earned so far based on the score entered). Then using the scores given to the calculator it will find the scores for each assignment required to earn all letter grades. These calculations are returned in a special class called a ScoreReport. If a course has dropped assignments the calculator will also handle those too. For more information refer to the wiki page for the library: [<ins>Wiki</ins>](https://github.com/HudsonEicke/GradeCalculatorProjects/wiki/Grade-Calculator-Library)

### Grade Calculator App
<img width="1423" height="732" alt="GradeCalculatorApp" src="https://github.com/user-attachments/assets/b1f7aff2-8c80-4fc5-94ee-39f13dd2b138" />

The grade calculator app is a GUI version of the grade calculator built in Avalonia UI. The app is built to work on all platforms. The UI is simple and easy to use for anyone. The app comes with both a template builder and a course calculator that neatly layout data. The app also contains full error checking to let the user know exactly what they did wrong and prevent errors in template file generation or inputs for scores along with other issues. For more information refer to the wiki page for the app: [<ins>Wiki</ins>](https://github.com/HudsonEicke/GradeCalculatorProjects/wiki/Grade-Calculator-App)

### Grade Calculator Terminal
The grade calculator terminal is a terminal based version of the grade calculator app. The app is also built to work on all platforms. The terminal app is a lighter weight version of the grade calculator app. The app also comes with a template builder a calculator and even a letter grade template builder for quicker course building. It also has full error checking to prevent errors. For more information refer to the wiki page for the app: [<ins>Wiki</ins>](https://github.com/HudsonEicke/GradeCalculatorProjects/wiki/Grade-Calculator-Terminal)

## License
These projects are licensed under the [<ins>MIT License</ins>](LICENSE).
