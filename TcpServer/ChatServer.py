import socket
import time
from datetime import datetime
from random import randint
import threading

HOST = '127.0.0.1'
PORT = 20480
MaxClients = 10

serverSocket = None
clientList = []
clientThreads = []
quitServer = False

#print_lock = threading.Lock()

# thread function 
def ClientThread(clientSocket):
    global clientList
    try:
        clientSocket.send(b'Welcome to the server')
        while True:            
            # data received from client 
            data = clientSocket.recv(1024)
            if not data: 
                print('Bye')                
                # lock released on exit 
                #print_lock.release() 
                break
            print("Client : " + data.decode("utf-8"), end='\n')
            # broadcast message
            for curClient in clientList:
                if curClient != clientSocket:
                    curClient.send(data) 
    except socket.error as exp:
        print("Exp in ClientThread : " + str(exp))
    # connection closed 
    clientSocket.close()
    print ('clientSocket.close()')    
    # remove the client if disconnected or closed
    clientList.remove(clientSocket)
    # remove the thread if disconnected or closed
    for curThread in clientThreads:
        if curThread.is_alive() == False:
            clientThreads.remove(curThread)

# Create Server Socket
def CreateSocket():
    global serverSocket
    try:
        #create an AF_INET, STREAM socket (TCP)
        serverSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    except socket.error as exp:
        print("Exp in Create Socket" + str(exp))
    print("SOCK_STREAM Socket created")

# Bind Socket & Listen
def BindSocket():
    global serverSocket
    try:
        serverSocket.bind((HOST, PORT))   
        serverSocket.listen(MaxClients) 
        # Add server socket to the list of readable connections
        #clientList.append(serverSocket)    
    except socket.error as exp:
        print ("Exp in BindSocket " + str(exp))
        BindSocket()
    print("Server socket bind @ ", PORT)

# Accept connection
def AcceptConnection():
    global serverSocket
    global clientThreads
    try:
        clientSocket, addr = serverSocket.accept()  
        clientIP = str(addr[0])
        clientPort = str(addr[1])
        print ('Client from ip [' + clientIP + "] via port no [" + clientPort + "]")
        clientList.append(clientSocket) 
        # lock acquired by client 
        #print_lock.acquire()
        newClientThread = threading.Thread(target= ClientThread, args=(clientSocket,))
        clientThreads.append(newClientThread)
        # background threads
        newClientThread.daemon = True
        newClientThread.start()
        print("Threads :" + str(len(clientThreads)) + " Connections : " + str(len(clientList) - 1) )
    except socket.error as exp:
        print ("Exp in AcceptConnection " + str(exp))
    print ("Total Clients now : ", len(clientList)-1)
    if quitServer == False:
        # Wait for next client
        AcceptConnection()

def StartServer():
    CreateSocket()
    BindSocket()
    print("Server Listening @ ", HOST, PORT)
    AcceptConnection()
    #while True:
        # Get the list sockets which are ready to be read through select
        #read_sockets,write_sockets,error_sockets = select.select(clientList,[],[])
    serverSocket.close()

def StopServer():
    global clientThreads
    global quitServer

    quitServer = True
    for curThread in clientThreads:
        curThread.join()

def Main():    
    StartServer()
    StopServer()

if __name__ == '__main__': 
    Main() 
