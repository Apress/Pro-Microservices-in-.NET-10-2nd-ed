
var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DistanceMicroservice>("distancemicroservice")
    .WithUrlForEndpoint("https", url =>
    {
        url.Url = "/swagger";
    });

builder.AddProject<Projects.GrpcDistance>("grpcdistance");

builder.AddProject<Projects.gRpcTester>("grpctester")
    .WithExplicitStart();

builder.Build().Run();
