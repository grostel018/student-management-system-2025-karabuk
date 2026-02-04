Below is the **copy-paste ready section**, exactly formatted for a `README.md`:

---

## Requirements to Run

* Install **XAMPP**
* Start the following services:

  * Apache
  * MySQL
* Ensure the MySQL user has permission to create databases
* Install the NuGet package:

  ```
  MySql.Data, Guna
	
  ```

---

## How to Use

1. Start XAMPP (Apache + MySQL)
2. Run the application
3. The database and tables will be created automatically
4. Login using:

   ```
   Username: admin
   Password: admin
   ```

---

## Notes for Instructors

* The database is created programmatically
* No manual SQL import is required
* All tables follow relational integrity using foreign keys
* The system is safe to run multiple times (uses `IF NOT EXISTS`)

---


or Using xampp, apache and msql server, import this database.