# About
Here’s an improved and detailed explanation of the concept you're trying to demonstrate. This example shows how to use RabbitMQ with .NET 8, where an API publishes a message to a RabbitMQ queue, and a .NET Background Service consumes the messages produced by the API.


# Run the application
1 - Install Docker Desktop _(if you don't have it)_   
2 - On the terminal open the directory of this project where the "docker-compose.yml" file is   
3 - Run the docker command bellow   
``` terminal
docker compose up -d
```
![image](https://github.com/user-attachments/assets/5055f636-65d8-43a6-89aa-18c07cbfc0c7)

4 - Check if the RabbitMQ docker container is running correctly
![image](https://github.com/user-attachments/assets/1dc90ab1-9c10-41d1-babf-5a286a2f4184)

5 - Try to access the localhost RabbitMQ address.   
http://localhost:15672/
![image](https://github.com/user-attachments/assets/eac6ab41-b4bb-4a27-a753-561f6a534ac8)

6 - Run the application on Visual Studio!   

7 - Access the swagger and send a request
![image](https://github.com/user-attachments/assets/6bcd3f64-f96b-4190-80ba-7aa0a002b604)

8 - The message sent on the terminal
![image](https://github.com/user-attachments/assets/2f1ea6c5-d10d-4ebc-adee-c0312749f850)

9 - Background service consumes the message
![image](https://github.com/user-attachments/assets/275b073c-cb20-4906-b05f-37254003491f)
