#
#-------------------------------------------------------------------------
# Basic Server
# ============
#
# Notes:
# ------
# Simple python server 
# This server replies Hi to new client
#
#-------------------------------------------------------------------------
#

import socket

class BasicServer:

    portNo = 4444
    ipAddress = "127.0.0.1"
    isServerRunning = False
    greetMsg = "Welcome to my server"
    maxClients = 10

    def startServer(self):
        return
    
    def stopServer(self):
        return 

basicServer = BasicServer()

basicServer.startServer()

