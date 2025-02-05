var builder = DistributedApplication.CreateBuilder(args);

var app1 = builder.AddProject<Projects.DataProtectionDemo1>("dataprotectiondemo1");

var app2 = builder.AddProject<Projects.DataProtectionDemo2>("dataprotectiondemo2");

builder.AddYarp("ingress")
    .WithHttpEndpoint(port: 8001)
    .WithReference(app1)
    .WithReference(app2)
    .LoadFromConfiguration("ReverseProxy");



var app = builder.Build();



app.Run();
