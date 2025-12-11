OpenTelemetry in React and Nginx
================================

In this sample we see how OpenTelemetry can run in the browser, and when the React app is built into a static site, OpenTelemetry can also be built into the static Nginx server.

To Run Locally
--------------

1. copy .env-example to .env and adjust settings to fit.

2. `npm install`

3. `npm run dev`

4. Browser automatically opens to http://localhost:3000/

To Run in Docker
----------------

1. `docker compose build`

2. `docker compose up`

3. Browse the Aspire dashboard at http://localhost:8000/

4. Browse the React app at http://localhost:8001/
