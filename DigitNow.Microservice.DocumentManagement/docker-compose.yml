version: "3.3"

networks:
 localhost:
  driver: "bridge"
  name: "localhost"
 local_network:
  driver: bridge
  name: local_network

services:
 documentmanagement:
  build:
   context: ./
   dockerfile: ./local.Dockerfile
  container_name: documentmanagement
  image: documentmanagement-digitnow
  restart: unless-stopped
  networks:
   - localhost
   - local_network
  ports: 
   - 7003:7003
  environment:
   ASPNETCORE_URLS: "http://0.0.0.0:7003"
   ASPNETCORE_ENVIRONMENT: "Development"
   MIGRATE_DATABASE: "true"
   ENABLE_AUTO_SERVICE_DISCOVERY: "false"