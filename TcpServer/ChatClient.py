import socket
import time
from datetime import datetime
from random import randint
import threading

HOST = '127.0.0.1'
PORT = 20480


def ClientThread(clientSocket):
    try:
        while True:   
            # data received from client 
            data = clientSocket.recv(1024) 
            #print(data)  
            if not data: 
                print('Bye')                
                # lock released on exit 
                #print_lock.release() 
                break
            print("Server : " + data.decode("utf-8"), end='\n')
    except socket.error as exp:
        print("Exp in ClientThread : " + str(exp))
    print('closed')   
    # connection closed 
    clientSocket.close()

def SendMsg(clientSocket):
    try:
        while True:   
            chatMsg = input('Client : ')
            #print ('Client : ' + chatMsg)
            clientSocket.send(bytes(chatMsg, 'utf-8'))
            if chatMsg == 'quit':
                break
    except socket.error as exp:
        print("Exp in ClientThread : " + str(exp))
    # connection closed 
    clientSocket.close()

    

def Main():    
    clientSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    clientSocket.connect((HOST, PORT)) 
    print('connect')
    newClientThread = threading.Thread(target= ClientThread, args=(clientSocket,))
    #newClientThread.daemon = True
    newClientThread.start()
    msgThread = threading.Thread(target= SendMsg, args=(clientSocket,))
    #msgThread.daemon = True
    msgThread.start()
    

if __name__ == '__main__': 
    Main() 

    