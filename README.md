# ExternalModule  
Acts as the gateway for the system, interfacing with DatabaseAPI, OperationsAPI, and SecurityManager. Ensures controlled access to the system's functionalities for optimal performance.  


## Setup       
- docker build . -t systemgateway    
- docker run --name SystemGateway -p 9050:443 -p 8050:80 -d systemgateway  