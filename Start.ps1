# Script.ps1

# Change to the directory containing the docker-compose file
cd .\SystemGatewayAPI\

# Run docker-build
docker build . -t systemgateway

# Run docker run container
docker run --name SystemGateway -p 9050:443 -p 8050:80 -d systemgateway  
