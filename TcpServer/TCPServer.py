# first of all import the socket library 
import socket                
import pickle
import time

ipAddress = '127.0.0.1'
port = 20480
msgCount = 0
bufferSize = 16

serverSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)          
print ("Socket successfully created")

serverSocket.bind((ipAddress, port))         
print ("socket binded to %s" %(port)) 

serverSocket.listen(5)      
print ("socket is listening")
  
while True: 
  
   # Establish connection with client. 
    clientSocket, addr = serverSocket.accept()      
    print ('Got connection from', addr )
    
    data = { 1 : 'Hey', 2 : 'There'}
    msg = pickle.dumps(data)
    print(data)

    #msg = f"{len(msg):<{bufferSize}}" + str(msg)
    msg = bytes(f'{len(msg):<{bufferSize}}', 'utf-8') + msg
    print(msg)
    # send a thank you message to the client.  
    #clientSocket.send(bytes(msg, "utf-8"))
    clientSocket.send(msg)

   # Close the connection with the client 
    clientSocket.close() 

serverSocket.close()
