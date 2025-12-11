#!/bin/bash

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Wait until SQL Server is ready
echo "Waiting for SQL Server to start..."
until /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1" &> /dev/null
do
  echo "SQL Server is not ready yet. Waiting..."
  sleep 1
done

# Run initialization script
echo "Running initialization script..."
/opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -i /init/init.sql

# Wait for SQL Server to keep running
wait
