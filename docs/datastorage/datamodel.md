# Defining Data Models

Models are a representation of the data you wish to present to the user. When using App Service Helpers, you can continue to create models just as you normally would when building apps. All that's required is to inherit from the AppServiceHelpers.Models.EntityData class.

The EntityData base class provides you with everything you need to make your models cloud-compatible, without having to worry about the implementation yourself.

## The EntityData Class

EntityData contains a few properties to help your app properly communicate with an Azure backend:

* Id: Standard identifier for the model.
* CreatedAt & UpdatedAt: Tracks the state of a model.
* Version: Identifies the specific state of the model at a particular point.

All four of these properties are used in automatic conflict resolution to help identify and resolve conflicts.

```csharp 
namespace AppServiceHelpers.Models
{
    public class EntityData
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [UpdatedAt]
        public DateTimeOffset UpdatedAt { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
```

### Example Model
Making your models cloud-compatible is easy: just subclass EntityData. Note that because EntityData includes an Id property, our models don't need to define another one.

```csharp 
using System;
using AppServiceHelpers.Models;

namespace MyApp.Models
{
    public class TodoItem : EntityData
    {
        public string Title { get; set;}
        public string Description { get; set;}
        public bool Completed { get; set;}
    }
}
```
