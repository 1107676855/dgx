
import socket
import threading
import json
import sqlite3
import math
global isConnect

import random
import time
isConnect= False
isStart = False

cid={}
playerdict={}
Tankinfo=""
Tankname=""
playerLogin={}
tagToName={}
HpNeedUpdata={}
playerDie={}
navMesh={11:"target",21:"target",22:"target",31:"target",32:"target",33:"target"}
navMeshinfo={}
sig=True
count=0
isGameOver=False


#enemy count
enemyLive1=[11]
enemyLive2=[21,22]
enemyLive3=[31,32,33]
enemyIsBoom={}

def on_new_connection(client_executor, addr):
    print('Accept new connection from %s:%s...' % addr)
    recy_thread=threading.Thread(target=message_receiver, args=(client_executor,addr))
    recy_thread.start()


    data=""
    while(data!='exit'):
        data=input()
        client_executor.send(data+'\n')

    client_executor.close()

    print('Connection from %s:%s closed.' % addr)



def message_receiver(client_executor,addr):
    username=""
    global playerdict
    global Tankinfo
    global  Tankname
    global isStart
    global count
    global playerLogin
    global tagToName
    global enemyLive1
    global enemyLive2
    global enemyLive3
    global enemyIsBoom
    global HpNeedUpdata
    global playerDie
    global navMesh
    global isGameOver
    navMeshLocal={11:"target",21:"target",22:"target",31:"target",32:"target",33:"target"}
    prePlayerInfo={}
    enemyIsDie1 = False
    enemyIsDie2 = False
    enemyIsDie3 = False
    enemyIsLive1 = [11]
    enemyIsLive2 = [21,22]
    enemyIsLive3 = [31,32,33]
    playerDielocal={}
    isGameoverLocal=False


    msg=""
    while True:
        if isGameOver==True and isGameoverLocal==False:
            client_executor.send("<GameOver:failed>"+"\n")
            isGameoverLocal=True


        if username!="" and HpNeedUpdata[username]==True:
            conn=sqlite3.connect("TPSUser.db")
            c=conn.cursor()
            c.execute("SELECT * from user where username = ?",(username,))
            row=c.fetchone()
            client_executor.send("<PlayerUpdata:{\"HpUpdata\":" + str(row[2]) + "}>"+'\n')
            HpNeedUpdata[username]=False
            c.close()
            conn.commit()
            conn.close()
        for x in playerdict:
            if x!=username:

                if prePlayerInfo.has_key(x)==False:
                    client_executor.send(playerdict[x] + '\n')
                    prePlayerInfo[x]=playerdict[x]
                elif prePlayerInfo[x]!=playerdict[x]:
                    client_executor.send(playerdict[x] + '\n')
                    prePlayerInfo[x]=playerdict[x]
        if Tankname!="" and Tankname!=username and Tankname!="NoBody":
            print (Tankinfo)
            client_executor.send(Tankinfo+'\n')

        if len(enemyIsLive1)!=len(enemyLive1):
            list_temp=list(set(enemyIsLive1)-set(enemyLive1))
            for x in list_temp:
                if enemyIsBoom.has_key(x):
                    client_executor.send("<EnemyBoom:{\"enemy\":"+str(x)+"}>"+ '\n')
                else:
                    client_executor.send( "<EnemyDie:{\"enemy\":"+str(x)+"}>"+ '\n')
                enemyIsLive1.remove(x)
        if len(enemyIsLive2) != len(enemyLive2):
            list_temp = list(set(enemyIsLive2) - set(enemyLive2))
            for x in list_temp:
                if enemyIsBoom.has_key(x):
                    client_executor.send("<EnemyBoom:{\"enemy\":" + str(x) + "}>" + '\n')

                else:
                    client_executor.send("<EnemyDie:{\"enemy\":"+str(x)+"}>" + '\n')
                enemyIsLive2.remove(x)
        if len(enemyIsLive3)!=len(enemyLive3):
            list_temp=list(set(enemyIsLive3)-set(enemyLive3))
            for x in list_temp:
                if enemyIsBoom.has_key(x):
                    client_executor.send("<EnemyBoom:{\"enemy\":" + str(x) + "}>" + '\n')
                else:
                    client_executor.send( "<EnemyDie:{\"enemy\":"+str(x)+"}>"+ '\n')
                enemyIsLive3.remove(x)

        if len(enemyLive1) == 0 and enemyIsDie1==False:
            client_executor.send("<GamePass:first>"+'\n')
            enemyIsDie1=True
        if len(enemyLive2)==0 and enemyIsDie2==False:
            client_executor.send("<GamePass:second>"+'\n')
            enemyIsDie2=True
        if len(enemyLive3)==0 and enemyIsDie3==False:
            client_executor.send("<GamePass:win>"+'\n')
            enemyIsDie3=True


        diff= set(navMesh.items()) ^ set(navMeshLocal.items())
        if len(diff)>0:
            for x in diff:
                client_executor.send("<Enemytarget:{\"enemy\":"+str(x[0])+","+navMeshinfo[x[0]]+",\"playertag\":\""+navMesh[x[0]]+"\"}>"+'\n')
                navMeshLocal[x[0]]=navMesh[x[0]]




        try:
            msg = client_executor.recv(1024).rstrip('\r\n')
        except:
            print ("error")

        if msg != "":
            print(msg)
            target = msg.find(":")
            tag = msg[:target]
            content = msg[target+1:]
            dict = json.loads(content)
            if tag == "register":
                print("register over, username: {0} password: {1}".format(dict['name'],dict['password'] ) )
                conn = sqlite3.connect("TPSUser.db")
                c = conn.cursor()
                c.execute("SELECT password from user where username = ?", (dict['name'],))
                row =c.fetchone()
                if row!=None:
                    print ("register failed")
                    client_executor.send("<register:failed>" + '\n')
                else:
                    c.execute("INSERT INTO user (username,password) VALUES ('{}','{}')".format(dict['name'],dict['password']))
                    client_executor.send("<register:success>" + '\n')
                    print("insert over!")
                c.close()
                conn.commit()
                conn.close()
            elif tag == "login":
                conn = sqlite3.connect("TPSUser.db")
                c = conn.cursor()
                c.execute("SELECT password from user where username = ?", (dict['name'],))
                row = c.fetchone()
                if row == None:
                    print("username is null")
                elif row[0] == dict['password']:
                    print("logined!")
                    username = dict['name']
                    # client_executor.send(bytes("login:success".encode('utf-8')))
                    if isStart==False:
                        playerLogin[username] = str(count)
                        info = "<login:{\"username\": \"" + username + \
                               "\",\"Num\": " + playerLogin[username] + "}>"

                        client_executor.send(info + '\n')
                        tagToName["player"+str(count+1)]=username
                        playerDie[username]=False
                        playerDielocal[username]=True
                        HpNeedUpdata[username]=False
                        c.execute("UPDATE user set userhp = ? where username = ?",(100,username))
                        c.execute("UPDATE user set money = ? where username = ?", (0, username))
                        cid[addr]=count
                        count+=1

                    elif isStart==True:
                        print(111)

                else:
                    print("login error")
                    client_executor.send("<login:failed>" + '\n')
                c.close()
                conn.commit()
                conn.close()
            elif tag == "CreatePlayer":
                #playerlist=playerdict.keys()
                playerCreate="<createPlayer:{\"username\": \"" + username  +"\",\"Num\":  \"" + playerLogin[username]+ "\"}>"
                print(playerCreate)
                isCreate=True
                client_executor.send(playerCreate+'\n')

            elif tag=="reday":
                while True:
                    if count==3:
                        client_executor.send("<reday:go!>"+'\n')
                        isStart=True
                        break

            elif tag == "playerstatus":
                if dict['tag'] != "player" + str(cid[addr] + 1):
                    break
                conn = sqlite3.connect("TPSUser.db")
                c = conn.cursor()
                c.execute("UPDATE user set  Gun1 = ?, Gun2 = ?, \
                           Genade = ? ,  px=?,py=?,pz=?,rw=?,rx=?,ry=?,rz=?,\
                           h=?,v=?,f=?,reweapon=? ,tag=?where username = ?",(dict['Gun1'],\
                          dict['Gun2'],dict['Genade'],dict['px'],dict['py'],dict['pz'],\
                          dict['rw'],dict['rx'],dict['ry'],dict['rz'],dict['h'],dict['v'],dict['f'],dict['reweapon'],dict['tag'],username))
                c.execute("SELECT * from user where username = ?", (username,))
                row=c.fetchone()
                info = "<playerstatus:{\"name\": \"" + str(row[0])   + \
                       "\",\"hp\": " + str(row[2]) + ",\"Gun1\": " + str(row[3]) +",\"Gun2\": " + str(row[4]) + \
                       ",\"Genade\":"+str(row[17])+",\"money\": "+ str(row[5])+ ",\"px\": "+ str(row[6])+",\"py\": "+str(row[7])+\
                       ",\"pz\":"+str(row[8])+",\"rw\":"+str(row[9])+",\"rx\":"+str(row[10])+",\"ry\":"+str(row[11])+\
                       ",\"rz\":"+str(row[12])+",\"h\":"+str(row[13])+",\"v\":"+str(row[14])+",\"f\":"+str(row[15])+\
                       ",\"reweapon\":"+str(row[16])+",\"tag\":\""+str(row[18])+"\"}>"
                playerdict[str(row[0])]=info
                c.close()
                conn.commit()
                conn.close()

            elif tag=="Tankstatus":
                Tankname=tagToName[dict["playername"]]
                Tankinfo="<"+msg+">"
            elif tag=="CreateEnemy":
                if dict["enemy"]==1:
                    f=open("enemy1.txt","r")
                    line = f.readline()
                    line = line[:-1]
                    client_executor.send(line + '\n')
                    while line:
                        line = f.readline()
                        line = line[:-1]
                        print (line)
                        client_executor.send(line+'\n')
                    f.close()
                elif dict["enemy"] == 2:
                    f = open("enemy2.txt", "r")
                    line = f.readline()
                    line = line[:-1]
                    client_executor.send(line + '\n')
                    while line:
                        line = f.readline()
                        line = line[:-1]
                        print (line)
                        client_executor.send(line + '\n')
                    f.close()
                elif dict["enemy"] == 3:
                    f = open("enemy3.txt", "r")
                    line = f.readline()
                    line = line[:-1]
                    client_executor.send(line + '\n')
                    while line:
                        line = f.readline()
                        line = line[:-1]
                        print (line)
                        client_executor.send(line + '\n')
                    f.close()

            elif tag == "EnemyDie":


                conn = sqlite3.connect("TPSUser.db")
                c = conn.cursor()
                c.execute("SELECT * from user where username = ?", (username,))
                row = c.fetchone()
                if dict["enemy"] / 10 == 1:
                    if enemyLive1.count(dict["enemy"]) > 0:
                        enemyLive1.remove(dict["enemy"])
                        enemyIsLive1.remove(dict["enemy"])
                        c.execute("UPDATE user set money = ? where username = ?", (row[5] + 10, username))
                if dict["enemy"] / 10 == 2:
                    if enemyLive2.count(dict["enemy"]) > 0:
                        enemyLive2.remove(dict["enemy"])
                        enemyIsLive2.remove(dict["enemy"])
                        c.execute("UPDATE user set money = ? where username = ?", (row[5] + 10, username))
                if dict["enemy"] / 10 == 3:
                    if enemyLive3.count(dict["enemy"]) > 0:
                        enemyLive3.remove(dict["enemy"])
                        enemyIsLive3.remove(dict["enemy"])
                        c.execute("UPDATE user set money = ? where username = ?", (row[5] + 10, username))

                c.execute("SELECT * from user where username = ?", (username,))
                row = c.fetchone()
                client_executor.send("<MoneyUpdata:{\"money\":" + str(row[5]) + "}>" + '\n')
                c.close()
                conn.commit()
                conn.close()
                #print("score add over")
            elif tag=="EnemyBoom":
                if dict["enemy"] / 10 == 1:
                    if enemyLive1.count(dict["enemy"]) > 0:
                        enemyLive1.remove(dict["enemy"])
                        enemyIsLive1.remove(dict["enemy"])
                        enemyIsBoom[dict["enemy"]]="\"px\":"+str(dict["px"])+",\"py\":"+str(dict["py"])+",\"pz\":"+str(dict["pz"])
                        print ("player Hp --")
                if dict["enemy"] / 10 == 2:
                    if enemyLive2.count(dict["enemy"]) > 0:
                        enemyLive2.remove(dict["enemy"])
                        enemyIsLive2.remove(dict["enemy"])
                        enemyIsBoom[dict["enemy"]] = "\"px\":" + str(dict["px"]) + ",\"py\":" + str(dict["py"]) + ",\"pz\":" + str(dict["pz"])
                        print ("player Hp --")
                if dict["enemy"] / 10 == 3:
                    if enemyLive3.count(dict["enemy"]) > 0:
                        enemyLive3.remove(dict["enemy"])
                        enemyIsLive3.remove(dict["enemy"])
                        enemyIsBoom[dict["enemy"]] = "\"px\":" + str(dict["px"]) + ",\"py\":" + str(dict["py"]) + ",\"pz\":" + str(dict["pz"])
                        print ("player Hp --")


                for x in tagToName:
                    conn = sqlite3.connect(("TPSUser.db"))
                    c = conn.cursor()
                    c.execute("SELECT * from user where username = ?", (tagToName[x],))
                    row = c.fetchone()
                    destance=math.sqrt(pow(row[6]-dict["px"],2)+pow(row[8]-dict["pz"],2))
                    if destance<10:
                        c.execute("UPDATE user set userhp = ? where username = ?",(row[2]-100,tagToName[x]))
                        if tagToName[x]==username:
                            c.execute("SELECT * from user where username = ?",(username,))
                            row=c.fetchone()
                            client_executor.send("<PlayerUpdata:{\"HpUpdata\":"+str(row[2])+"}>"+'\n')
                        else:
                            HpNeedUpdata[tagToName[x]]=True

                    c.close()
                    conn.commit()
                    conn.close()

            elif tag=="EnemyBullet":
                conn=sqlite3.connect("TPSUser.db")
                c=conn.cursor()
                c.execute("SELECT * from user where username = ?", (username,))
                row=c.fetchone()
                c.execute("UPDATE user set userhp = ? where username = ?", (row[2] - 50, username))
                c.execute("SELECT * from user where username = ?", (username,))
                row=c.fetchone()
                client_executor.send("<PlayerUpdata:{\"HpUpdata\":" + str(row[2]) + "}>"+'\n')
                c.close()
                conn.commit()
                conn.close()
            elif tag=="EnemyTarget":
                if dict["playertag"]!="target":

                    if navMesh[dict["enemy"]]=="target":
                        navMesh[dict["enemy"]]=dict["playertag"]
                else:
                    navMesh[dict["enemy"]]="target"
                navMeshinfo[dict["enemy"]]="\"px\":"+str(dict["px"])+",\"py\":"+str(dict["py"])+",\"pz\":"+str(dict["pz"])

            elif tag=="BuyBullet":
                conn = sqlite3.connect("TPSUser.db")
                c = conn.cursor()
                c.execute("SELECT * from user where username = ?", (username,))
                row = c.fetchone()
                c.execute("UPDATE user set money = ? where username = ?", (row[5] -30, username))
                c.execute("SELECT * from user where username = ?", (username,))
                row = c.fetchone()
                client_executor.send("<MoneyUpdata:{\"money\":" + str(row[5]) + "}>" + '\n')
                c.close()
                conn.commit()
                conn.close()

            elif tag == 'playerDie':
                conn = sqlite3.connect("TPSUser.db")
                c = conn.cursor()
                c.execute("UPDATE  user set userhp = 0 where username = ?",(username,))

                c.execute("SELECT * from user where username = ?", (username,))
                row = c.fetchone()

                info = "<playerstatus:{\"name\": \"" + str(row[0]) + \
                       "\",\"hp\": " + str(row[2]) + ",\"Gun1\": " + str(row[3]) + ",\"Gun2\": " + str(row[4]) + \
                       ",\"Genade\":" + str(row[17]) + ",\"money\": " + str(row[5]) + ",\"px\": " + str(
                    row[6]) + ",\"py\": " + str(row[7]) + \
                       ",\"pz\":" + str(row[8]) + ",\"rw\":" + str(row[9]) + ",\"rx\":" + str(
                    row[10]) + ",\"ry\":" + str(row[11]) + \
                       ",\"rz\":" + str(row[12]) + ",\"h\":" + str(row[13]) + ",\"v\":" + str(
                    row[14]) + ",\"f\":" + str(row[15]) + \
                       ",\"reweapon\":" + str(row[16]) + ",\"tag\":\"" + str(row[18]) + "\"}>"

                playerdict[username] = info
                playerDie[username]=True
                if len(set(playerDie.items()) ^ set(playerDielocal.items())) == 0:

                    isGameOver= True
                c.close()
                conn.commit()
                conn.close()

            elif tag=="GameOver":
                isGameOver=True


listener = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
listener.bind(('127.0.0.1', 50000))
listener.listen(5)
print('Waiting for connect...')


while True:

    client_executor, addr = listener.accept()
    t = threading.Thread(target=on_new_connection, args=(client_executor, addr))

    t.start()


