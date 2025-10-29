var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SrvBack>("srvback");
builder.AddProject<Projects.AppFront>("appfront");

builder.Build().Run();
