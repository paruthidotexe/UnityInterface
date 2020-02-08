import socket                
import pickle
import time

# Create a socket object 
clientSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)            

ipAddress = '127.0.0.1'
port = 20480
bufferSize = 1024
  
# connect to the server on local computer 
clientSocket.connect((ipAddress, port)) 

while True:
    full_msg = b''
    new_msg = True
    while True:
        msg = clientSocket.recv(bufferSize)
        if not msg:
            break

        if new_msg:
            print(f'new msg length: {msg[:bufferSize]}')
            print("-------------")
            print(msg[:bufferSize])
            print("-------------")
            msglen = str(msg[:bufferSize])
            new_msg = False

        full_msg += msg

        if len(full_msg)-bufferSize == msglen:
            #print("Full msg received")
            #print(full_msg[bufferSize:].decode("utf-8"), end='\n')

            data = pickle.loads(full_msg[bufferSize:])
            #print(data)

            new_msg = True
            full_msg = b''
        print(full_msg.decode("utf-8"), end='\n')

# close the connection 
clientSocket.close()  

#print (full_msg)
