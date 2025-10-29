Project title: Library Management System

Description:
The Library Management System is a comprehensive web-based application built with ASP.NET Core MVC and Firebase Realtime Database designed to streamline library operations. This system provides an intuitive interface for managing books, library members, and book transactions efficiently.

Key Features:
📖 Book Management: Add, edit, delete, and track books with ISBN, categories, quantities, and cover images
👥 Member Management: Register members with auto-generated membership numbers, track status (Active/Inactive/Suspended), and support different member types (Regular/Premium/Student)
🔄 Transaction Management: Issue and return books with automatic overdue detection and fine calculation ($5 per day)
📊 Admin Dashboard: Real-time statistics showing total books, members, active issues, and overdue books
🔐 Secure Admin Login: Session-based authentication for library administrators
🎨 Modern UI: Responsive Bootstrap 5 design with Font Awesome icons, works on all devices
🔥 Real-time Sync: All data stored in Firebase Realtime Database with instant updates

Installation steps:
Prerequisites:
Before starting, ensure you have:

.NET SDK 8.0 or higher - Download from https://dotnet.microsoft.com/download
Firebase Account (Free) - Create at https://firebase.google.com/
Code Editor - Visual Studio Code or Visual Studio 2022
Web Browser - Chrome, Edge, Firefox, or Safari


Step 1: Clone or Download the Project
bash# Clone the repository
git clone https://github.com/yourusername/LibraryManagement.git
cd LibraryManagement
OR Download ZIP file and extract it.

Step 2: Create Required Folders
bash# Create image folders
mkdir -p wwwroot/images/books

# On Windows (PowerShell):
New-Item -ItemType Directory -Path "wwwroot/images/books" -Force

Step 3: Add Required Images
Place these images in wwwroot/images/:

logo.png (or logo.jpg) - Your library logo (150x50px recommended)
default-book.png - Default book cover image (200x300px recommended)


Note: You can use placeholder images initially.


Step 4: Restore Dependencies
bashdotnet restore
This installs all required packages including Newtonsoft.Json.

Step 5: Firebase Setup
5.1: Create Firebase Project

Go to https://console.firebase.google.com/
Click "Add project" or "Create a project"
Enter project name: LibraryManagement
Click "Continue" → Disable Google Analytics (optional) → Click "Create project"

5.2: Enable Realtime Database

In Firebase Console, click "Realtime Database" from left sidebar
Click "Create Database"
Choose your location:

India/Asia: asia-southeast1
US/Americas: us-central1
Europe: europe-west1


Select "Start in test mode" → Click "Enable"

5.3: Set Database Rules

Go to "Rules" tab in Realtime Database
Replace with:

json{
  "rules": {
    ".read": true,
    ".write": true
  }
}

Click "Publish"

⚠️ Note: These rules are for development only. For production, implement proper authentication.
5.4: Get Your Database URL

In Realtime Database, copy the URL from the top (e.g., https://library-management-xxxxx-default-rtdb.asia-southeast1.firebasedatabase.app/)
Important: URL must end with /

5.5: Configure Firebase URL in Code

Open Services/FirebaseService.cs
Find line 9:

csharpprivate readonly string _firebaseUrl = "https://YOUR-PROJECT-ID.firebaseio.com/";

Replace with your actual Firebase URL:

csharpprivate readonly string _firebaseUrl = "https://library-management-xxxxx-default-rtdb.asia-southeast1.firebasedatabase.app/";

Save the file

5.6: Add Admin Account
Method 1 - Import JSON (Recommended):

Create a file admin-data.json with:

json{
  "admins": {
    "admin1": {
      "username": "admin",
      "password": "admin123",
      "email": "admin@library.com",
      "fullName": "System Administrator"
    }
  }
}

In Firebase Console → Realtime Database → Data tab
Click "⋮" menu → "Import JSON"
Select your admin-data.json file → Click "Import"

Method 2 - Manual Entry:

In Firebase Console → Realtime Database → Data tab
Hover over root node → Click "+"
Name: admins → Click Add
Click "+" next to admins → Name: admin1
Add these fields to admin1:

username: admin
password: admin123
email: admin@library.com
fullName: System Administrator

Verify Admin Account:

Open in browser: YOUR-FIREBASE-URL/admins.json
Should show the admin data


How to run the project:

Method 1: Using .NET CLI (Recommended)
Open terminal/command prompt in the project directory
Run the application:
bashdotnet run
You'll see output like:

Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.

Open your web browser and go to:

HTTPS: https://localhost:5001 (Recommended)
HTTP: http://localhost:5000

To stop the application, press Ctrl + C in the terminal



Method 2: Using Visual Studio Code
Open the project folder in VS Code:
bashcode .
Install C# Dev Kit extension (if not installed):
Click Extensions icon in sidebar
Search "C# Dev Kit"
Click Install

Run the project:

Press F5, OR
Click "Run" → "Start Debugging", OR
Open terminal in VS Code and run dotnet run
Browser will automatically open at https://localhost:5001


Screenshot:
<img width="1596" height="871" alt="Screenshot 2025-10-29 162400" src="https://github.com/user-attachments/assets/3d69e7a1-bf7d-467f-a0e6-72f68c3f9224" />
<img width="1820" height="779" alt="Screenshot 2025-10-29 162124" src="https://github.com/user-attachments/assets/688a72a2-8f1d-4e11-91ac-0a0926a713f2" />
<img width="1788" height="547" alt="Screenshot 2025-10-29 162148" src="https://github.com/user-attachments/assets/19e53189-4914-462c-92a2-d2ba091bd98a" />
<img width="1725" height="806" alt="Screenshot 2025-10-29 162212" src="https://github.com/user-attachments/assets/c5b33aa5-0c03-4767-a71f-9b8fedf84bd8" />
<img width="1810" height="456" alt="Screenshot 2025-10-29 162228" src="https://github.com/user-attachments/assets/6bf20b53-0757-4e91-8d80-05debae91e52" />
<img width="1727" height="460" alt="Screenshot 2025-10-29 162246" src="https://github.com/user-attachments/assets/7528176c-4fe4-4dd1-af1a-dfce63d09284" />
<img width="1852" height="873" alt="Screenshot 2025-10-29 162541" src="https://github.com/user-attachments/assets/90aa1683-3ef8-47b1-b38e-5cdc9e40d54c" />
<img width="1849" height="874" alt="Screenshot 2025-10-29 162626" src="https://github.com/user-attachments/assets/a024d665-4d43-4172-b850-7131c79448de" />
<img width="1847" height="876" alt="Screenshot 2025-10-29 162641" src="https://github.com/user-attachments/assets/906faea9-799a-4026-a6f7-f95971a17a2b" />
<img width="1848" height="878" alt="Screenshot 2025-10-29 162709" src="https://github.com/user-attachments/assets/1db4bdb4-e5ed-499e-a548-2a422e99efe1" />









