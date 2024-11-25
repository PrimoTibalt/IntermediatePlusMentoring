### 1. What is the difference between load testing and profiling? 

> Profiling is used on application level to determine it's bottlenecks<br>
> using such metrics as CPU and memory usage. Pip-points to reasons of bad performance.<br>

> Load testing on the other hand helps to determine that there is an issue<br>
> with system performance under expected high load, so later you could use<br>
> profiling to determine what is causing it.<br>

### 2. What are the differences between performance, load, and stress testing?

> Performance testing is focused on over-all system performance metrics.<br>
> Includes load and stress testing

> Load testing is non-functional type of testing which is performed under<br>
> expected user load. Assesses system performance under normally-high load.<br>

> Stress testing is non-functional type of testing which is performed under<br>
> extreme conditions.

### 3. When would you prefer vertical scaling over horizontal?

> When it's cheeper or when you build small project without expectancies of high traffic.

### 4. Does ASP.NET Core API support horizontal scaling? Explain your answer.

> Yes, ASP.NET Core API supports horizontal scaling because you can create<br>
> applications using it which would be stateless. That would require additional<br>
> setup for such things as authentication and internal routing<br>