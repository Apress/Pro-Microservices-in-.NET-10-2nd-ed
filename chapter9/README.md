Chapter 9: Deploying Microservices
==================================

This chapter shows how to containerize an ASP.NET application. In these code samples, we build up from source code to a deployed application in Azure.

- 9.1 First Dockerfile - This is a good start for containerizing with Docker
- 9.2 Full Dockerfile - This expanded example uses more Dockerfile best practices
- 9.3 Container with .NET SDK - Built in to .NET is an opinionated tool for building containers
- 9.4 Azure Kubernetes Service - Example YAML files for deploying a site to Kubernetes
- 9.5 Azure Container Apps - A script to provision ACA
- 9.6 Azure App Service - A script to provision a single containerized site
- 9.7 Azure Application Gateway - Provision a light-weight layer 7 reverse proxy
- 9.8 Azure API Management - Provision the gateway to front an enterprise collection of microservices behind a single subdomain
- 9.9 YARP - Yet Another Reverse Proxy is a NuGet package that you can add to any ASP.NET app
