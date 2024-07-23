# CodeNamesAPI Documentation

## Overview
This project provides a backend API for managing rooms and users in the CodeNames game. The API is built using ASP.NET Core and implements several endpoints for room and user management. It leverages services for business logic (`IRoomService`, `IUserService`) and handles exceptions to provide meaningful responses.

## Endpoints

### RoomController

#### 1. Get Room by ID
- **Endpoint:** `GET /api/room/roomid/{roomId}`
- **Description:** Fetches details of a room using its unique identifier.
- **Parameters:**
  - `roomId` (Guid): Unique identifier of the room.
- **Response:** 
  - `200 OK`: Room details.
  - `400 Bad Request`: If an error occurs.

#### 2. Create Room and User
- **Endpoint:** `POST /api/room/{username}`
- **Description:** Creates a new room and adds a user to it.
- **Parameters:**
  - `username` (string): Username to be added to the new room.
- **Response:** 
  - `200 OK`: Details of the created room.
  - `400 Bad Request`: If an error occurs.

#### 3. Add User to Room
- **Endpoint:** `POST /api/room/{roomid}/{username}`
- **Description:** Adds an existing user to a specified room.
- **Parameters:**
  - `roomid` (Guid): Unique identifier of the room.
  - `username` (string): Username to be added to the room.
- **Response:** 
  - `200 OK`: Details of the room after adding the user.
  - `400 Bad Request`: If an error occurs.

#### 4. Start Game
- **Endpoint:** `PATCH /api/room/{userid}`
- **Description:** Starts the game for a user.
- **Parameters:**
  - `userid` (Guid): Unique identifier of the user.
- **Response:** 
  - `200 OK`: If the game starts successfully.
  - `400 Bad Request`: If an error occurs.

#### 5. Reset Game
- **Endpoint:** `PUT /api/room/{userId}`
- **Description:** Resets the game for a user.
- **Parameters:**
  - `userId` (Guid): Unique identifier of the user.
- **Response:** 
  - `200 OK`: If the game resets successfully.
  - `400 Bad Request`: If an error occurs.

### UserController

#### 1. Change User Name
- **Endpoint:** `PATCH /api/user/{userid}/{newName}`
- **Description:** Changes the username of an existing user.
- **Parameters:**
  - `userid` (Guid): Unique identifier of the user.
  - `newName` (string): New username to be assigned.
- **Response:** 
  - `200 OK`: If the username is changed successfully.
  - `400 Bad Request`: If an error occurs.

#### 2. Set User Team and Role
- **Endpoint:** `PATCH /api/user/{userId}`
- **Description:** Sets the team and role for a user.
- **Parameters:**
  - `userId` (Guid): Unique identifier of the user.
  - `userRole` (UserRole): Role to be assigned to the user.
  - `teamColor` (TeamColor, optional): Team color to be assigned.
- **Response:** 
  - `200 OK`: If the team and role are set successfully.
  - `400 Bad Request`: If an error occurs.

## Error Handling
All endpoints handle exceptions and return appropriate HTTP status codes:
- `CustomException`: Returns `400 Bad Request` with the error message.
- Other exceptions: Also return `400 Bad Request` with the error message.

## Dependencies
- **BLL.Interface:** Business Logic Layer interfaces.
- **BLL.Validation:** Contains custom exception handling.
- **DLL.Enums:** Defines enums such as `UserRole` and `TeamColor`.

## How to Run
1. Ensure you have .NET Core SDK installed.
2. Clone the repository.
3. Restore dependencies: `dotnet restore`.
4. Build the project: `dotnet build`.
5. Run the project: `dotnet run`.
