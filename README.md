# Gilmond.MongoDB.IdentityServer4

This is a MongoDB implementation of both configuration and operational persistence for [IdentityServer4](https://github.com/identityserver/identityserver4).

## Usage

### Configuration

### Docker

For local development, we recommend using [Docker](). Follow these steps to create a MongoDB container locally and have identity server connect to it:


1. Start MongoDB locally with the following command:

_Note, if you bind a specific port here using_ `-p 27017:12345` _instead of_ `-P`_, you can skip step 8._

`docker run --name is4-mongo -d -P mongo --auth`

2. Attach an interactive console to the admin database:

`docker exec -it is4-mongo mongo admin`

3. Create a temporary super user:

`db.createUser({ user: 'super', pwd: 'temp', roles: [{ role: 'userAdminAnyDatabase', db: 'admin' }] })`

4. Authenticate using your super user:

`db.auth('super', 'temp')`

5. Create a service account for _IdentityServer_ to use:

`db.createUser({ user: 'is4-service', pwd: '<insertStrongPassword>', roles: [{ role: 'readWrite', db: 'is4' }] })`

6. Remove the temporary super user:

`db.dropUser('super')`

7. Exit from the interactive console:

`exit`

8. Determine the port bound to 27017 for your `is4-mongo` container (unless you configured a specific port binding in step 1):

`docker port is4-mongo`

This will give output such as:

`27017/tcp -> 0.0.0.0:32768`

The final number (`32768`) is the port used below when configuring the connection.

9. Configure connection details:

```csharp

```