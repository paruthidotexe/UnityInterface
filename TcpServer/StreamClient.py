import socket
import time
import threading

HOST = '127.0.0.1'
#PORT = 12345
PORT = 8888
msgToServer = (b'hai', b'GetTotalClients', b'Ping')
startTime = time.time()
pingCount = 10

def ClientThread(clientSocket):
    global startTime
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


def PingThread(clientSocket):
    global startTime
    try:
        loop = 0  
        while loop < pingCount:
            if time.time() - startTime > 5.0:
                startTime = time.time()
                clientSocket.send(msgToServer[2])
                #print(msgToServer[2])
                loop += 1

    except socket.error as exp:
        print("Exp in ClientThread : " + str(exp))
    print('PingThread closed')   
    # connection closed 
    #clientSocket.close()


s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((HOST, PORT))
s.send(b'Streaming python client')

newClientThread = threading.Thread(target= ClientThread, args=(s,))
#newClientThread.daemon = False
newClientThread.start()

newPingThread = threading.Thread(target= PingThread, args=(s,))
newPingThread.daemon = True
newPingThread.start()


# while True:
#     data = s.recv(1024)
#     if not data:
#         break
#     print (data)
#     strVal = str(data)
#     #print("--<<" + strVal + ">>--")

#print("Close Client")
#s.close()

