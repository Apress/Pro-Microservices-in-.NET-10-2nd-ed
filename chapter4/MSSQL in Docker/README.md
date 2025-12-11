MSSQL in Docker
===============

If you don't have a MS SQL Server instance handy, you can use this project to spin up SQL Server 2025 in a container to run these demos.

Instructions
------------

1. Ensure your container runtime is started: Docker Desktop, Podman, etc.

2. Open a terminal in this folder and run:

   ```sh
   docker compose up
   ```

3. Start any of the solutions in your favorite IDE.

4. Set user secrets to this:

   ```json
   {
     "ConnectionStrings": {
       "MyDbContext": "Server=localhost,1433;Database=MyDatabase;User Id=sa;Password=P@55word!;TrustServerCertificate=true"
     }
   }
   ```

5. Start the project using the IDE's debugger or in a terminal run:

   ```
   dotnet run
   ```
