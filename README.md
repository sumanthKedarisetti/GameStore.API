Day -1 
Time spent- 1 hour 

Things done
--started a dot net project in visual studio code
--configured the dev env -to develop and run the application
--configured the  rest client extension- to test apis without opening the browser
--created DTOs for GameDTO  and creategameDTO. used records(immutable by default,reference type & equality checked by value type) feature in C# which is used for storing data 
--configured 3 rest end points 
   --getgames
   --getgamebyid
   --postid
  --tested all the end points properly & everything is working as expected.
  --pushed the code to git and created a repository there 
  --Now all the changes can be tracked.
  --tested the code after pushing to git and cloning from the repo. working good.
Day -2
Time spent -2 hours

Things done
--want to implement authentication for the API developed.
--Explored the different IAM systems.
--Found Keycloak which is an open source Identity and Access Management .
--Installed it locally and started the server. 
--It also needs JDk to be installed. faced few challenges(environment variables for the JDK).
--Then Tried to understand Realm (isolated space for users,groups,roles,clients), and clients (represents the web app/API)
--understood Authorization code flow 
     login url(discovers the endpoints from authority) with clientid(from keycloak when we create a client)
                        |
               redirected to keycloak
                        |
        after login sucessfully server sends authorisation code
                        |
         then post request to token url(discovers endpoint from authority)-->gets token 
                        |
         ASp.Net core stores the signed token in cookie
                        |
         in susequenet requests -->valiadates token from cookie using (metadata from authority)

---configured the authentication service middleware in program.cs
-- everything is working perfectly.
Day -3 
Time spent - 2 hours
--changed the authentication from cookie based to JWT based.
--changed the configuration in program.cs and also enabled service account roles in key cloak.
--Everyhting is working fine

Day -4 Time spent -2 hours
--Read about docker and some concepts like image, container and how to author docker file
--installed docker and created a docker file to build and run the application
--Able to run the container.
-- also deployed /installed the key cloak as a docker container
--then configured the appsettings file like authority url to validate the token, created the token using same host.
--able to create tokens 
--api deployed in one container with port 5005(mapped to localhost 5005) and keycloak deployed in one container(mapped to localhost:8080) . Now api needs to validate the tokens. so in api,authority url I used host.docker.internal so it can access the host machine. Host.docker.internal is the way to access host machine from container 
--Everything is working properly. i am able to create a token and able to validate the api using postman. (All working)

Day -5 Time spent -2 hours
-- want to install postgre sql in a container
--installed the postgre sql as docker container in port 5432 it is working and mapped to localhost 5432
--connected from api to postgre sql server using (container name-port) as all the containers are in the same network.
--Then configured the endpoints to interact with db like get,put,delete,post.
--also configure the Entityframework in api to interact with the database.

Day -6 Time spent 1 hr 
--changed the project folder and file structure.
--created controllers,services, Interface(Iservices,IRepository),DTOs, Persitence(Migrations,Repository,Databasecontext),Entities


