Chapter 4: ASP.NET is a Great Place for Microservices
=====================================================

These samples show lots of ways to run ASP.NET:

- ASP.NET MVC - Controllers and Views render HTML for enterprise-scale apps that
need SEO
- Razor Pages - Host a simple website that doesn’t need much code-behind
- Web API - Controllers render JSON for REST services where the client is other
microservices or thick-client browser apps like React and Vue.js
- Minimal APIs - Very small web services and single-task applications where the ceremony
of controllers isn’t necessary
- Azure Functions - Focus solely on the method. Scale up to meet demand, and down to 0
when not in use.
- KEDA Functions - Host Azure Functions in containers running in Kubernetes

You can use the ASP.NET technology that best meets your needs. Or combine multiple types into a single project.

Usage
-----

Some of these samples require a database. You can quickly spin up Microsoft SQL Server in a container:

1. Start Docker Desktop or a compatible container runtime

2. Run the container through Docker Compose:

   ```sh
   cd 'MSSQL in Docker'
   docker compose up
   ```

3. Set the app's connection string to this:

   ```text
   Server=localhost;Database=MyDatabase;User Id=SA;Password=P@ssword!;TrustServerCertificate=True;
   ```

   Note: if you changed the password in the docker-compose file, you'll also need to adjust it here.
