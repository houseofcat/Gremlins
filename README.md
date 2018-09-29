# Gremlins  

### Gremlins In The Code  

Reusable functions that help harden C#/NET systems by purposefully adding stress and errors in a variety of ways.  
Go easy on me, it is a work in progress, with no structure/release plan.  
New needed features -> open a feature Issue!  

### Features  
Supports NET472.  

#### Exceptions  
Gremlins can generate RandomExceptions.  
Gremlins can generate contextual RandomExceptions.  
Gremlins can generate Random System.Exceptions, Random System.Net Exceptions, or Random System.Net & Sql.Exceptions.  

#### Memory  
Gremlins can simulate marshaled memory usage. Can be used to simulate high memory usage.  
Gremlins can simulate CLR/Managed memory usage. Can be used to simulate high memory usage.  

#### CPU  
Gremlins can starve CPU usage and specify the thread priority. Can be used to simulate load on the system or appdomain.  

#### SQL  
Gremlins can starve the SqlConnection pool keeping the connections open internally until released. Can be used to simulate load on the ConnectionPool and how well your application performs when the connectionpool is low on connections.  

#### Monitoring  
Thread monitoring - a variety of ways to to see the number of threads actively engaged in work.  
SqlConnection monitoring - a variety of AdoNetPerformance counters that monitor SqlConnections.  

#### Demo Client  
A Demo client project that should (when finished) demonstrate how best to use all these features.  


### NuGet Info
#### HouseofCat.Gremlins  
[![NuGet](https://img.shields.io/nuget/dt/HouseofCat.Gremlins.svg)](https://www.nuget.org/packages/HouseofCat.Gremlins/) [![NuGet](https://img.shields.io/nuget/v/HouseofCat.Gremlins.svg)](https://www.nuget.org/packages/HouseofCat.Gremlins/)
