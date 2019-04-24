# Introduction

Data storage is a fundamental component of application development. Most applications display and allow user interaction (e.g.: creating, updating, deleting) with some form of data. By using Azure Mobile Apps and App Service Helpers, we can build a robust, scalable backend that communicates with your mobile app in just a few lines of code.


## Online/Offline Synchronization

Building mobile apps for offline or disconnected scenarios is a must. Whether you're driving cross-country or just in the basement of your building, it's incredibly common to drop access to cellular data. Think about all the times you have used a mobile app and haven't had access to the internet. How did that application handle that scenario? Probably not super gracefully.

Developers have to think about building apps that work in offline scenarios. Sadly, this is not as easy as it sounds. You have to think about caching data, synchronizing it with server data when online, and handling all the potential merge issues.

App Service Helpers handles all of this for you automatically with online/offline sync - you don't even have to think about it. Our data stores push and pull data down from the server when a connection is available, cache it locally in a SQLite database, and manage merge conflicts for you.

To learn more about online/offline synchronization, visit our documentation on automatic and custom data storage.

## Automatic Conflict Resolution

When manipulating data, it's common to have merge issues - there are two versions of the exact same record. This is especially common when working with online/offline synchronization.

App Service Helpers allows you to select from three main strategies on a per-table basis: ClientWins, ServerWins, and LatestWins. ASH even contains a delegate so that you can handle conflicts individually, such as presenting a dialog for the user to select the correct version. You don't have to worry about implementing any of the logic for the resolution, just select the strategy that works best for you.

To learn more about automatic conflict resolution, visit our documentation on conflict resolution.
