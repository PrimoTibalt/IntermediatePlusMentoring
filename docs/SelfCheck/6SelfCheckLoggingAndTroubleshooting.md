### 1. What are the differences between performance, load, and stress testing?

> Performance testing is assessing how system performs under various conditions.<br>
> Can include but is not limited to load and stress testing.<br>
> Focus is on overall system performance metrics.

> Load testing is performed under expected high user load.<br>

> Stress testing should simulate extreme conditions and how system<br>
> behaves in such scenarious would be its result. Can include spikes<br>
> in traffic. Can be as bad as when system fails (should recover)<br>

### 2. When would you prefer vertical scaling over horizontal?

> When it is cheeper. <br>
> When system was not designed to support vertical scailing. <br>

### 3. Does ASP.NET Core API support horizontal scailing? Explain your answer.

> Horizontal scailing means that the same API (in our case) is replicated<br>
> on multiple machines which should not know about each other. <br>
> ASP.NET Core API is a backend application that by default has nothing to do<br>
> with horizontal scailing. You can create ASP.NET Core API that would control<br>
> horizontal or vertical scailing of some system tho.<br>
> So by default ASP.NET Core API have nothing to do with infrastructure tasks<br>
> such as scailing.<br>
> If question would have been - can ASP.NET Core API be horizontally scaled? -<br>
> I would say yes.