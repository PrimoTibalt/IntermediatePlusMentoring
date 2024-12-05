### Setup
4 cores 16 threads.<br>
28gb ram (13gb available to docker).<br>
CPU load is shared accross multiple CPU threads, that's why more than 100% load is possible.

### Metrics in docker compose
Caching enabled 
Concurrent threads | 1 | 10 | 100 | 1000 | 10000 |
:---|:---:|:---:|:---:|:---:|---:
Avg Response Time | 1.54ms | 1.65ms | 1.91ms | 2.4ms | 16.23ms |
Avg Requests per Second | 1 | 8.55 | 82.3 | 959 | 7850 |
Error Rate % | 0 | 0 | 0 | 0 | 0 | 
Peak Response Time | 2.24ms | 12.50ms | 56.15ms | 228.89ms | 1260ms |
CPU Load % | 0.11 | 0.71 | 2.14 | 24.38 | 139 |
Memory Usage % | 0.39 | 0.49 | 1.63 | 6.78 | 14.80 |

Caching disabled 
Concurrent threads | 1 | 10 | 100 | 1000 | 10000 |
:---|:---:|:---:|:---:|:---:|---:
Avg Response Time | 2.57ms | 3ms | 2.75ms | 3.35ms | 409.01ms | 
Avg Requests per Second | 1 | 8.5 | 82.3 | 815 | 6550 |
Error Rate % | 0 | 0 | 0 | 0 | 0 |
Peak Response Time | 12.04ms | 26.19ms | 109.85ms | 191.60ms | 2.70s |
CPU Load % | 0.27 | 1.63 | 7.59 | 75.36 | 522.54 |
Memory Usage % | 0.63 | 1.8 | 2.5 | 2.91 | 24.98 |