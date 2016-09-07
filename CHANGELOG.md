# 1.1.0

* **NEW**: One line authentication for Facebook, Twitter, Google, MSA, and Azure AD. We even secure store authentication tokens and manage token refresh scenarios automatically for you.
* **NEW**: Handle conflicts that occur between your local data store and the server with our `ConflictResolutionStrategy`. Control on a per-table basis by setting the `ConflictResolutionStrategy`
        property on the table, or subscribe to the delegate method to handle conflicts on a per-occurrence basis.
* **NEW**: Easily bind to a cloud-connected collection with `ConnectedObservableCollection`.
* **ENHANCEMENT**: Simplify API for connecting to an Azure Mobile Apps backend to just three line of code with our `EasyMobileServiceClient`.

# 1.0.0

* **NEW**:Easily connect to Azure Mobile Apps backends in just four lines of code.
* **NEW**:Automatic online/offline sync and conflict resolution.