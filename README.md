# PROG6212 Part 2 - RaceDay API

## Project Overview
This is the RESTful Web API for the RaceDay system. It serves as the backend for the MVC web application built in Part 3. The API handles all data operations, enforces role-based access, and is fully tested.

## Setup and Run Instructions
1. Clone this repository to your local machine.
2. Open the solution (`RaceDay.API.sln`) in Visual Studio.
3. Ensure you have SQL Server (or LocalDB) installed and running.
4. Open the Package Manager Console and run `Update-Database` to create the database.
5. Press `F5` (or click Run) to start the API.
6. Navigate to `https://localhost:<port>/swagger` to view and test the endpoints via the Swagger UI.

## User Roles
The system supports two distinct roles:
*   **Organiser:** Can create, update, and delete events and categories. They can also capture results for participants.
*   **Participant:** Can view events, enroll in events by selecting a category, and view their own results.
*(Public areas are restricted; users must be logged in via session management to access protected endpoints).*

## CI/CD GitHub Actions
The GitHub Actions workflow is configured to automatically build the project and run all unit tests on every push to the `main` branch.

**Successful Green Build:**
*(Replace the line below with your actual screenshot)*
`[Insert Screenshot of Green CI/CD Build Here]`

## Video Demonstration
Watch the full demonstration of the running API, code structure, authentication, and unit tests here:

*(Replace the link below with your actual YouTube link)*
`[Insert Unlisted YouTube Video Link Here]`