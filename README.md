# Docker/Kubernetes Auto Scaling Demo with CPU Stresser

A lightweight, containerized ASP.NET Core Minimal API with a dynamic HTML/JS dashboard, built to demonstrate container isolation, multi-stage Docker builds, and CPU workload stress-testing for cloud infrastructure scalability.

---

## Overview

This application serves as a benchmark tool for container orchestration testing. It exposes endpoints to inspect container runtime metadata (e.g., Hostname/Pod ID, Server Time) and simulate CPU workloads to observe container resource allocation and load balancing behavior.

### Key Features
- **Minimal API Backend:** Built on .NET 8 for low memory footprint and high throughput.
- **Container Isolation:** Runtime metadata returns unique container ID (`machineName`) instead of host system info.
- **CPU Stress Engine:** Configurable time-based CPU load simulation with built-in safety boundaries.
- **Multi-Stage Dockerfile:** Optimized image size separating build-time SDKs from runtime environments.
- **Interactive Dashboard:** Pure HTML/JS frontend communicating with multi-instance container deployments over mapped ports (`8080`, `8081`, `8082`).

---

## Architecture & Multi-Stage Docker Build

The project utilizes a multi-stage Dockerfile to keep the final image minimal and production-ready:

1. **Build Stage:** `mcr.microsoft.com/dotnet/sdk:8.0` compiles the C# code and restores dependencies.
2. **Runtime Stage:** `mcr.microsoft.com/dotnet/aspnet:8.0` receives only the published binaries, significantly reducing attack surface and image size.

---

## Getting Started

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) with WSL 2 backend installed.
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (optional, for local non-containerized execution).

### Quickstart with Docker

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/DeakAndras06/Cloud-Demo.git](https://github.com/DeakAndras06/Cloud-Demo.git)
   cd Cloud-Demo
   ```
   
2. **Build the Docker Image:**
  ```bash
  docker build -t cloud-backend:v1 .
  ```
  
3. **Launch Container Instances:**
  ```bash
  docker run -d -p 8080:80 --name cloud-backend-1 cloud-backend:v1
  docker run -d -p 8081:80 --name cloud-backend-2 cloud-backend:v1
  docker run -d -p 8082:80 --name cloud-backend-3 cloud-backend:v1
  ```
  
4. **Verify Running Containers:**
  ```bash
    docker ps
  ```
    
5. **Test API Endpoints:**
  ```bash
  curl http://localhost:8080/api/info
  curl -X POST "http://localhost:8080/api/stress?durationSeconds=10"
  ```
    
6. **Cleanup & Teardown:**
  ```bash
  docker stop cloud-backend-1 cloud-backend-2 cloud-backend-3
  docker rm cloud-backend-1 cloud-backend-2 cloud-backend-3
  ```
### Deployment Steps

1. **Start Minikube**
   ```bash
   minikube start --driver=docker
   ```
2. **Build the Docker Image**
   ```bash
   docker build -t cloud-backend:v1 .
   ```
3. **Load the Image into Minikube**
   ```bash
   minikube image load cloud-backend:v1
   ```
4. **Apply Manifests**
   ```bash
   kubectl apply -f k8s/
   ```
5. **Access**
   ```bash
   minikube service cloud-backend-service
   ```
note: You will have to open another terminal to continue giving commands

### Monitoring

1. **Get the Add-on**
   ```bash
   minikube addons enable metrics-server
   ```
2. **Check resource use**
   ```bash
   kubectl top pods
   ```
3. **Watch it live**
   ```bash
   kubectl get hpa cloud-backend-hpa -w
   ```
note: New data is shown every 15-30 seconds. Start CPU stress in webapp to observe auto scaling/descaling.

### Credits

  Author: Google Gemini 3.6 Flash / 
  Code Review, Architecture & Technical Oversight: Deák András Botond
