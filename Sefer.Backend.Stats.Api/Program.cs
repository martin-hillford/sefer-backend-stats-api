var app = App.Create(args);

app.MapVisitorEndpoints();
app.MapStudentEndpoints();
app.MapEnrollmentEndpoints();
app.MapUserDeviceEndpoints();
app.MapPerformanceEndpoints();

app.Run();