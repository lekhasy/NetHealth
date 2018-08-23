# NetHealth

The ultimate network health tracking system.

### The motivation

Counter Strike.

### How does it suppose to work

The system have two modules:

- DataCollector Module: it ping to one host and persist the ping result.
- OctusBoard Module: it communicate with DataCollectors to get the latest data and display to end-user in a web view.

#### DataCollector Module have two part:

- DataCollector: a liblary contains logic for collecting ping results.
- DataCollector.Host: a web API service which used for hosting the DataCollector logic and communicating with OctusBoard.
