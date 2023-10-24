# ExternalModule
Module that allows communication from external modules with the inner application

## Setup       
- docker build . -t systemgateway    
- docker run --name SystemGateway -p 9050:443 -p 8050:80 -d systemgateway  