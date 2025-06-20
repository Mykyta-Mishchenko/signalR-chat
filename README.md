**Online Chat**

This project was built using ASP.NET (.NET 8) and Angular 18.

**Key Features**
* Search for users by name or email (partial matches are supported)
* Create personal chats with found users
* Create group chats (a chat is considered a group chat if more than 3 participants are selected; all participants must be in the current user's contact list)
* Send and receive messages in real time
* Notifications for new messages if the user is in a different chat
* Sentiment analysis of messages with corresponding emoji display

**Project Architecture**

**chat-backend**

The backend follows a modular architecture and is split into three primary modules:
* Authentication/Authorization — based on JWT tokens
* Online Chat — core chat functionality
* Profile Module — for future extensibility

Additional backend structure:
* AppExtensionMethods — centralized configuration of services and extensions
* Mappers — AutoMapper profiles for clean object mapping
* Shared — contains:
  * Data — database context, entity models, and table configurations
  * Models — global/shared models

**chat-frontend**

The frontend is implemented using standalone Angular components and structured as follows:
* core — includes guards, interceptors, and shared services (e.g., AuthService)
* features — main components and services for individual modules
* shared — globally accessible parts of the application

**Core Chat Logic**
* Backend: chat-backend/Modules/OnlineChat
* Frontend: chat-frontend/src/app/features/chat

**Additional Notes**
To provide a smooth chat experience, the following components were used:
* ChatsController — retrieves data directly from the database during initial load or when creating new chats (to supplement real-time features)
* ChatHub — manages real-time chat communication via SignalR

**Test user info:**

gmail: example@gmail.com

password: examplE12_

