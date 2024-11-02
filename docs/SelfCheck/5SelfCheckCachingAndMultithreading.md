### 1. How ASP.NET API handles multiple requests?

> They are handled in multithreaded manner. Each request is executed by a worker thread.<br>

### 2. What are the benefits and downsides of caching? When should we consider applying caching?

> The main benefit of caching is increased performance in case code meets cached data.<br>
> The downside is increased complexity of application and caching overhead on stored data.<br>

### 3. What are the differences between In-memory, Distributed or Request caching options?

> The difference would be where cached data is stored.
> For In-Memory type - it would be Process memory of the app.
> For Distributed it would be database servers, dedicated to cache. 
> Request caching allows cache to be stored on client side or on proxy servers.

### 4. What does ‘session affinity’ and ‘thread affinity’ mean? When do we have to consider session affinity?

>Session affinity - when multiple requests from a client are sticked to a single responder.<br>
>Session affinity should be used when servers don't share common data, like encryption keys.<br>

>Thread affinity - when Thread has preferences on which processor it would like to run.<br>
>Doesn't gurantee that it would actually run on a prefered processor but the chance is higher.<br>

### 5. What are the race conditions and deadlocks? Do they possible in a single threaded application?

>Race conditions happen when multiple threads are performing sequence of operations correct<br>
>result of which is dependent on data, available to all of threads.<br>
>Deadlocks is when execution of two or more threads is not possible because they<br>
>are waiting for each other.<br>
>They can't happen in a single threaded application.

### 6. Why is it not safe to use static constructors/fields when your code is running in a multithreaded application?

>Static content can be accessed and changed simultaneously by multiple threads.<br>
>If it was not constructed in thread-safe way - data corruption or unexpected behavior<br>
>can occur.

### 7. What  objects and features .NET proposes to solve race conditions and deadlocks?

> Using locks or other synchronization primitives can solve race conditions. <br>
> Concurrent types are designed to solve problems related to deadlocks. They are lock-free.
